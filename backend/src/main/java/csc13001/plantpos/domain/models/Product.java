package csc13001.plantpos.domain.models;

import com.fasterxml.jackson.annotation.JsonIgnore;
import csc13001.plantpos.utils.http.JsonModel;
import jakarta.persistence.*;
import jakarta.validation.constraints.Min;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.PositiveOrZero;
import lombok.Data;
import lombok.EqualsAndHashCode;
import lombok.NoArgsConstructor;

import java.math.BigDecimal;

@Data
@NoArgsConstructor
@Entity
@Table(name = "products")
@EqualsAndHashCode(callSuper = true)
public class Product extends JsonModel {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "product_id")
    private Long productId;

    @JsonIgnore
    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "category_id", nullable = false)
    private Category category;

    @NotBlank(message = "Product name cannot be blank")
    @Column(name = "name")
    private String name = "";

    @Column(name = "description")
    private String description;

    @Column(name = "image")
    private String imageUrl;

    @NotNull(message = "Price cannot be null")
    @Column(name = "price", columnDefinition = "decimal(18,2)")
    private BigDecimal price;

    @PositiveOrZero(message = "Stock must be a non-negative number")
    @Column(name = "stock")
    private int stock;

    @Min(value = 0, message = "Care level must be at least 0")
    @Column(name = "care_level")
    private int careLevel;

    @Column(name = "environment_type")
    private String environmentType = "";

    @Column(name = "size")
    private String size = "";

    @Min(value = 0, message = "Light requirement must be at least 0")
    @Column(name = "light_requirement")
    private int lightRequirement;

    @Min(value = 0, message = "Watering schedule must be at least 0")
    @Column(name = "watering_schedule")
    private int wateringSchedule;

    public Product(
            String name, String description, String imageUrl, BigDecimal price, int stock, int careLevel,
            String environmentType, String size, int lightRequirement, int wateringSchedule, Category category) {
        this.name = name;
        this.description = description;
        this.imageUrl = imageUrl;
        this.price = price;
        this.stock = stock;
        this.careLevel = careLevel;
        this.environmentType = environmentType;
        this.size = size;
        this.lightRequirement = lightRequirement;
        this.wateringSchedule = wateringSchedule;
        this.category = category;
    }
}