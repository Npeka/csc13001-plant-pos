package csc13001.plantpos.exception.inventory;

public class InventoryException extends RuntimeException {

    public InventoryException() {
        super();
    }

    public InventoryException(String message) {
        super(message);
    }

    public static class InventoryNotFoundException extends InventoryException {
        public InventoryNotFoundException() {
            super(InventoryErrorMessages.INVENTORY_NOT_FOUND);
        }
    }

    public static class InventoryExistsException extends InventoryException {
        public InventoryExistsException() {
            super(InventoryErrorMessages.INVENTORY_EXISTS);
        }
    }

    public static class InventoryNotEnoughStockException extends InventoryException {
        public InventoryNotEnoughStockException() {
            super(InventoryErrorMessages.INVENTORY_NOT_ENOUGH_STOCK);
        }
    }
}
