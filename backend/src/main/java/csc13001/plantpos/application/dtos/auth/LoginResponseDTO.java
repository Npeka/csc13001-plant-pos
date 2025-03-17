package csc13001.plantpos.application.dtos.auth;

import csc13001.plantpos.domain.models.User;
import csc13001.plantpos.utils.http.JsonModel;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.EqualsAndHashCode;
import lombok.NoArgsConstructor;

@Data
@NoArgsConstructor
@AllArgsConstructor
@EqualsAndHashCode(callSuper = true)
public class LoginResponseDTO extends JsonModel {
    private User user;
    private String accessToken;
}
