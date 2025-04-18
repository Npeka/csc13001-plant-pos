package csc13001.plantpos.notification.events;

import lombok.*;

@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
public class NotificationEvent {
    private String recipient;
    private String message;
}
