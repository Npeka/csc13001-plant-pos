package csc13001.plantpos.domain.models;

import com.fasterxml.jackson.annotation.JsonAutoDetect;
import com.fasterxml.jackson.annotation.JsonInclude;
import jakarta.persistence.*;
import jakarta.validation.constraints.Min;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.PositiveOrZero;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.math.BigDecimal;
import java.util.HashSet;
import java.util.Set;

@Data
@NoArgsConstructor
@JsonAutoDetect(fieldVisibility = JsonAutoDetect.Visibility.ANY)
@JsonInclude(JsonInclude.Include.NON_NULL)
@Entity
@Table(name = "products")
public class Product {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "product_id")
    private Long productId;

    @ManyToMany(cascade = { CascadeType.MERGE, CascadeType.PERSIST }, fetch = FetchType.EAGER)
    @JoinTable(name = "product_category", joinColumns = @JoinColumn(name = "product_id"), inverseJoinColumns = @JoinColumn(name = "category_id"))
    private Set<Category> categories = new HashSet<>();

    @NotBlank(message = "Product name cannot be blank")
    @Column(name = "name")
    private String name = "";

    @Column(name = "description")
    private String description;

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

    public Product(String name, String description, BigDecimal price, int stock, int careLevel, String environmentType,
            String size, int lightRequirement, int wateringSchedule, Set<Category> categories) {
        this.name = name;
        this.description = description;
        this.price = price;
        this.stock = stock;
        this.careLevel = careLevel;
        this.environmentType = environmentType;
        this.size = size;
        this.lightRequirement = lightRequirement;
        this.wateringSchedule = wateringSchedule;
        this.categories = categories;
    }
}