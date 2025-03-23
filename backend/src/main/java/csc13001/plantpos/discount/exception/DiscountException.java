package csc13001.plantpos.discount.exception;

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

    public static class DiscountInvalidDateException extends DiscountException {
        public DiscountInvalidDateException() {
            super(DiscountErrorMessages.INVALID_DATE);
        }
    }

    public static class DiscountNotApplicableException extends DiscountException {
        public DiscountNotApplicableException() {
            super(DiscountErrorMessages.DISCOUNT_NOT_APPLICABLE);
        }
    }
}
