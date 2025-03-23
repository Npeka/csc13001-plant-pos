package csc13001.plantpos.inventory.models;

import jakarta.persistence.*;
import jakarta.validation.constraints.Min;
import jakarta.validation.constraints.NotBlank;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.math.BigDecimal;
import java.util.Date;

@Entity
@Table(name = "inventory")
@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
public class Inventory {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "inventory_id")
    private Long inventoryId;

    @NotBlank(message = "Supplier is required")
    @Column(name = "supplier", nullable = false)
    private String supplier;

    @Min(value = 0, message = "Total price must be positive")
    @Column(name = "total_price", nullable = false)
    private BigDecimal totalPrice;

    @Column(name = "purchase_date", nullable = false)
    private Date purchaseDate;
}