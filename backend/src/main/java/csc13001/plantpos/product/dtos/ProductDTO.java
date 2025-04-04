package csc13001.plantpos.product.dtos;

import csc13001.plantpos.product.Product;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.math.BigDecimal;

@Data
@NoArgsConstructor
@AllArgsConstructor
public class ProductDTO {
    private Product product;
    private Integer salesQuantity;
    private BigDecimal revenue;

    public ProductDTO(Product product, int salesQuantity, BigDecimal revenue) {
        this.product = product;
        this.salesQuantity = salesQuantity;
        this.revenue = revenue;
    }
}