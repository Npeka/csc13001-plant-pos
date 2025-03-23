package csc13001.plantpos.inventory.exception;

import csc13001.plantpos.utils.http.HttpResponse;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.bind.annotation.RestControllerAdvice;

@RestControllerAdvice
public class InventoryExceptionHandler {

    @ExceptionHandler(InventoryException.InventoryExistsException.class)
    public ResponseEntity<?> handleInventoryExists(InventoryException.InventoryExistsException ex) {
        return HttpResponse.badRequest(InventoryErrorMessages.INVENTORY_EXISTS);
    }

    @ExceptionHandler(InventoryException.InventoryNotFoundException.class)
    public ResponseEntity<?> handleInventoryNotFound(InventoryException.InventoryNotFoundException ex) {
        return HttpResponse.badRequest(InventoryErrorMessages.INVENTORY_NOT_FOUND);
    }

    @ExceptionHandler(InventoryException.InventoryNotEnoughStockException.class)
    public ResponseEntity<?> handleInventoryNotEnoughStock(InventoryException.InventoryNotEnoughStockException ex) {
        return HttpResponse.badRequest(InventoryErrorMessages.INVENTORY_NOT_ENOUGH_STOCK);
    }
}
