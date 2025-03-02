package csc13001.plantpos.exception.staff;

import csc13001.plantpos.utils.http.HttpResponse;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.bind.annotation.RestControllerAdvice;

@RestControllerAdvice
public class StaffExceptionHandler {

    @ExceptionHandler(StaffException.StaffExistsException.class)
    public ResponseEntity<?> handleStaffExists(StaffException.StaffExistsException ex) {
        return HttpResponse.badRequest(StaffErrorMessages.STAFF_EXISTS);
    }

    @ExceptionHandler(StaffException.StaffNotFoundException.class)
    public ResponseEntity<?> handleStaffNotFound(StaffException.StaffNotFoundException ex) {
        return HttpResponse.badRequest(StaffErrorMessages.STAFF_NOT_FOUND);
    }
}
