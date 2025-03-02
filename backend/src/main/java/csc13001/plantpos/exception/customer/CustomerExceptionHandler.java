package csc13001.plantpos.exception.customer;

import csc13001.plantpos.utils.http.HttpResponse;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.bind.annotation.RestControllerAdvice;

@RestControllerAdvice
public class CustomerExceptionHandler {

    @ExceptionHandler(CustomerException.CustomerExistsException.class)
    public ResponseEntity<?> handleCustomerExists(CustomerException.CustomerExistsException ex) {
        return HttpResponse.badRequest(CustomerErrorMessages.CUSTOMER_EXISTS);
    }

    @ExceptionHandler(CustomerException.CustomerNotFoundException.class)
    public ResponseEntity<?> handleCustomerNotFound(CustomerException.CustomerNotFoundException ex) {
        return HttpResponse.badRequest(CustomerErrorMessages.CUSTOMER_NOT_FOUND);
    }
}
