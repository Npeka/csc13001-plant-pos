package csc13001.plantpos.statistic;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.time.LocalDateTime;
import java.util.List;
import java.util.stream.Collectors;

import org.springframework.context.ApplicationEventPublisher;
import org.springframework.scheduling.annotation.Scheduled;
import org.springframework.stereotype.Service;

import csc13001.plantpos.order.OrderItemRepository;
import csc13001.plantpos.order.OrderRepository;
import csc13001.plantpos.order.models.Order;
import csc13001.plantpos.order.models.OrderItem;
import csc13001.plantpos.product.Product;
import csc13001.plantpos.product.ProductRepository;
import csc13001.plantpos.product.dtos.ProductDTO;
import csc13001.plantpos.statistic.dtos.ProductsStatisticsDTO;
import csc13001.plantpos.statistic.dtos.SalesStatisticsDTO;
import lombok.RequiredArgsConstructor;

@Service
@RequiredArgsConstructor
public class StatisticsService {
	private final ProductRepository productRepository;
	private final OrderRepository orderRepository;
	private final OrderItemRepository orderItemRepository;
	private final ApplicationEventPublisher eventPublisher;

	public SalesStatisticsDTO getSalesStatistics(
			TimeType timeType,
			LocalDateTime startDate,
			LocalDateTime endDate) {
		// Lấy đơn hàng trong khoảng thời gian
		List<Order> orders = orderRepository.findAllByOrderDateBetween(startDate, endDate);

		// Lấy đơn hàng của kỳ trước (năm trước)
		LocalDateTime preStartDate;
		LocalDateTime preEndDate;
		if (timeType == TimeType.DAILY) {
			preStartDate = startDate.minusDays(1);
			preEndDate = endDate.minusDays(1);
		} else if (timeType == TimeType.MONTHLY) {
			preStartDate = startDate.minusMonths(1);
			preEndDate = endDate.minusMonths(1);
		} else {
			preStartDate = startDate.minusYears(1);
			preEndDate = endDate.minusYears(1);
		}
		List<Order> preOrders = orderRepository.findAllByOrderDateBetween(preStartDate, preEndDate);

		// Tính toán chỉ số cho kỳ hiện tại
		BigDecimal revenue = calculateTotalRevenue(orders);
		BigDecimal profit = calculateTotalProfit(orders);
		long orderCount = orders.size();

		// Tính toán chỉ số cho kỳ trước
		BigDecimal preRevenue = calculateTotalRevenue(preOrders);
		BigDecimal preProfit = calculateTotalProfit(preOrders);
		long preOrderCount = preOrders.size();

		// Tính tỷ lệ tăng trưởng
		BigDecimal revenueGrowthRate = calculateGrowthRate(revenue, preRevenue);
		BigDecimal profitGrowthRate = calculateGrowthRate(profit, preProfit);
		long orderCountGrowthRate = preOrderCount == 0 ? 0
				: ((orderCount - preOrderCount) * 100) / preOrderCount;

		// Nhóm doanh thu theo thời gian
		List<SalesStatisticsDTO.TimeSeriesRevenue> timeSeriesRevenues = groupRevenueByTime(orders, timeType);

		return SalesStatisticsDTO.builder()
				.revenue(revenue)
				.revenueGrowthRate(revenueGrowthRate)
				.profit(profit)
				.profitGrowthRate(profitGrowthRate)
				.orderCount(orderCount)
				.orderCountGrowthRate(orderCountGrowthRate)
				.timeSeriesRevenues(timeSeriesRevenues)
				.build();
	}

	/** 📌 Tính tổng doanh thu */
	private BigDecimal calculateTotalRevenue(List<Order> orders) {
		return orders.stream()
				.map(Order::getFinalPrice)
				.reduce(BigDecimal.ZERO, BigDecimal::add);
	}

	/** 📌 Tính tổng lợi nhuận */
	private BigDecimal calculateTotalProfit(List<Order> orders) {
		return orders.stream()
				.map(order -> order.getFinalPrice().subtract(calculateTotalCost(order)))
				.reduce(BigDecimal.ZERO, BigDecimal::add);
	}

