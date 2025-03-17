package csc13001.plantpos.application.dtos.auth;

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
    @NotBlank(message = "Fullname is required")
    @Size(min = 5, max = 256, message = "Fullname must be between 5 and 256 characters")
    private String fullname;

    @NotBlank(message = "Username is required")
    @Size(min = 5, max = 50, message = "Username must be between 5 and 50 characters")
    private String username;

    @NotBlank(message = "Password is required")
    @Size(min = 5, max = 50, message = "Password must be between 5 and 50 characters")
    private String password;
}
