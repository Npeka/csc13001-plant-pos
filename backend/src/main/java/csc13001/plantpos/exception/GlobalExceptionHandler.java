package csc13001.plantpos.exception;

import org.springframework.data.crossstore.ChangeSetPersister;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.bind.annotation.RestControllerAdvice;

import csc13001.plantpos.utils.http.HttpResponse;

@RestControllerAdvice
public class GlobalExceptionHandler {
    @ExceptionHandler(ChangeSetPersister.NotFoundException.class)
    public ResponseEntity<?> handleResourceNotFound(ChangeSetPersister.NotFoundException ex) {
        return HttpResponse.notFound();
    }
}
