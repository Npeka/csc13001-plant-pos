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
    CategoryRepository categoryRepository;

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
        try {
            return categoryRepository.findById(id).get();
        } catch (Exception e) {
            throw new CategoryException.CategoryNotFoundException();
        }
    }

    public void updateCategory(Long id, Category categoryDetails) {
        Category category = categoryRepository.findById(id)
                .orElseThrow(() -> new CategoryException.CategoryNotFoundException());

        category.setName(categoryDetails.getName());
        category.setDescription(categoryDetails.getDescription());
        categoryRepository.save(category);
    }

    public void deleteCategory(Long id) {
        try {
            categoryRepository.deleteById(id);
        } catch (Exception e) {
            throw new CategoryException.CategoryNotFoundException();
        }
    }
}
