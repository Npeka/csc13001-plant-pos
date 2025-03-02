package csc13001.plantpos.utils.http;

import org.springframework.http.ResponseEntity;

import com.fasterxml.jackson.annotation.JsonAutoDetect;
import com.fasterxml.jackson.annotation.JsonInclude;
import lombok.AllArgsConstructor;

@JsonAutoDetect(fieldVisibility = JsonAutoDetect.Visibility.ANY)
@JsonInclude(JsonInclude.Include.NON_NULL)
@AllArgsConstructor
public class HttpResponse<T> {
    private final Status status;
    private final String message;
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

    // 404 - Resource not found
    public static <T> ResponseEntity<?> notFound() {
        return ResponseEntity.notFound().build();
    }

    public enum Status {
        success,
        error
    }
}
