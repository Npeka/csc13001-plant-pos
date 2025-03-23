package csc13001.plantpos.staff.exception;

public class StaffException extends RuntimeException {

    public StaffException() {
        super();
    }

    public StaffException(String message) {
        super(message);
    }

    public static class StaffNotFoundException extends StaffException {
        public StaffNotFoundException() {
            super(StaffErrorMessages.STAFF_NOT_FOUND);
        }
    }

    public static class StaffExistsException extends StaffException {
        public StaffExistsException() {
            super(StaffErrorMessages.STAFF_EXISTS);
        }
    }
}
