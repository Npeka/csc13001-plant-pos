package csc13001.plantpos.product;

import csc13001.plantpos.category.Category;
import csc13001.plantpos.category.CategoryRepository;
import csc13001.plantpos.category.exception.CategoryException;
import csc13001.plantpos.domain.enums.MinioBucket;
import csc13001.plantpos.global.MinIOService;
import csc13001.plantpos.product.dtos.ProductDTO;
import csc13001.plantpos.product.exception.ProductException;
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
        Category category = categoryRepository.findById(productDTO.getCategory().getCategoryId())
                .orElseThrow(CategoryException.CategoryNotFoundException::new);

        Product product = new Product();
        product.setName(productDTO.getName());
        product.setDescription(productDTO.getDescription());
        product.setSalePrice(productDTO.getSalePrice());
        product.setCareLevel(productDTO.getCareLevel());
        product.setEnvironmentType(productDTO.getEnvironmentType());
        product.setSize(productDTO.getSize());
        product.setLightRequirement(productDTO.getLightRequirement());
        product.setWateringSchedule(productDTO.getWateringSchedule());
        product.setCategory(category);

        if (image != null) {
            String imageurl = minIOService.uploadFile(image, MinioBucket.PRODUCT);
            product.setImageUrl(imageurl);
        }

        return productRepository.save(product);
    }

    public void updateProduct(Long id, ProductDTO productDTO, MultipartFile image) {
        Product product = productRepository.findById(id)
                .orElseThrow(ProductException.ProductNotFoundException::new);

        if (image != null) {
            if (product.getImageUrl() != null) {
                minIOService.deleteFile(product.getImageUrl());
            }
            String imageUrl = minIOService.uploadFile(image, MinioBucket.PRODUCT);
            product.setImageUrl(imageUrl);
        }

        for (Field dtoField : ProductDTO.class.getDeclaredFields()) {
            dtoField.setAccessible(true);
            try {
                Object newValue = dtoField.get(productDTO);
                if (newValue == null)
                    continue;

                if (dtoField.getName().equals("category")) {
                    Category category = categoryRepository.findById(((Category) newValue).getCategoryId())
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
        Product product = productRepository.findById(productId)
                .orElseThrow(ProductException.ProductNotFoundException::new);
        if (product.getImageUrl() != null) {
            minIOService.deleteFile(product.getImageUrl());
        }
        productRepository.deleteById(productId);
    }
}
