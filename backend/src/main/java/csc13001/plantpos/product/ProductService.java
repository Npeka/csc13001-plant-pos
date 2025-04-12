package csc13001.plantpos.product;

import csc13001.plantpos.category.CategoryRepository;
import csc13001.plantpos.category.exception.CategoryException;
import csc13001.plantpos.global.MinIOService;
import csc13001.plantpos.global.enums.MinioBucket;
import csc13001.plantpos.product.exception.ProductException;
import lombok.RequiredArgsConstructor;

import org.springframework.context.ApplicationEventPublisher;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.web.multipart.MultipartFile;

import java.util.List;

@Service
@RequiredArgsConstructor
public class ProductService {
    private final ApplicationEventPublisher eventPublisher;
    private final ProductRepository productRepository;
    private final CategoryRepository categoryRepository;
    private final MinIOService minIOService;

    public List<Product> getAllProducts() {
        return productRepository.findAll();
    }

    public Product getProductById(Long id) {
        return productRepository.findById(id)
                .orElseThrow(ProductException.ProductNotFoundException::new);
    }

    @Transactional
    public Product createProduct(Product product, MultipartFile image) {
        if (!categoryRepository.existsById(product.getCategory().getCategoryId())) {
            throw new CategoryException.CategoryNotFoundException();
        }

        if (image != null) {
            String imageurl = minIOService.uploadFile(image, MinioBucket.PRODUCT);
            product.setImageUrl(imageurl);
        }

        Product savedProduct = productRepository.save(product);
        eventPublisher.publishEvent(savedProduct);
        return savedProduct;
    }

    @Transactional
    public void updateProduct(Product product, MultipartFile image) {
        if (!productRepository.existsById(product.getProductId())) {
            throw new ProductException.ProductNotFoundException();
        }

        if (!categoryRepository.existsById(product.getCategory().getCategoryId())) {
            throw new CategoryException.CategoryNotFoundException();
        }

        if (image != null) {
            if (product.getImageUrl() != null) {
                minIOService.deleteFile(product.getImageUrl());
            }
            String imageUrl = minIOService.uploadFile(image, MinioBucket.PRODUCT);
            product.setImageUrl(imageUrl);
        }

        productRepository.save(product);
    }

    @Transactional
    public void deleteProduct(Long productId) {
        Product product = productRepository.findById(productId)
                .orElseThrow(ProductException.ProductNotFoundException::new);

        if (product.getImageUrl() != null) {
            minIOService.deleteFile(product.getImageUrl());
        }

        productRepository.deleteById(productId);
    }
}
