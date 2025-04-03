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

    public static class EmailExistsException extends AuthException {
        public EmailExistsException() {
            super(AuthErrorMessages.EMAIL_EXISTS);
        }
    }

    public static class PhoneExistsException extends AuthException {
        public PhoneExistsException() {
            super(AuthErrorMessages.PHONE_EXISTS);
        }
    }

    public static class UsernameEmailPhoneExistsException extends AuthException {
        public UsernameEmailPhoneExistsException() {
            super(AuthErrorMessages.USERNAME_EMAIL_PHONE_EXISTS);
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

    public static class InvalidTokenException extends AuthException {
        public InvalidTokenException() {
            super(AuthErrorMessages.INVALID_TOKEN);
        }
    }
}
