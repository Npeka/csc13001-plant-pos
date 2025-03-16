package csc13001.plantpos.application.dtos.auth;

import csc13001.plantpos.utils.http.JsonModel;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.Size;
import lombok.Data;
import lombok.EqualsAndHashCode;
import lombok.NoArgsConstructor;
import io.swagger.v3.oas.annotations.media.Schema;

@Data
@NoArgsConstructor
@EqualsAndHashCode(callSuper = true)
public class LoginDTO extends JsonModel {
    @Schema(example = "admin")
    @NotBlank(message = "Username is required")
    @Size(min = 5, max = 50, message = "Username or password is invalid")
    private String username;

    @Schema(example = "admin")
    @NotBlank(message = "Password is required")
    @Size(min = 5, max = 50, message = "Username or password is invalid")
    private String password;
}
