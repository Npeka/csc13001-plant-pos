package csc13001.plantpos.exception.discount;

public class DiscountException extends RuntimeException {

    public DiscountException() {
        super();
    }

    public DiscountException(String message) {
        super(message);
    }

    public static class DiscountNotFoundException extends DiscountException {
        public DiscountNotFoundException() {
            super(DiscountErrorMessages.DISCOUNT_NOT_FOUND);
        }
    }

    public static class DiscountExistsException extends DiscountException {
        public DiscountExistsException() {
            super(DiscountErrorMessages.DISCOUNT_EXISTS);
        }
    }

}
