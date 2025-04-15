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
        return HttpResponse.ok("Lấy danh sách danh mục thành công", categories);
    }

    @PostMapping
    public ResponseEntity<?> createCategory(
            @RequestBody @Validated Category category,
            BindingResult bindingResult) {
        if (bindingResult.hasErrors()) {
            return HttpResponse.badRequest(bindingResult);
        }
        Category createdCategory = categoryService.createCategory(category);
        return HttpResponse.ok("Tạo danh mục thành công", createdCategory);
    }

    @GetMapping("/{id}")
    public ResponseEntity<?> getCategoryById(@PathVariable Long id) {
        Category category = categoryService.getCategoryById(id);
        if (category == null) {
            return HttpResponse.notFound("Không tìm thấy danh mục");
        }
        return HttpResponse.ok("Lấy thông tin danh mục thành công", category);
    }

    @PutMapping("/{id}")
    public ResponseEntity<?> updateCategory(
            @PathVariable Long id,
            @RequestBody @Validated Category category,
            BindingResult bindingResult) {
        if (bindingResult.hasErrors()) {
            return HttpResponse.badRequest(bindingResult);
        }
        category.setCategoryId(id);
        categoryService.updateCategory(category);
        return HttpResponse.ok("Cập nhật danh mục thành công");
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<?> deleteCategory(@PathVariable Long id) {
        categoryService.deleteCategory(id);
        return HttpResponse.ok("Xóa danh mục thành công");
    }
}
