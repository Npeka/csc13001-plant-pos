package csc13001.plantpos.exception.order;

public class OrderException extends RuntimeException {

    public OrderException() {
        super();
    }

    public OrderException(String message) {
        super(message);
    }

    public static class OrderNotFoundException extends OrderException {
        public OrderNotFoundException() {
            super(OrderErrorMessages.ORDER_NOT_FOUND);
        }
    }

    public static class OrderExistsException extends OrderException {
        public OrderExistsException() {
            super(OrderErrorMessages.ORDER_EXISTS);
        }
    }

}
