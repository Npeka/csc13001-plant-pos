package csc13001.plantpos.application.dtos.auth;

import csc13001.plantpos.domain.models.User;
import csc13001.plantpos.utils.http.JsonModel;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.EqualsAndHashCode;
import lombok.NoArgsConstructor;
import io.swagger.v3.oas.annotations.media.Schema;

@Data
@NoArgsConstructor
@AllArgsConstructor
@EqualsAndHashCode(callSuper = true)
public class LoginResponseDTO extends JsonModel {

    @Schema(description = "User details after login")
    private User user;

    @Schema(example = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...")
    private String accessToken;
}
