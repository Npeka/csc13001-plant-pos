package csc13001.plantpos.statistic;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.time.LocalDateTime;
import java.util.List;
import java.util.stream.Collectors;

import org.springframework.stereotype.Service;

import csc13001.plantpos.order.OrderItemRepository;
import csc13001.plantpos.order.OrderRepository;
import csc13001.plantpos.order.models.Order;
import csc13001.plantpos.statistic.dtos.SalesStatisticsDTO;
import lombok.RequiredArgsConstructor;

@Service
@RequiredArgsConstructor
public class StatisticsService {
    private final OrderRepository orderRepository;
    private final OrderItemRepository orderItemRepository;

    public SalesStatisticsDTO getSalesStatistics(TimeType timeType, LocalDateTime startDate,
            LocalDateTime endDate) {
        // Lấy đơn hàng trong khoảng thời gian
        List<Order> orders = orderRepository.findAllByOrderDateBetween(startDate, endDate);

        // Lấy đơn hàng của kỳ trước (năm trước)
        List<Order> preOrders = orderRepository.findAllByOrderDateBetween(startDate.minusYears(1),
                endDate.minusYears(1));

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
                : current.subtract(previous).divide(previous, 4, RoundingMode.HALF_UP);
    }
}
