package csc13001.plantpos.order.models;

import csc13001.plantpos.discount.DiscountProgram;
import csc13001.plantpos.product.Product;
import jakarta.persistence.*;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.math.BigDecimal;

import com.fasterxml.jackson.annotation.JsonIgnore;

@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
@Entity
@Table(name = "order_items")
public class OrderItem {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "order_item_id")
    private Long orderItemId;

    @JsonIgnore
    @ManyToOne
    @JoinColumn(name = "order_id", nullable = false)
    private Order order;

    @ManyToOne
    @JoinColumn(name = "product_id", nullable = false)
    private Product product;

    @Column(nullable = false)
    private Integer quantity;

    @Column(name = "sale_price", nullable = false)
    private BigDecimal salePrice;

    @Column(name = "purchase_price", nullable = false)
    private BigDecimal purchasePrice;

    @ManyToOne
    @JoinColumn(name = "discount_program_id")
    private DiscountProgram discountProgram;

    public OrderItem(Order order, Product product, Integer quantity, BigDecimal salePrice) {
        this.order = order;
        this.product = product;
        this.quantity = quantity;
        this.salePrice = salePrice;
    }
}