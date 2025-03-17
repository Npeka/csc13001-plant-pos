package csc13001.plantpos.application.services;

import csc13001.plantpos.adapters.repositories.CategoryRepository;
import csc13001.plantpos.adapters.repositories.ProductRepository;
import csc13001.plantpos.domain.models.Category;
import csc13001.plantpos.domain.models.Product;
import csc13001.plantpos.exception.product.ProductException;
import csc13001.plantpos.exception.category.CategoryException;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.HashSet;
import java.util.List;
import java.util.Set;

@Service
public class ProductService {
    @Autowired
    private ProductRepository productRepository;

    @Autowired
    private CategoryRepository categoryRepository;

    public List<Product> getAllProducts() {
        return productRepository.findAll();
    }

    public Product createProduct(Product product) {
        Set<Category> existingCategories = new HashSet<>();
        for (Category category : product.getCategories()) {
            Category existingCategory = categoryRepository.findById(category.getCategoryId())
                    .orElseThrow(CategoryException.CategoryNotFoundException::new);
            existingCategories.add(existingCategory);
        }
        product.setCategories(existingCategories);
        return productRepository.save(product);
    }

    public Product getProductById(Long id) {
        try {
            return productRepository.findById(id).get();
        } catch (Exception e) {
            throw new ProductException.ProductNotFoundException();
        }
    }

    public void updateProduct(Long id, Product productDetails) {
        try {
            Product product = productRepository.findById(id).get();

            product.setName(productDetails.getName());
            product.setPrice(productDetails.getPrice());

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
