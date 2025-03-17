package csc13001.plantpos.exception.category;

public class CategoryException extends RuntimeException {

    public CategoryException() {
        super();
    }

    public CategoryException(String message) {
        super(message);
    }

    public static class CategoryNotFoundException extends CategoryException {
        public CategoryNotFoundException() {
            super(CategoryErrorMessages.CATEGORY_NOT_FOUND);
        }
    }

    public static class CategoryExistsException extends CategoryException {
        public CategoryExistsException() {
            super(CategoryErrorMessages.CATEGORY_EXISTS);
        }
    }

}
