package csc13001.plantpos.adapters.controllers;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.validation.BindingResult;
import org.springframework.validation.annotation.Validated;
import org.springframework.web.bind.annotation.*;
import csc13001.plantpos.domain.models.Category;
import csc13001.plantpos.utils.http.HttpResponse;
import csc13001.plantpos.application.services.CategoryService;

import java.util.List;

@RestController
@RequestMapping("/api/categories")
public class CategoryController {

    @Autowired
    private CategoryService categoryService;

    @GetMapping
    public List<Category> getAllCategories() {
        return categoryService.getAllCategories();
    }

    @PostMapping
    public ResponseEntity<?> createCategory(
            @RequestBody @Validated Category category,
            BindingResult bindingResult) {
        if (bindingResult.hasErrors()) {
            return HttpResponse.badRequest(bindingResult);
        }
        Category createdCategory = categoryService.createCategory(category);
        return HttpResponse.ok("Create category successful", createdCategory);
    }

    @GetMapping("/{id}")
    public ResponseEntity<?> getCategoryById(@PathVariable Long id) {
        Category category = categoryService.getCategoryById(id);
        if (category == null) {
            return HttpResponse.notFound("Category not found");
        }
        return HttpResponse.ok("Get category successful", category);
    }

    @PutMapping("/{id}")
    public ResponseEntity<?> updateCategory(
            @PathVariable Long id,
            @RequestBody @Validated Category categoryDetails,
            BindingResult bindingResult) {
        if (bindingResult.hasErrors()) {
            return HttpResponse.badRequest(bindingResult);
        }
        Category category = categoryService.updateCategory(id, categoryDetails);
        if (category == null) {
            return HttpResponse.notFound("Category not found");
        }
        return HttpResponse.ok("Update category successful");
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<?> deleteCategory(@PathVariable Long id) {
        categoryService.deleteCategory(id);
        return HttpResponse.ok("Delete category successful");
    }
}
