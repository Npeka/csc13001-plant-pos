package csc13001.plantpos.exception.auth;

import csc13001.plantpos.utils.http.HttpResponse;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.bind.annotation.RestControllerAdvice;

@RestControllerAdvice
public class AuthExceptionHandler {

    @ExceptionHandler(AuthException.UsernameExistsException.class)
    public ResponseEntity<?> handleUsernameExists(AuthException.UsernameExistsException ex) {
        return HttpResponse.badRequest(AuthErrorMessages.USERNAME_EXISTS);
    }

    @ExceptionHandler(AuthException.UserNotFoundException.class)
    public ResponseEntity<?> handleUserNotFound(AuthException.UserNotFoundException ex) {
        return HttpResponse.badRequest(AuthErrorMessages.USER_NOT_FOUND);
    }

    @ExceptionHandler(AuthException.InvalidPasswordException.class)
    public ResponseEntity<?> handleInvalidPassword(AuthException.InvalidPasswordException ex) {
        return HttpResponse.badRequest(AuthErrorMessages.INVALID_PASSWORD);
    }
}
