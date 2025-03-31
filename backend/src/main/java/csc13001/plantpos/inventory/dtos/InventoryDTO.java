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
    @NotBlank(message = "Nhà cung cấp là bắt buộc")
    private String supplier;

    @NotNull(message = "Tổng giá là bắt buộc")
    @Min(value = 0, message = "Tổng giá phải lớn hơn 0")
    private BigDecimal totalPrice;

    @NotNull(message = "Ngày mua là bắt buộc")
    private Date purchaseDate;

    @NotEmpty(message = "Phải có ít nhất một sản phẩm")
    private List<InventoryItemDTO> items;

    @Data
    @NoArgsConstructor
    public static class InventoryItemDTO {
        @NotNull(message = "ID sản phẩm là bắt buộc")
        private Long productId;

        @NotNull(message = "Số lượng là bắt buộc")
        @Min(value = 1, message = "Số lượng phải ít nhất là 1")
        private Integer quantity;

        @NotNull(message = "Giá mua là bắt buộc")
        @Min(value = 0, message = "Giá mua phải lớn hơn 0")
        private BigDecimal purchasePrice;
    }
}