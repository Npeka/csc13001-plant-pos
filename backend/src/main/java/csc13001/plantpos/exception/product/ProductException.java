package csc13001.plantpos.exception.product;

public class ProductException extends RuntimeException {

    public ProductException() {
        super();
    }

    public ProductException(String message) {
        super(message);
    }

    public static class ProductNotFoundException extends ProductException {
        public ProductNotFoundException() {
            super(ProductErrorMessages.PRODUCT_NOT_FOUND);
        }
    }

    public static class ProductExistsException extends ProductException {
        public ProductExistsException() {
            super(ProductErrorMessages.PRODUCT_EXISTS);
        }
    }

}
