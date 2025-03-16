package csc13001.plantpos.adapters.controllers;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.validation.BindingResult;
import org.springframework.web.bind.annotation.*;
import csc13001.plantpos.domain.models.Product;
import csc13001.plantpos.utils.http.HttpResponse;
import csc13001.plantpos.application.dtos.product.ProductDTO;
import csc13001.plantpos.application.services.ProductService;

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

    @PostMapping
    public ResponseEntity<?> createProduct(
            @RequestBody ProductDTO productDTO,
            BindingResult bindingResult) {
        if (bindingResult.hasErrors()) {
            return HttpResponse.badRequest(bindingResult);
        }
        Product createdProduct = productService.createProduct(productDTO);
        return HttpResponse.ok("Create product successful", createdProduct);
    }

    @PutMapping("/{id}")
    public ResponseEntity<?> updateProduct(
            @PathVariable Long id,
            @RequestBody ProductDTO productDTO, BindingResult bindingResult) {
        if (bindingResult.hasErrors()) {
            return HttpResponse.badRequest(bindingResult);
        }
        productService.updateProduct(id, productDTO);
        return HttpResponse.ok("Update product successful");
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<?> deleteProduct(@PathVariable Long id) {
        productService.deleteProduct(id);
        return ResponseEntity.noContent().build();
    }
}
