package csc13001.plantpos.global;

import csc13001.plantpos.utils.http.HttpResponse;
import jakarta.validation.ConstraintViolation;
import jakarta.validation.ConstraintViolationException;

import java.util.List;
import java.util.stream.Collectors;

import org.springframework.data.crossstore.ChangeSetPersister;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.bind.annotation.RestControllerAdvice;

@RestControllerAdvice
public class GlobalExceptionHandler {

    @ExceptionHandler(ChangeSetPersister.NotFoundException.class)
    public ResponseEntity<?> handleResourceNotFound(ChangeSetPersister.NotFoundException ex) {
        return HttpResponse.notFound();
    }

    @ExceptionHandler(IllegalArgumentException.class)
    public ResponseEntity<?> handleIllegalArgumentException(IllegalArgumentException ex) {
        return HttpResponse.badRequest(ex.getMessage());
    }

    @ExceptionHandler(Exception.class)
    public ResponseEntity<?> handleGeneralException(Exception ex) {
        return HttpResponse.invalidInputData(ex.getMessage());
    }

    @ExceptionHandler(RuntimeException.class)
    public ResponseEntity<?> handleRuntimeException(RuntimeException ex) {
        return HttpResponse.invalidInputData(ex.getMessage());
    }

    @ExceptionHandler(ConstraintViolationException.class)
    public ResponseEntity<?> handleConstraintViolation(ConstraintViolationException ex) {
        List<String> templates = ex.getConstraintViolations().stream()
                .map(ConstraintViolation::getMessageTemplate)
                .collect(Collectors.toList());

        return HttpResponse.badRequest(templates.get(0));
    }
}
