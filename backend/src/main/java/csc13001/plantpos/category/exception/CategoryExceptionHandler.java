package csc13001.plantpos.category.exception;

import csc13001.plantpos.utils.http.HttpResponse;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.bind.annotation.RestControllerAdvice;

@RestControllerAdvice
public class CategoryExceptionHandler {

    @ExceptionHandler(CategoryException.CategoryExistsException.class)
    public ResponseEntity<?> handleCustomerExists(CategoryException.CategoryExistsException ex) {
        return HttpResponse.badRequest(CategoryErrorMessages.CATEGORY_EXISTS);
    }

    @ExceptionHandler(CategoryException.CategoryNotFoundException.class)
    public ResponseEntity<?> handleCustomerNotFound(CategoryException.CategoryNotFoundException ex) {
        return HttpResponse.badRequest(CategoryErrorMessages.CATEGORY_NOT_FOUND);
    }

    @ExceptionHandler(CategoryException.CategoryUpdateFailedException.class)
    public ResponseEntity<?> handleCategoryUpdateFailed(CategoryException.CategoryUpdateFailedException ex) {
        return HttpResponse.badRequest(CategoryErrorMessages.CATEGORY_UPDATE_FAILED);
    }
}
