package csc13001.plantpos.application.services;

import csc13001.plantpos.adapters.repositories.CategoryRepository;
import csc13001.plantpos.adapters.repositories.ProductRepository;
import csc13001.plantpos.application.dtos.product.ProductDTO;
import csc13001.plantpos.domain.enums.MinioBucket;
import csc13001.plantpos.domain.models.Category;
import csc13001.plantpos.domain.models.Product;
import csc13001.plantpos.exception.category.CategoryException;
import csc13001.plantpos.exception.product.ProductException;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.web.multipart.MultipartFile;

import java.lang.reflect.Field;
import java.util.List;
import java.util.stream.Collectors;

@Service
@RequiredArgsConstructor
public class ProductService {
    private final ProductRepository productRepository;
    private final CategoryRepository categoryRepository;
    private final MinIOService minIOService;

    public List<ProductDTO> getAllProducts() {
        return productRepository.findAll().stream()
                .map(ProductDTO::new)
                .collect(Collectors.toList());
    }

    public Product getProductById(Long id) {
        return productRepository.findById(id)
                .orElseThrow(ProductException.ProductNotFoundException::new);
    }

    @Transactional
    public Product createProduct(ProductDTO productDTO, MultipartFile image) {
        Category category = categoryRepository.findByName(productDTO.getCategoryName())
                .orElseThrow(CategoryException.CategoryNotFoundException::new);

        Product product = new Product();
        product.setName(productDTO.getName());
        product.setDescription(productDTO.getDescription());
        product.setPrice(productDTO.getPrice());
        product.setCareLevel(productDTO.getCareLevel());
        product.setEnvironmentType(productDTO.getEnvironmentType());
        product.setSize(productDTO.getSize());
        product.setLightRequirement(productDTO.getLightRequirement());
        product.setWateringSchedule(productDTO.getWateringSchedule());
        product.setCategory(category);

        String imageurl = minIOService.uploadFile(image, MinioBucket.PRODUCT);
        product.setImageUrl(imageurl);

        return productRepository.save(product);
    }

    public void updateProduct(Long id, ProductDTO productDTO) {
        Product product = productRepository.findById(id)
                .orElseThrow(ProductException.ProductNotFoundException::new);

        for (Field dtoField : ProductDTO.class.getDeclaredFields()) {
            dtoField.setAccessible(true);
            try {
                Object newValue = dtoField.get(productDTO);
                if (newValue == null)
                    continue;

                if (dtoField.getName().equals("categoryName")) {
                    Category category = categoryRepository.findByName((String) newValue)
                            .orElseThrow(CategoryException.CategoryNotFoundException::new);
                    product.setCategory(category);
                } else {
                    Field productField = Product.class.getDeclaredField(dtoField.getName());
                    productField.setAccessible(true);
                    productField.set(product, newValue);
                }
            } catch (NoSuchFieldException | IllegalAccessException e) {
                throw new CategoryException.CategoryUpdateFailedException();
            }
        }

        productRepository.save(product);
    }

    public void deleteProduct(Long productId) {
        if (!productRepository.existsById(productId)) {
            throw new ProductException.ProductNotFoundException();
        }
        productRepository.deleteById(productId);
    }
}
