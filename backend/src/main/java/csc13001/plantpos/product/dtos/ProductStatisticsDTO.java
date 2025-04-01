package csc13001.plantpos.product.dtos;

import csc13001.plantpos.category.Category;
import csc13001.plantpos.product.Product;
import jakarta.validation.constraints.Max;
import jakarta.validation.constraints.Min;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.PositiveOrZero;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.math.BigDecimal;

@Data
@NoArgsConstructor
@AllArgsConstructor
public class ProductStatisticsDTO {
    private ProductDTO product;
    private Integer salesQuantity;
    private BigDecimal revenue;

    @Data
    @NoArgsConstructor
    @AllArgsConstructor
    public static class ProductDTO {
        private Long productId;
        private Category category;
        private String name;
        private String description;
        private String imageUrl;
        private BigDecimal salePrice;
        private BigDecimal purchasePrice;
        private int stock;
        private int size = 1;
        private int careLevel;
        private int lightRequirement;
        private int wateringSchedule;
        private String environmentType = "";
    }

    public ProductStatisticsDTO(Product product, int salesQuantity, BigDecimal revenue) {
        this.product = new ProductDTO(
                product.getProductId(),
                product.getCategory(),
                product.getName(),
                product.getDescription(),
                product.getImageUrl(),
                product.getSalePrice(),
                product.getPurchasePrice(),
                product.getStock(),
                product.getSize(),
                product.getCareLevel(),
                product.getLightRequirement(),
                product.getWateringSchedule(),
                product.getEnvironmentType());
        this.salesQuantity = salesQuantity;
        this.revenue = revenue;
    }
}