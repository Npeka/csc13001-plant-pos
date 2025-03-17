package csc13001.plantpos.utils.http;

import org.springframework.http.ResponseEntity;
import org.springframework.validation.BindingResult;

import com.fasterxml.jackson.annotation.JsonAutoDetect;
import com.fasterxml.jackson.annotation.JsonInclude;

import io.swagger.v3.oas.annotations.media.Schema;
import lombok.AllArgsConstructor;

@JsonAutoDetect(fieldVisibility = JsonAutoDetect.Visibility.ANY)
@JsonInclude(JsonInclude.Include.NON_NULL)
@AllArgsConstructor
public class HttpResponse<T> {
    @Schema(description = "Status of the response", example = "success")
    private final Status status;

    @Schema(description = "Message of the response", example = "Get all products successful")
    private final String message;

    @Schema(description = "Data of the response")
    private final T data;

    public static <T> HttpResponse<T> success(String message, T data) {
        return new HttpResponse<>(Status.success, message, data);
    }

    public static <T> HttpResponse<T> error(String message) {
        return new HttpResponse<>(Status.error, message, null);
    }

    // 200 - OK
    public static <T> ResponseEntity<?> ok(String message, T data) {
        return ResponseEntity.ok(success(message, data));
    }

    public static <T> ResponseEntity<?> ok(String message) {
        return ResponseEntity.ok(success(message, null));
    }

    // 400 - Bad Request
    public static <T> ResponseEntity<?> badRequest(String message) {
        return ResponseEntity.badRequest().body(error(message));
    }

    // 400 - Custom Bad Request
    public static <T> ResponseEntity<?> invalidInputData() {
        return ResponseEntity.badRequest().body(error("Invalid input data"));
    }

    public static <T> ResponseEntity<?> invalidInputData(String message) {
        return ResponseEntity.badRequest().body(error(message));
    }

    public static <T> ResponseEntity<?> badRequest(BindingResult bindingResult) {
        return ResponseEntity.badRequest().body(error(bindingResult.getFieldErrors().get(0).getDefaultMessage()));
    }

    // 404 - Resource not found
    public static <T> ResponseEntity<?> notFound(String message) {
        return ResponseEntity.status(404).body(error(message));
    }

    public static <T> ResponseEntity<?> notFound() {
        return ResponseEntity.status(404).body(error("Resource not found"));
    }

    public enum Status {
        success,
        error
    }
}