	/** 📌 Tính tổng chi phí của một đơn hàng */
	private BigDecimal calculateTotalCost(Order order) {
		return orderItemRepository.findByOrder_OrderId(order.getOrderId()).stream()
				.map(item -> item.getPurchasePrice().multiply(BigDecimal.valueOf(item.getQuantity())))
				.reduce(BigDecimal.ZERO, BigDecimal::add);
	}

	/** 📌 Nhóm doanh thu theo thời gian */
	private List<SalesStatisticsDTO.TimeSeriesRevenue> groupRevenueByTime(List<Order> orders, TimeType timeType) {
		return orders.stream()
				.collect(Collectors.groupingBy(
						order -> getTimeKey(order.getOrderDate(), timeType),
						Collectors.reducing(BigDecimal.ZERO, Order::getFinalPrice,
								BigDecimal::add)))
				.entrySet().stream()
				.map(entry -> new SalesStatisticsDTO.TimeSeriesRevenue(entry.getKey(),
						entry.getValue()))
				.collect(Collectors.toList());
	}

	/** 📌 Lấy key thời gian theo loại thống kê */
	private String getTimeKey(LocalDateTime orderDateTime, TimeType timeType) {
		switch (timeType) {
			case DAILY:
				return orderDateTime.toLocalDate() + " "
						+ String.format("%02d:00", orderDateTime.getHour());
			case MONTHLY:
				return orderDateTime.getYear() + "-"
						+ String.format("%02d", orderDateTime.getMonthValue()) + "-" +
						String.format("%02d", orderDateTime.getDayOfMonth());
			case YEARLY:
				return orderDateTime.getYear() + "-"
						+ String.format("%02d", orderDateTime.getMonthValue());
			default:
				throw new IllegalArgumentException("Invalid time type");
		}
	}

	/** 📌 Tính tỷ lệ tăng trưởng */
	private BigDecimal calculateGrowthRate(BigDecimal current, BigDecimal previous) {
		return previous.compareTo(BigDecimal.ZERO) == 0 ? BigDecimal.ZERO
				: current.subtract(previous).divide(previous, 4, RoundingMode.HALF_UP)
						.multiply(BigDecimal.valueOf(100));
	}

	public List<ProductDTO> topSellingProducts(Integer limit) {
		List<Product> products = productRepository.findAll();

		return products.stream()
				.map(product -> {
					List<OrderItem> orderItems = orderItemRepository
							.findByProduct_ProductId(product.getProductId());

					int salesQuantity = orderItems.stream()
							.mapToInt(OrderItem::getQuantity)
							.sum();

					BigDecimal totalRevenue = orderItems.stream()
							.map(item -> item.getSalePrice().multiply(
									BigDecimal.valueOf(item.getQuantity())))
							.reduce(BigDecimal.ZERO, BigDecimal::add);

					return new ProductDTO(product, salesQuantity, totalRevenue);
				})
				.sorted((p1, p2) -> Integer.compare(p2.getSalesQuantity(), p1.getSalesQuantity()))
				.limit(limit != null ? limit : Long.MAX_VALUE)
				.collect(Collectors.toList());
	}

	public ProductsStatisticsDTO getProductStatisticsReview() {
		List<Product> products = productRepository.findAll();

		List<ProductDTO> topSellingProducts = topSellingProducts(4);
		List<Product> lowStockProducts = products.stream()
				.sorted((p1, p2) -> Integer.compare(p1.getStock(), p2.getStock()))
				.limit(4)
				.collect(Collectors.toList());

		return ProductsStatisticsDTO.builder()
				.topSellingProducts(topSellingProducts)
				.lowStockProducts(lowStockProducts)
				.build();
	}

	@Scheduled(cron = "0 0 20 * * *")
	public void updateProductStatistics() {
		SalesStatisticsDTO salesStatisticsDTO = this.getSalesStatistics(TimeType.DAILY,
				LocalDateTime.now().minusDays(1),
				LocalDateTime.now());

		this.eventPublisher.publishEvent(salesStatisticsDTO);
	}
}
