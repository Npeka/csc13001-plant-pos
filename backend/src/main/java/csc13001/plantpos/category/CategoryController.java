package csc13001.plantpos.category;

import csc13001.plantpos.utils.http.HttpResponse;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.validation.BindingResult;
import org.springframework.validation.annotation.Validated;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/api/categories")
public class CategoryController {

    @Autowired
    private CategoryService categoryService;

    @GetMapping
    public ResponseEntity<?> getAllCategories() {
        List<Category> categories = categoryService.getAllCategories();
        return HttpResponse.ok("Get all categories successful", categories);
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
            @PathVariable Long categoryId,
            @RequestBody @Validated Category category,
            BindingResult bindingResult) {
        if (bindingResult.hasErrors()) {
            return HttpResponse.badRequest(bindingResult);
        }
        category.setCategoryId(categoryId);
        categoryService.updateCategory(category);
        return HttpResponse.ok("Update category successful");
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<?> deleteCategory(@PathVariable Long id) {
        categoryService.deleteCategory(id);
        return HttpResponse.ok("Delete category successful");
    }
}
