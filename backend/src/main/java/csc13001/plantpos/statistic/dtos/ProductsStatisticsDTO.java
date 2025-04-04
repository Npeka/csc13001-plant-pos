package csc13001.plantpos.statistic.dtos;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.util.List;

import csc13001.plantpos.product.Product;
import csc13001.plantpos.product.dtos.ProductDTO;

@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
public class ProductsStatisticsDTO {
    private List<ProductDTO> topSellingProducts;
    private List<Product> lowStockProducts;
}
