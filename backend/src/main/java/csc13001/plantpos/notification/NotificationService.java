package csc13001.plantpos.notification;

import lombok.RequiredArgsConstructor;

import java.util.List;

import org.springframework.context.event.EventListener;
import org.springframework.mail.SimpleMailMessage;
import org.springframework.mail.javamail.JavaMailSender;
import org.springframework.scheduling.annotation.Async;
import org.springframework.stereotype.Service;

import csc13001.plantpos.notification.dtos.CreateNotificationDTO;
import csc13001.plantpos.notification.dtos.NotificationDTO;
import csc13001.plantpos.notification.events.NotificationEvent;
import csc13001.plantpos.notification.events.OtpEvent;
import csc13001.plantpos.notification.models.Notification;
import csc13001.plantpos.notification.models.NotificationUser;
import csc13001.plantpos.user.User;
import csc13001.plantpos.user.UserRepository;

@Service
@RequiredArgsConstructor
public class NotificationService {
    private final JavaMailSender mailSender;
    private final UserRepository userRepository;
    private final NotificationRepository notificationRepository;
    private final NotificationUserRepository notificationUserRepository;

    public List<NotificationDTO> getNotificationByStaffId(Long staffId) {
        List<NotificationUser> notificationUsers = notificationUserRepository.findByUser_UserId(staffId);
        return notificationUsers.stream().map(
                notificationUser -> NotificationDTO.builder()
                        .notificationUserId(notificationUser.getNotificationUserId())
                        .title(notificationUser.getNotification().getTitle())
                        .content(notificationUser.getNotification().getContent())
                        .type(notificationUser.getNotification().getType())
                        .typeName(notificationUser.getNotification().getType().getName())
                        .createdAt(notificationUser.getNotification().getCreatedAt())
                        .isRead(notificationUser.getIsRead())
                        .build())
                .toList();
    }

    public void createNotification(CreateNotificationDTO notificationDTO) {
        List<User> to = userRepository.findAllById(notificationDTO.getTo());
        if (to.isEmpty()) {
            throw new RuntimeException("Người nhận không tồn tại");
        }

        Notification notification = Notification.builder()
                .title(notificationDTO.getTitle())
                .content(notificationDTO.getContent())
                .type(notificationDTO.getType())
                .build();

        notification = notificationRepository.save(notification);
        for (User user : to) {
            NotificationUser notificationUser = NotificationUser.builder()
                    .notification(notification)
                    .user(user)
                    .build();
            notificationUserRepository.save(notificationUser);
        }
    }

    public void markAsRead(Long notificationUserId) {
        NotificationUser notificationUser = notificationUserRepository.findById(notificationUserId)
                .orElseThrow(() -> new RuntimeException("Thông báo không tồn tại"));
        notificationUser.setIsRead(true);
        notificationUserRepository.save(notificationUser);
    }

    @Async
    @EventListener
    public void handleNotiEvent(NotificationEvent event) {

    }

    @Async
    @EventListener
    public void handlerOtpEven(OtpEvent event) {
        sendEmail(event.getEmail(), "Xác thực OTP", event.getOtp());
    }

    private void sendEmail(String to, String subject, String text) {
        SimpleMailMessage message = new SimpleMailMessage();
        message.setTo(to);
        message.setSubject(subject);
        message.setText(text);
        mailSender.send(message);
    }
}
