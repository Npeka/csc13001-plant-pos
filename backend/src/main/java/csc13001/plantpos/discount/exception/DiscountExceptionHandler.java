package csc13001.plantpos.discount.exception;

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

    @ExceptionHandler(DiscountException.DiscountInvalidDateException.class)
    public ResponseEntity<?> handleDiscountInvalidDate(DiscountException.DiscountInvalidDateException ex) {
        return HttpResponse.badRequest(DiscountErrorMessages.INVALID_DATE);
    }

    @ExceptionHandler(DiscountException.DiscountNotApplicableException.class)
    public ResponseEntity<?> handleDiscountProgramNotApplicable(
            DiscountException.DiscountNotApplicableException ex) {
        return HttpResponse.badRequest(DiscountErrorMessages.DISCOUNT_NOT_APPLICABLE);
    }

    @ExceptionHandler(DiscountException.DiscountHasExpiredException.class)
    public ResponseEntity<?> handleDiscountHasExpired(DiscountException.DiscountHasExpiredException ex) {
        return HttpResponse.badRequest(DiscountErrorMessages.DISCOUNT_HAS_EXPIRED);
    }

    @ExceptionHandler(DiscountException.DiscountNotActiveException.class)
    public ResponseEntity<?> handleDiscountNotActive(DiscountException.DiscountNotActiveException ex) {
        return HttpResponse.badRequest(DiscountErrorMessages.DISCOUNT_NOT_ACTIVE);
    }

    @ExceptionHandler(DiscountException.DiscountRateInvalidException.class)
    public ResponseEntity<?> handleDiscountRateInvalid(DiscountException.DiscountRateInvalidException ex) {
        return HttpResponse.badRequest(DiscountErrorMessages.DISCOUNT_RATE_INVALID);
    }

    @ExceptionHandler(DiscountException.DiscountAlreadyAppliedException.class)
    public ResponseEntity<?> handleDiscountAlreadyApplied(DiscountException.DiscountAlreadyAppliedException ex) {
        return HttpResponse.badRequest(DiscountErrorMessages.DISCOUNT_ALREADY_APPLIED);
    }
}
