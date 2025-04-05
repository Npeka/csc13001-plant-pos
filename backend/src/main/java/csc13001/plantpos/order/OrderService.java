package csc13001.plantpos.order;

import csc13001.plantpos.customer.Customer;
import csc13001.plantpos.customer.CustomerRepository;
import csc13001.plantpos.customer.CustomerType;
import csc13001.plantpos.customer.exception.CustomerException;
import csc13001.plantpos.discount.DiscountProgram;
import csc13001.plantpos.discount.DiscountProgramRepository;
import csc13001.plantpos.discount.DiscountUsageRepository;
import csc13001.plantpos.discount.exception.DiscountException;
import csc13001.plantpos.discount.models.DiscountUsage;
import csc13001.plantpos.inventory.InventoryItemRepository;
import csc13001.plantpos.inventory.exception.InventoryException;
import csc13001.plantpos.inventory.models.InventoryItem;
import csc13001.plantpos.order.dtos.CreateOrderDTO;
import csc13001.plantpos.order.dtos.OrderItemlDTO;
import csc13001.plantpos.order.exception.OrderException;
import csc13001.plantpos.order.models.Order;
import csc13001.plantpos.order.models.OrderItem;
import csc13001.plantpos.product.Product;
import csc13001.plantpos.product.ProductRepository;
import csc13001.plantpos.product.exception.ProductException;
import csc13001.plantpos.staff.exception.StaffException;
import csc13001.plantpos.user.User;
import csc13001.plantpos.user.UserRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.dao.EmptyResultDataAccessException;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Propagation;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.Date;
import java.util.List;

@Service
@RequiredArgsConstructor
public class OrderService {
    private final CustomerRepository customerRepository;
    private final InventoryItemRepository inventoryItemRepository;
    private final UserRepository userRepository;
    private final OrderRepository orderRepository;
    private final OrderItemRepository orderItemReopsitory;
    private final ProductRepository productRepository;
    private final DiscountProgramRepository discountProgramRepository;
    private final DiscountUsageRepository discountUsageRepository;

    public List<Order> getAllOrders() {
        return orderRepository.findAll();
    }

    @Transactional
    public Order createOrder(CreateOrderDTO createOrderDTO) {
        // check if customer exists
        Customer customer = null;
        if (createOrderDTO.getCustomerPhone() != null && !createOrderDTO.getCustomerPhone().isEmpty()) {
            customer = customerRepository.findByPhone(createOrderDTO.getCustomerPhone())
                    .orElseThrow(CustomerException.CustomerNotFoundException::new);
        }

        // check if discount exists
        DiscountProgram discountProgram = null;
        if (createOrderDTO.getDiscountId() != null) {
            discountProgram = discountProgramRepository.findById(createOrderDTO.getDiscountId())
                    .orElseThrow(DiscountException.DiscountNotFoundException::new);

            // check if discount is already applied
            if (discountUsageRepository.existsByCustomer_CustomerIdAndDiscountProgram_DiscountId(
                    customer.getCustomerId(),
                    discountProgram.getDiscountId())) {
                throw new DiscountException.DiscountAlreadyAppliedException();
            }

            // check if discount is active
            if (!discountProgram.isActive()) {
                throw new DiscountException.DiscountNotActiveException();
            }

            // check if discount is applicable
            if (discountProgram.getApplicableCustomerType() != CustomerType.All) {
                if (customer != null && customer.getLoyaltyCardType()
                        .isLowerThan(discountProgram.getApplicableCustomerType())) {
                    throw new DiscountException.DiscountNotApplicableException();
                }
            }

            DiscountUsage usage = DiscountUsage.builder()
                    .customer(customer)
                    .discountProgram(discountProgram)
                    .usedAt(LocalDateTime.now())
                    .build();

            discountUsageRepository.save(usage);
        }

        // check if staff exists
        User staff = userRepository.findById(createOrderDTO.getStaffId())
                .orElseThrow(StaffException.StaffNotFoundException::new);

        // create order
        Order order = Order.builder()
                .customer(customer)
                .staff(staff)
                .discountProgram(discountProgram)
                .totalPrice(BigDecimal.ZERO)
                .finalPrice(BigDecimal.ZERO)
                .build();
        Order savedOrder = orderRepository.save(order);

        // create order details
        // calculate total price and final price
        BigDecimal totalPrice = this.createOrderDetails(createOrderDTO, savedOrder);
        BigDecimal finalPrice = totalPrice;

        if (discountProgram != null) {
            BigDecimal discountRate = BigDecimal.valueOf(discountProgram.getDiscountRate());
            finalPrice = totalPrice.multiply(BigDecimal.valueOf(100).subtract(discountRate))
                    .divide(BigDecimal.valueOf(100), RoundingMode.HALF_UP);
        }

        // update order
        savedOrder.setTotalPrice(totalPrice);
        savedOrder.setFinalPrice(finalPrice);
        orderRepository.save(savedOrder);

        // earn points for customer
        if (customer != null) {
            customer.addLoyaltyPointsBySpending(finalPrice);
            customerRepository.save(customer);
        }

        return savedOrder;
    }

