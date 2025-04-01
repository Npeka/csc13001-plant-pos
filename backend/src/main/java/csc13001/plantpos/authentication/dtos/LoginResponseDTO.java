package csc13001.plantpos.authentication.dtos;

import csc13001.plantpos.user.User;
import csc13001.plantpos.utils.http.JsonModel;
import io.swagger.v3.oas.annotations.media.Schema;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.EqualsAndHashCode;
import lombok.NoArgsConstructor;

@Data
@NoArgsConstructor
@AllArgsConstructor
@EqualsAndHashCode(callSuper = true)
public class LoginResponseDTO extends JsonModel {

    @Schema(description = "Thông tin người dùng sau khi đăng nhập")
    private User user;

    @Schema(example = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...")
    private String accessToken;
}
