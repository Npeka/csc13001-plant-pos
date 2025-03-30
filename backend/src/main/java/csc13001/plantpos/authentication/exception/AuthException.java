package csc13001.plantpos.authentication.exception;

public class AuthException extends RuntimeException {

    public AuthException() {
        super();
    }

    public AuthException(String message) {
        super(message);
    }

    public static class UsernameExistsException extends AuthException {
        public UsernameExistsException() {
            super(AuthErrorMessages.USERNAME_EXISTS);
        }
    }

    public static class InvalidPasswordException extends AuthException {
        public InvalidPasswordException() {
            super(AuthErrorMessages.INVALID_PASSWORD);
        }
    }

    public static class UserNotFoundException extends AuthException {
        public UserNotFoundException() {
            super(AuthErrorMessages.USER_NOT_FOUND);
        }
    }

    public static class InvalidOtpException extends AuthException {
        public InvalidOtpException() {
            super(AuthErrorMessages.INVALID_OTP);
        }
    }

    public static class OtpExpiredException extends AuthException {
        public OtpExpiredException() {
            super(AuthErrorMessages.OTP_EXPIRED);
        }
    }
}
