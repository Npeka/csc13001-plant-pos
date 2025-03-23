package csc13001.plantpos.inventory.dtos;

import jakarta.validation.constraints.Min;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotEmpty;
import jakarta.validation.constraints.NotNull;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.math.BigDecimal;
import java.util.Date;
import java.util.List;

@Data
@NoArgsConstructor
public class InventoryDTO {
    @NotBlank(message = "Supplier is required")
    private String supplier;

    @NotNull(message = "Total price is required")
    @Min(value = 0, message = "Total price must be positive")
    private BigDecimal totalPrice;

    @NotNull(message = "Purchase date is required")
    private Date purchaseDate;

    @NotEmpty(message = "At least one product is required")
    private List<InventoryItemDTO> items;

    @Data
    @NoArgsConstructor
    public static class InventoryItemDTO {
        @NotNull(message = "Product ID is required")
        private Long productId;

        @NotNull(message = "Quantity is required")
        @Min(value = 1, message = "Quantity must be at least 1")
        private Integer quantity;

        @NotNull(message = "Purchase price is required")
        @Min(value = 0, message = "Purchase price must be positive")
        private BigDecimal purchasePrice;
    }
}