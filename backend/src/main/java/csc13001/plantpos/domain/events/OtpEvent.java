package csc13001.plantpos.domain.events;

import lombok.Getter;
import org.springframework.context.ApplicationEvent;

@Getter
public class OtpEvent extends ApplicationEvent {
    private final String recipient;
    private final String message;

    public OtpEvent(String recipient, String message) {
        super(recipient);
        this.recipient = recipient;
        this.message = message;
    }
}
