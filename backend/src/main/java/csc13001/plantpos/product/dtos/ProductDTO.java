package csc13001.plantpos.product.dtos;

import csc13001.plantpos.product.Product;
import csc13001.plantpos.utils.http.JsonModel;
import jakarta.validation.constraints.Min;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;
import lombok.Data;
import lombok.EqualsAndHashCode;
import lombok.NoArgsConstructor;

import java.math.BigDecimal;

@Data
@NoArgsConstructor
@EqualsAndHashCode(callSuper = true)
public class ProductDTO extends JsonModel {
    private Long productId;

    @NotBlank(message = "Product name cannot be blank")
    private String name;

    private String description;

    private String imageUrl;

    @NotNull(message = "Price cannot be null")
    private BigDecimal salePrice;

    private int stock;

    @Min(value = 0, message = "Care level must be at least 0")
    private int careLevel;

    private String environmentType;

    private int size;

    @Min(value = 0, message = "Light requirement must be at least 0")
    private int lightRequirement;

    @Min(value = 0, message = "Watering schedule must be at least 0")
    private int wateringSchedule;

    @NotBlank(message = "Category name cannot be blank")
    private String categoryName;

    public ProductDTO(Product product) {
        this.productId = product.getProductId();
        this.name = product.getName();
        this.description = product.getDescription();
        this.imageUrl = product.getImageUrl();
        this.salePrice = product.getSalePrice();
        this.stock = product.getStock();
        this.careLevel = product.getCareLevel();
        this.environmentType = product.getEnvironmentType();
        this.size = product.getSize();
        this.lightRequirement = product.getLightRequirement();
        this.wateringSchedule = product.getWateringSchedule();
        this.categoryName = (product.getCategory() != null) ? product.getCategory().getName() : null;
    }
}
