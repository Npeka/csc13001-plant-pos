package csc13001.plantpos.application.services;

import csc13001.plantpos.domain.events.NotificationEvent;
import csc13001.plantpos.domain.events.OtpEvent;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.event.EventListener;
import org.springframework.mail.SimpleMailMessage;
import org.springframework.mail.javamail.JavaMailSender;
import org.springframework.scheduling.annotation.Async;
import org.springframework.stereotype.Service;


@Service
public class NotificationService {

    @Autowired
    private JavaMailSender mailSender;

    @Async
    @EventListener
    public void handleNotiEvent(NotificationEvent event) {

    }

    @Async
    @EventListener
    public void handlerOtpEven(OtpEvent event) {
//        sendEmail(event.getRecipient(), "OTP Verification", event.getMessage());
    }

    private void sendEmail(String to, String subject, String text) {
        SimpleMailMessage message = new SimpleMailMessage();
        message.setTo(to);
        message.setSubject(subject);
        message.setText(text);
        mailSender.send(message);
    }
}
