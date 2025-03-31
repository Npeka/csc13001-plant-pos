package csc13001.plantpos.product;

import com.fasterxml.jackson.annotation.JsonGetter;
import com.fasterxml.jackson.annotation.JsonIgnore;
import com.fasterxml.jackson.annotation.JsonSetter;
import csc13001.plantpos.category.Category;
import csc13001.plantpos.utils.http.JsonModel;
import jakarta.persistence.*;
import jakarta.validation.constraints.Max;
import jakarta.validation.constraints.Min;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.PositiveOrZero;
import lombok.*;

import java.math.BigDecimal;

@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
@Entity
@Table(name = "products")
@EqualsAndHashCode(callSuper = true)
public class Product extends JsonModel {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "product_id")
    private Long productId;

    @JsonIgnore
    @ManyToOne
    @JoinColumn(name = "category_id", nullable = false)
    private Category category;

    @NotBlank(message = "Tên sản phẩm không được để trống")
    @Column(name = "name", nullable = false)
    private String name;

    @Column(name = "description", columnDefinition = "TEXT", nullable = false)
    private String description;

    @Column(name = "image_url")
    private String imageUrl;

    @NotNull(message = "Giá bán không được để trống")
    @Column(name = "sale_price", columnDefinition = "decimal(18,2)", nullable = false)
    private BigDecimal salePrice;

    @Column(name = "purchase_price", columnDefinition = "decimal(18,2)")
    private BigDecimal purchasePrice;

    @PositiveOrZero(message = "Số lượng tồn kho phải là số không âm")
    @Column(name = "stock", nullable = false)
    private int stock;

    @Builder.Default
    @Min(value = 1, message = "Kích thước phải ít nhất là 1")
    @Max(value = 5, message = "Kích thước phải nhiều nhất là 5")
    @Column(name = "size")
    private int size = 1;

    @Min(value = 1, message = "Mức độ chăm sóc phải ít nhất là 1")
    @Max(value = 5, message = "Mức độ chăm sóc phải nhiều nhất là 5")
    @Column(name = "care_level")
    private int careLevel;

    @Min(value = 1, message = "Yêu cầu ánh sáng phải ít nhất là 1")
    @Max(value = 5, message = "Yêu cầu ánh sáng phải nhiều nhất là 5")
    @Column(name = "light_requirement")
    private int lightRequirement;

    @Min(value = 1, message = "Lịch tưới nước phải ít nhất là 1")
    @Max(value = 5, message = "Lịch tưới nước phải nhiều nhất là 5")
    @Column(name = "watering_schedule")
    private int wateringSchedule;

    @Builder.Default
    @Column(name = "environment_type")
    private String environmentType = "";

    public Product(
            String name, String description, String imageUrl, BigDecimal salePrice, BigDecimal purchasePrice,
            int careLevel,
            String environmentType, int size, int lightRequirement, int wateringSchedule, Category category) {
        this.name = name;
        this.description = description;
        this.imageUrl = imageUrl;
        this.salePrice = salePrice;
        this.purchasePrice = purchasePrice;
        this.stock = 0;
        this.careLevel = careLevel;
        this.environmentType = environmentType;
        this.size = size;
        this.lightRequirement = lightRequirement;
        this.wateringSchedule = wateringSchedule;
        this.category = category;
    }

    @JsonSetter("categoryId")
    public void setCategoryId(Long categoryId) {
        if (category == null) {
            category = new Category();
        }
        category.setCategoryId(categoryId);
    }

    @JsonGetter("categoryId")
    public Long getCategoryId() {
        return (category != null) ? category.getCategoryId() : null;
    }

    // Alias methods for compatibility
    public Long getProductId() {
        return productId;
    }
}
