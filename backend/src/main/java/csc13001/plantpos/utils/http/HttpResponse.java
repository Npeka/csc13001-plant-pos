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
    @Schema(description = "Trạng thái của phản hồi", example = "thành công")
    private final Status status;

    @Schema(description = "Thông báo của phản hồi", example = "Lấy danh sách sản phẩm thành công")
    private final String message;

    @Schema(description = "Dữ liệu của phản hồi")
    private final T data;

    public static <T> HttpResponse<T> success(String message, T data) {
        return new HttpResponse<>(Status.thành_công, message, data);
    }

    public static <T> HttpResponse<T> error(String message) {
        return new HttpResponse<>(Status.lỗi, message, null);
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
        return ResponseEntity.badRequest().body(error("Dữ liệu đầu vào không hợp lệ"));
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
        return ResponseEntity.status(404).body(error("Không tìm thấy tài nguyên"));
    }

    public enum Status {
        thành_công,
        lỗi
    }
}
