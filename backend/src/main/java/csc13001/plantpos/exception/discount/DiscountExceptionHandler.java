package csc13001.plantpos.exception.discount;

import csc13001.plantpos.utils.http.HttpResponse;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.bind.annotation.RestControllerAdvice;

@RestControllerAdvice
public class DiscountExceptionHandler {

    @ExceptionHandler(DiscountException.DiscountExistsException.class)
    public ResponseEntity<?> handleDiscountExists(DiscountException.DiscountExistsException ex) {
        return HttpResponse.badRequest(DiscountErrorMessages.DISCOUNT_EXISTS);
    }

    @ExceptionHandler(DiscountException.DiscountNotFoundException.class)
    public ResponseEntity<?> handleDiscountNotFound(DiscountException.DiscountNotFoundException ex) {
        return HttpResponse.badRequest(DiscountErrorMessages.DISCOUNT_NOT_FOUND);
    }
}
