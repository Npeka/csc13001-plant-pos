package csc13001.plantpos.product;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.JsonMappingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import csc13001.plantpos.product.dtos.ProductDTO;
import csc13001.plantpos.product.exception.ProductException;
import csc13001.plantpos.utils.http.HttpResponse;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.validation.BindingResult;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.multipart.MultipartFile;

import java.util.List;

@RestController
@RequestMapping("/api/products")
public class ProductController {

    @Autowired
    private ProductService productService;

    @GetMapping
    public ResponseEntity<?> getAllProducts() {
        List<ProductDTO> products = productService.getAllProducts();
        return HttpResponse.ok("Get all products successful", products);
    }

    @GetMapping("/{id}")
    public ResponseEntity<?> getProductById(@PathVariable Long id) {
        Product product = productService.getProductById(id);
        return HttpResponse.ok("Get product successful", product);
    }

    @PostMapping(consumes = { MediaType.MULTIPART_FORM_DATA_VALUE })
    public ResponseEntity<?> createProduct(
            @RequestPart("product") String productJson,
            @RequestPart(value = "image", required = false) MultipartFile image,
            BindingResult bindingResult) throws JsonMappingException, JsonProcessingException {
        if (bindingResult.hasErrors()) {
            return HttpResponse.badRequest(bindingResult);
        }

        ObjectMapper objectMapper = new ObjectMapper();
        ProductDTO productDTO = objectMapper.readValue(productJson, ProductDTO.class);
        validateImage(image);

        Product createdProduct = productService.createProduct(productDTO, image);
        return HttpResponse.ok("Create product successful", createdProduct);
    }

    @PutMapping(path = "/{id}", consumes = { MediaType.MULTIPART_FORM_DATA_VALUE })
    public ResponseEntity<?> updateProduct(
            @PathVariable Long id,
            @RequestPart("product") String productJson,
            @RequestPart(value = "image", required = false) MultipartFile image,
            BindingResult bindingResult) throws JsonMappingException, JsonProcessingException {
        if (bindingResult.hasErrors()) {
            return HttpResponse.badRequest(bindingResult);
        }

        ObjectMapper objectMapper = new ObjectMapper();
        ProductDTO productDTO = objectMapper.readValue(productJson, ProductDTO.class);
        validateImage(image);

        productService.updateProduct(id, productDTO, image);
        return HttpResponse.ok("Update product successful");
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<?> deleteProduct(@PathVariable Long id) {
        productService.deleteProduct(id);
        return HttpResponse.ok("Delete product successful");
    }

    private void validateImage(MultipartFile image) {
        if (image == null) {
            return;
        }

        String contentType = image.getContentType();
        if (contentType == null || !(contentType.startsWith("image/"))) {
            throw new ProductException.ProductWrongTypeImageException();
        }
    }
}
