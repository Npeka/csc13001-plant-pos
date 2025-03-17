package csc13001.plantpos.domain.models;

import com.fasterxml.jackson.annotation.JsonAutoDetect;
import com.fasterxml.jackson.annotation.JsonInclude;

import jakarta.persistence.*;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.Size;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@NoArgsConstructor
@JsonAutoDetect(fieldVisibility = JsonAutoDetect.Visibility.ANY)
@JsonInclude(JsonInclude.Include.NON_NULL)
@Entity
@Table(name = "categories")
public class Category {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "category_id")
    private Long categoryId;

    @NotBlank(message = "Category is required")
    @Size(max = 100, message = "Category name should not exceed 100 characters")
    @Column(name = "name", nullable = false, unique = true)
    private String name = "";

    @Size(max = 255, message = "Category description should not exceed 255 characters")
    @Column(name = "description")
    private String description;

    public Category(String name, String description) {
        this.name = name;
        this.description = description;
    }
}
