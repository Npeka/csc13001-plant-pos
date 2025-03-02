package csc13001.plantpos.application.services;

import csc13001.plantpos.adapters.repositories.ProductRepository;
import csc13001.plantpos.domain.models.Product;
import csc13001.plantpos.exception.product.ProductException;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import java.util.List;

@Service
public class ProductService {
    @Autowired
    private ProductRepository productRepository;

    public List<Product> getAllProducts() {
        return productRepository.findAll();
    }

    public Product createProduct(Product product) {
        Product savedProduct = productRepository.save(product);
        return savedProduct;
    }

    public Product getProductById(Long id) {
        try {
            return productRepository.findById(id).get();
        } catch (Exception e) {
            throw new ProductException.ProductNotFoundException();
        }
    }

    public Product updateProduct(Long id, Product productDetails) {
        try {
            Product product = productRepository.findById(id).get();

            product.setName(productDetails.getName());
            product.setPrice(productDetails.getPrice());

            return productRepository.save(product);
        } catch (Exception e) {
            throw new ProductException.ProductNotFoundException();
        }
    }

    public void deleteProduct(Long productId) {
        try {
            productRepository.deleteById(productId);
        } catch (Exception e) {
            throw new ProductException.ProductNotFoundException();
        }
    }
}
