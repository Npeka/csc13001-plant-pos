package csc13001.plantpos.application.services;

import csc13001.plantpos.adapters.repositories.*;
import csc13001.plantpos.application.dtos.order.CreateOrderDTO;
import csc13001.plantpos.application.dtos.order.OrderDetailDTO;
import csc13001.plantpos.application.dtos.order.UpdateOrderDTO;
import csc13001.plantpos.domain.enums.OrderStatus;
import csc13001.plantpos.domain.models.Customer;
import csc13001.plantpos.domain.models.DiscountProgram;
import csc13001.plantpos.domain.models.Order;
import csc13001.plantpos.domain.models.OrderDetail;
import csc13001.plantpos.domain.models.Product;
import csc13001.plantpos.exception.discount.DiscountException;
import csc13001.plantpos.exception.order.OrderException;
import csc13001.plantpos.exception.product.ProductException;
import csc13001.plantpos.exception.staff.StaffException;
import lombok.RequiredArgsConstructor;
import org.springframework.context.ApplicationEventPublisher;
import org.springframework.dao.EmptyResultDataAccessException;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Propagation;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.util.List;

@Service
@RequiredArgsConstructor
public class OrderService {
    private final CustomerService customerService;
    private final InventoryService inventoryService;
    private final UserRepository userRepository;
    private final OrderRepository orderRepository;
    private final OrderDetailRepository orderDetailRepository;
    private final ProductRepository productRepository;
    private final DiscountProgramRepository discountProgramRepository;
    private final ApplicationEventPublisher eventPublisher;

    public List<Order> getAllOrders() {
        return orderRepository.findAll();
    }

    @Transactional
    public Order createOrder(CreateOrderDTO createOrderDTO) {
        Customer customer = customerService.createCustomerIfNotExists(Customer.builder()
                .phone(createOrderDTO.getCustomerPhone())
                .build());

        if (!userRepository.existsById(createOrderDTO.getStaffId())) {
            throw new StaffException.StaffNotFoundException();
        }

        Order order = Order.builder()
                .customerId(customer.getCustomerId())
                .staffId(createOrderDTO.getStaffId())
                .status(OrderStatus.PENDING)
                .totalPrice(BigDecimal.ZERO)
                .build();

        Order savedOrder = orderRepository.save(order);
        BigDecimal totalPrice = this.createOrderDetails(createOrderDTO, savedOrder);
        savedOrder.setTotalPrice(totalPrice);

        // eventPublisher.publishEvent(
        // new OrderCreatedEvent(
        // savedOrder.getOrderId(),
        // savedOrder.getStaff().getStaffId()));

        return savedOrder;
    }

    @Transactional(propagation = Propagation.REQUIRES_NEW)
    public BigDecimal createOrderDetails(CreateOrderDTO createOrderDTO, Order savedOrder) {
        BigDecimal totalPrice = BigDecimal.ZERO;

        for (OrderDetailDTO orderDetailDTO : createOrderDTO.getItems()) {
            Product product = productRepository.findById(orderDetailDTO.getProductId())
                    .orElseThrow(ProductException.ProductNotFoundException::new);

            inventoryService.updateStock(product.getProductId(), orderDetailDTO.getQuantity());

            Long discountId = orderDetailDTO.getDiscountId();
            DiscountProgram discountProgram = null;
            if (discountId != null) {
                discountProgram = discountProgramRepository.findById(discountId)
                        .orElseThrow(DiscountException.DiscountNotFoundException::new);
            }

            BigDecimal unitPrice = product.getPrice();
            totalPrice = totalPrice.add(unitPrice);

            OrderDetail orderDetail = OrderDetail.builder()
                    .orderId(savedOrder.getOrderId())
                    .productId(orderDetailDTO.getProductId())
                    .quantity(orderDetailDTO.getQuantity())
                    .unitPrice(unitPrice)
                    .discountId(discountId)
                    .build();

            orderDetailRepository.save(orderDetail);
        }

        return totalPrice;
    }

    public Order getOrderById(Long orderId) {
        Order order = orderRepository.findById(orderId)
                .orElseThrow(OrderException.OrderNotFoundException::new);
        return order;
    }

    public Order updateOrder(UpdateOrderDTO updateOrderDTO) {
        Order order = orderRepository.findById(updateOrderDTO.getId())
                .orElseThrow(OrderException.OrderNotFoundException::new);

        order.setStatus(updateOrderDTO.getStatus());
        return orderRepository.save(order);
    }

    @Transactional
    public Order updateOrderStatus(Long orderId, OrderStatus status) {
        Order order = orderRepository.findById(orderId)
                .orElseThrow(OrderException.OrderNotFoundException::new);

        if (status == OrderStatus.CANCELED && order.getStatus() != OrderStatus.CANCELED) {
            List<OrderDetail> orderDetails = orderDetailRepository.findByOrderId(orderId);
            for (OrderDetail orderDetail : orderDetails) {
                Product product = productRepository.findById(orderDetail.getProductId())
                        .orElseThrow(ProductException.ProductNotFoundException::new);
                inventoryService.updateStock(product.getProductId(), -orderDetail.getQuantity());
            }
        }

        order.setStatus(status);
        return orderRepository.save(order);
    }

    public void deleteOrder(Long orderId) {
        try {
            orderRepository.deleteById(orderId);
        } catch (EmptyResultDataAccessException e) {
            throw new OrderException.OrderNotFoundException();
        }
    }
}
