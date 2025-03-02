package csc13001.plantpos.exception.customer;

public class CustomerException extends RuntimeException {

    public CustomerException() {
        super();
    }

    public CustomerException(String message) {
        super(message);
    }

    public static class CustomerNotFoundException extends CustomerException {
        public CustomerNotFoundException() {
            super(CustomerErrorMessages.CUSTOMER_NOT_FOUND);
        }
    }

    public static class CustomerExistsException extends CustomerException {
        public CustomerExistsException() {
            super(CustomerErrorMessages.CUSTOMER_EXISTS);
        }
    }

}
