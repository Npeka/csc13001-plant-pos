package csc13001.plantpos.adapters.controllers;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.validation.BindingResult;
import org.springframework.web.bind.annotation.*;
import csc13001.plantpos.domain.models.Product;
import csc13001.plantpos.utils.http.HttpResponse;
import csc13001.plantpos.application.services.ProductService;

import java.util.List;

@RestController
@RequestMapping("/api/products")
public class ProductController {

    @Autowired
    private ProductService productService;

    @GetMapping
    public List<Product> getAllProducts() {
        return productService.getAllProducts();
    }

    @PostMapping
    public ResponseEntity<?> createProduct(
            @RequestBody Product product,
            BindingResult bindingResult) {
        if (bindingResult.hasErrors()) {
            return HttpResponse.invalidInputData();
        }
        Product createdProduct = productService.createProduct(product);
        return HttpResponse.ok("Create product successful", createdProduct);
    }

    @GetMapping("/{id}")
    public ResponseEntity<?> getProductById(@PathVariable Long id) {
        Product product = productService.getProductById(id);
        return HttpResponse.ok("Get product successful", product);

    }

    @PutMapping("/{id}")
    public ResponseEntity<?> updateProduct(
            @PathVariable Long id,
            @RequestBody Product productDetails) {
        productService.updateProduct(id, productDetails);
        return HttpResponse.ok("Update product successful");
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<?> deleteProduct(@PathVariable Long id) {
        productService.deleteProduct(id);
        return ResponseEntity.noContent().build();
    }
}
