package csc13001.plantpos.domain.events;

import lombok.Getter;
import org.springframework.context.ApplicationEvent;

@Getter
public class OtpEvent extends ApplicationEvent {
    private final String email;
    private final String otp;

    public OtpEvent(String email, String otp) {
        super(email);
        this.email = email;
        this.otp = otp;
    }
}
