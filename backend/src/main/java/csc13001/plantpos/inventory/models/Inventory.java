package csc13001.plantpos.inventory.models;

import jakarta.persistence.*;
import jakarta.validation.constraints.Min;
import jakarta.validation.constraints.NotBlank;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.math.BigDecimal;
import java.time.LocalDate;
import java.util.List;

import com.fasterxml.jackson.annotation.JsonFormat;

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

    @NotBlank(message = "Nhà cung cấp là bắt buộc")
    @Column(name = "supplier", nullable = false)
    private String supplier;

    @Min(value = 0, message = "Tổng giá phải lớn hơn 0")
    @Column(name = "total_price", nullable = false)
    private BigDecimal totalPrice;

    @JsonFormat(shape = JsonFormat.Shape.STRING)
    @Column(name = "purchase_date", columnDefinition = "DATE", nullable = false)
    private LocalDate purchaseDate;

    @OneToMany(mappedBy = "inventory", cascade = CascadeType.ALL)
    private List<InventoryItem> items;
}