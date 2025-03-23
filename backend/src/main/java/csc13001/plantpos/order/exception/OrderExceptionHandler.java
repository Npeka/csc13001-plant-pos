package csc13001.plantpos.order.exception;

import csc13001.plantpos.utils.http.HttpResponse;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.bind.annotation.RestControllerAdvice;

@RestControllerAdvice
public class OrderExceptionHandler {

    @ExceptionHandler(OrderException.OrderExistsException.class)
    public ResponseEntity<?> handleOrderExists(OrderException.OrderExistsException ex) {
        return HttpResponse.badRequest(OrderErrorMessages.ORDER_EXISTS);
    }

    @ExceptionHandler(OrderException.OrderNotFoundException.class)
    public ResponseEntity<?> handleOrderNotFound(OrderException.OrderNotFoundException ex) {
        return HttpResponse.badRequest(OrderErrorMessages.ORDER_NOT_FOUND);
    }
}
