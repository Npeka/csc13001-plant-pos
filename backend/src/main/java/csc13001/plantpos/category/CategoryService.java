package csc13001.plantpos.category;

import csc13001.plantpos.category.exception.CategoryException;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
@RequiredArgsConstructor
public class CategoryService {
    private final CategoryRepository categoryRepository;

    public List<Category> getAllCategories() {
        return categoryRepository.findAll();
    }

    public Category createCategory(Category category) {
        return categoryRepository.save(category);
    }

    public Category getCategoryById(Long id) {
        return categoryRepository.findById(id)
                .orElseThrow(CategoryException.CategoryNotFoundException::new);
    }

    public void updateCategory(Category category) {
        categoryRepository.findById(category.getCategoryId())
                .map(existingCategory -> {
                    existingCategory.setName(category.getName());
                    existingCategory.setDescription(category.getDescription());
                    return categoryRepository.save(existingCategory);
                })
                .orElseThrow(CategoryException.CategoryNotFoundException::new);
    }

    public void deleteCategory(Long id) {
        if (!categoryRepository.existsById(id)) {
            throw new CategoryException.CategoryNotFoundException();
        }
        categoryRepository.deleteById(id);
    }
}
