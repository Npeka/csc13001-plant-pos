package csc13001.plantpos.infrastructure;

import csc13001.plantpos.domain.events.NotificationEvent;
import org.springframework.context.event.EventListener;
import org.springframework.scheduling.annotation.Async;
import org.springframework.stereotype.Service;
//import org.springframework.mail.javamail.JavaMailSender;
//import org.springframework.mail.SimpleMailMessage;
//import org.springframework.stereotype.Service;


@Service
public class NotificationService {
//    @Autowired
//    private JavaMailSender mailSender;

    @Async
    @EventListener
    public void handleNotiEvent(NotificationEvent event) {
        System.out.println("Sending notification to " + event.getRecipient() + ": " + event.getMessage());
    }
}
