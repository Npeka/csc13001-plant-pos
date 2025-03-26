package csc13001.plantpos.order.models;

import csc13001.plantpos.customer.Customer;
import csc13001.plantpos.discount.DiscountProgram;
import csc13001.plantpos.user.User;
import jakarta.persistence.*;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.List;

import com.fasterxml.jackson.annotation.JsonFormat;
import com.fasterxml.jackson.annotation.JsonIgnore;

@Entity
@Table(name = "orders")
@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
public class Order {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "order_id")
    private Long orderId;

    @JsonIgnore
    @ManyToOne
    @JoinColumn(name = "user_id", nullable = false)
    private User staff;

    @ManyToOne
    @JoinColumn(name = "customer_id")
    private Customer customer;

    @Builder.Default
    @Column(name = "order_date", nullable = false)
    @JsonFormat(shape = JsonFormat.Shape.STRING, pattern = "yyyy-MM-dd HH:mm:ss")
    private LocalDateTime orderDate = LocalDateTime.now();

    @ManyToOne
    @JoinColumn(name = "discount_id")
    private DiscountProgram discountProgram;

    @Column(name = "total_price", nullable = false)
    private BigDecimal totalPrice;

    @Column(name = "final_price", nullable = false)
    private BigDecimal finalPrice;

    @OneToMany(mappedBy = "order", cascade = CascadeType.ALL)
    private List<OrderItem> items;

    public Long getOrderId() {
        return orderId;
    }

    public Long getCustomerId() {
        return customer != null ? customer.getCustomerId() : null;
    }
}
