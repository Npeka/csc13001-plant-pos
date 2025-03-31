package csc13001.plantpos.authentication.dtos;

import csc13001.plantpos.utils.http.JsonModel;
import io.swagger.v3.oas.annotations.media.Schema;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.Size;
import lombok.Data;
import lombok.EqualsAndHashCode;
import lombok.NoArgsConstructor;

@Data
@NoArgsConstructor
@EqualsAndHashCode(callSuper = true)
public class LoginDTO extends JsonModel {
    @Schema(example = "admin")
    @NotBlank(message = "Tên đăng nhập là bắt buộc")
    @Size(min = 5, max = 50, message = "Tên đăng nhập hoặc mật khẩu không hợp lệ")
    private String username;

    @Schema(example = "admin")
    @NotBlank(message = "Mật khẩu là bắt buộc")
    @Size(min = 5, max = 50, message = "Tên đăng nhập hoặc mật khẩu không hợp lệ")
    private String password;
}
