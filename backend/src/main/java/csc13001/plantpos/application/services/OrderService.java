package csc13001.plantpos.application.services;

import csc13001.plantpos.adapters.repositories.*;
import csc13001.plantpos.application.dtos.order.CreateOrderDTO;
import csc13001.plantpos.application.dtos.order.OrderDetailDTO;
import csc13001.plantpos.application.dtos.order.UpdateOrderDTO;
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
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.util.List;

@Service
@RequiredArgsConstructor
public class OrderService {
    private final CustomerService customerService;
    private final StaffRepository staffRepository;
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
        Customer customer = customerService.createCustomer(Customer.builder()
                .phone(createOrderDTO.getCustomerPhone())
                .build());

        if (!staffRepository.existsById(createOrderDTO.getStaffId())) {
            throw new StaffException.StaffNotFoundException();
        }

        Order order = Order.builder()
                .customerId(customer.getCustomerId())
                .staffId(createOrderDTO.getStaffId())
                .totalPrice(createOrderDTO.getTotalPrice())
                .status(createOrderDTO.getStatus())
                .build();

        Order savedOrder = orderRepository.save(order);
        this.createOrderDetailsAsync(createOrderDTO, savedOrder);

        // eventPublisher.publishEvent(
        // new OrderCreatedEvent(
        // savedOrder.getOrderId(),
        // savedOrder.getStaff().getStaffId()));

        return savedOrder;
    }

    public void createOrderDetailsAsync(CreateOrderDTO createOrderDTO, Order savedOrder) {
        for (OrderDetailDTO orderDetailDTO : createOrderDTO.getItems()) {
            Product product = productRepository.findById(orderDetailDTO.getProductId())
                    .orElseThrow(ProductException.ProductNotFoundException::new);

            Long discountId = orderDetailDTO.getDiscountId();
            DiscountProgram discountProgram = null;
            if (discountId != null) {
                discountProgram = discountProgramRepository.findById(discountId)
                        .orElseThrow(DiscountException.DiscountNotFoundException::new);
            }

            BigDecimal unitPrice = product.getPrice();

            OrderDetail orderDetail = OrderDetail.builder()
                    .orderId(savedOrder.getOrderId())
                    .productId(orderDetailDTO.getProductId())
                    .quantity(orderDetailDTO.getQuantity())
                    .unitPrice(unitPrice)
                    .discountId(discountId)
                    .build();

            orderDetailRepository.save(orderDetail);
        }
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

    public void deleteOrder(Long orderId) {
        try {
            orderRepository.deleteById(orderId);
        } catch (EmptyResultDataAccessException e) {
            throw new OrderException.OrderNotFoundException();
        }
    }

}
