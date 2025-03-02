package csc13001.plantpos.domain.models;

import jakarta.persistence.*;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.math.BigDecimal;
import java.util.HashSet;
import java.util.Set;

@Data
@NoArgsConstructor
@Entity
@Table(name = "products")
public class Product {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "product_id")
    private Long productId;

    @ManyToMany
    @JoinTable(
            name = "product_category",
            joinColumns = @JoinColumn(name = "product_id"),
            inverseJoinColumns = @JoinColumn(name = "category_id"))
    private Set<Category> categories = new HashSet<>();

    @Column(name = "name")
    private String name = "";

    @Column(name = "description")
    private String description;

    @Column(name = "price", columnDefinition = "decimal(18,2)")
    private BigDecimal price;

    @Column(name = "stock")
    private int stock;

    @Column(name = "care_level")
    private int careLevel;

    @Column(name = "environment_type")
    private String environmentType = "";

    @Column(name = "size")
    private String size = "";

    @Column(name = "light_requirement")
    private int lightRequirement;

    @Column(name = "watering_schedule")
    private int wateringSchedule;
}
