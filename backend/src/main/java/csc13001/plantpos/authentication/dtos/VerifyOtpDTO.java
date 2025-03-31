package csc13001.plantpos.authentication.dtos;

import csc13001.plantpos.utils.http.JsonModel;
import lombok.Data;
import lombok.EqualsAndHashCode;
import lombok.NoArgsConstructor;

@Data
@NoArgsConstructor
@EqualsAndHashCode(callSuper = true)
public class VerifyOtpDTO extends JsonModel {
    private String username;
    private String otp;
}