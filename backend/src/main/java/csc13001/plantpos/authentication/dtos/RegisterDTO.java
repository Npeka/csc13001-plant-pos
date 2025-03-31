package csc13001.plantpos.authentication.dtos;

import csc13001.plantpos.utils.http.JsonModel;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.Size;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.EqualsAndHashCode;
import lombok.NoArgsConstructor;

@Data
@NoArgsConstructor
@AllArgsConstructor
@EqualsAndHashCode(callSuper = true)
public class RegisterDTO extends JsonModel {
    @NotBlank(message = "Họ tên là bắt buộc")
    @Size(min = 5, max = 256, message = "Họ tên phải từ 5 đến 256 ký tự")
    private String fullname;

    @NotBlank(message = "Tên đăng nhập là bắt buộc")
    @Size(min = 5, max = 50, message = "Tên đăng nhập phải từ 5 đến 50 ký tự")
    private String username;

    @NotBlank(message = "Mật khẩu là bắt buộc")
    @Size(min = 5, max = 50, message = "Mật khẩu phải từ 5 đến 50 ký tự")
    private String password;
}