    @Transactional(propagation = Propagation.REQUIRES_NEW)
    public BigDecimal createOrderDetails(CreateOrderDTO createOrderDTO, Order savedOrder) {
        BigDecimal totalPrice = BigDecimal.ZERO;

        for (OrderItemlDTO orderItemDTO : createOrderDTO.getItems()) {
            // check if product exists
            Product product = productRepository.findById(orderItemDTO.getProductId())
                    .orElseThrow(ProductException.ProductNotFoundException::new);

            // check if stock is enough
            int quantityChange = orderItemDTO.getQuantity();
            if (product.getStock() < quantityChange) {
                throw new InventoryException.InventoryNotEnoughStockException();
            }

            // get inventory items by product id
            List<InventoryItem> inventoryItems = inventoryItemRepository.findOldestInventoryItem(product.getProductId())
                    .orElseThrow(InventoryException.InventoryNotFoundException::new);

            // update inventory items and calculate total purchase price
            BigDecimal totalPurchasePrice = BigDecimal.ZERO;
            for (InventoryItem inventoryItem : inventoryItems) {
                if (quantityChange <= 0) {
                    break;
                }

                // update inventory item
                int usedQuantity = Math.min(inventoryItem.getRemainingQuantity(), quantityChange);
                inventoryItem.setRemainingQuantity(inventoryItem.getRemainingQuantity() - usedQuantity);
                inventoryItemRepository.save(inventoryItem);

                // save the last purchase price
                product.setPurchasePrice(inventoryItem.getPurchasePrice());

                // calculate total purchase price
                BigDecimal purchasePrice = inventoryItem.getPurchasePrice();
                totalPurchasePrice = totalPurchasePrice.add(purchasePrice.multiply(BigDecimal.valueOf(usedQuantity)));

                // update quantity change
                quantityChange -= usedQuantity;
            }

            // update product stock
            int quantity = orderItemDTO.getQuantity();
            product.setStock(product.getStock() - quantity);
            productRepository.save(product);

            // calculate average purchase price
            BigDecimal salePrice = product.getSalePrice();
            BigDecimal avgPurchasePrice = totalPurchasePrice
                    .divide(BigDecimal.valueOf(quantity), RoundingMode.HALF_UP);

            OrderItem orderDetail = OrderItem.builder()
                    .order(savedOrder)
                    .product(product)
                    .quantity(quantity)
                    .salePrice(salePrice)
                    .purchasePrice(avgPurchasePrice)
                    .build();

            orderItemReopsitory.save(orderDetail);
            totalPrice = totalPrice.add(salePrice.multiply(BigDecimal.valueOf(quantity)));
        }

        return totalPrice;
    }

    public Order getOrderById(Long orderId) {
        return orderRepository.findById(orderId)
                .orElseThrow(OrderException.OrderNotFoundException::new);
    }

    public List<Order> getOrdersByCustomerId(Long customerId) {
        if (!customerRepository.existsById(customerId)) {
            throw new CustomerException.CustomerNotFoundException();
        }
        return orderRepository.findByCustomer_CustomerId(customerId);
    }

    public List<Order> getOrdersByStaffId(Long staffId) {
        if (!userRepository.existsById(staffId)) {
            throw new StaffException.StaffNotFoundException();
        }
        return orderRepository.findByStaff_UserId(staffId);
    }

    public void deleteOrder(Long orderId) {
        try {
            orderRepository.deleteById(orderId);
        } catch (EmptyResultDataAccessException e) {
            throw new OrderException.OrderNotFoundException();
        }
    }
}
