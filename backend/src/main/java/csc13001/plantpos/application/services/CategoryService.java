package csc13001.plantpos.application.services;

import csc13001.plantpos.adapters.repositories.CategoryRepository;
import csc13001.plantpos.domain.models.Category;
import csc13001.plantpos.exception.category.CategoryException;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import java.util.List;

@Service
public class CategoryService {

    @Autowired
    private CategoryRepository categoryRepository;

    public List<Category> getAllCategories() {
        return categoryRepository.findAll();
    }

    public Category createCategory(Category category) {
        if (categoryRepository.findByName(category.getName()).isPresent()) {
            throw new CategoryException.CategoryExistsException();
        }

        return categoryRepository.save(category);
    }

    public Category getCategoryById(Long id) {
        return categoryRepository.findById(id)
                .orElseThrow(CategoryException.CategoryNotFoundException::new);
    }

    public Category updateCategory(Long id, Category categoryDetails) {
        return categoryRepository.findById(id)
                .map(existingCategory -> {
                    existingCategory.setName(categoryDetails.getName());
                    existingCategory.setDescription(categoryDetails.getDescription());
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
