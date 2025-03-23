package csc13001.plantpos.product.exception;

import csc13001.plantpos.utils.http.HttpResponse;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.bind.annotation.RestControllerAdvice;

@RestControllerAdvice
public class ProductExceptionHandler {

    @ExceptionHandler(ProductException.ProductExistsException.class)
    public ResponseEntity<?> handleProductExists(ProductException.ProductExistsException ex) {
        return HttpResponse.badRequest(ProductErrorMessages.PRODUCT_EXISTS);
    }

    @ExceptionHandler(ProductException.ProductNotFoundException.class)
    public ResponseEntity<?> handleProductNotFound(ProductException.ProductNotFoundException ex) {
        return HttpResponse.badRequest(ProductErrorMessages.PRODUCT_NOT_FOUND);
    }

    @ExceptionHandler(ProductException.ProductWrongTypeImageException.class)
    public ResponseEntity<?> handleProductImage(ProductException.ProductWrongTypeImageException ex) {
        return HttpResponse.badRequest(ProductErrorMessages.PRODUCT_WRONG_TYPE_IMAGE);
    }
}
