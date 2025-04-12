package csc13001.plantpos.notification;

import lombok.RequiredArgsConstructor;

import java.text.DecimalFormat;
import java.text.DecimalFormatSymbols;
import java.util.ArrayList;
import java.util.List;
import java.util.Locale;
import java.util.Map;
import java.util.stream.Collectors;

import org.springframework.context.event.EventListener;
import org.springframework.mail.SimpleMailMessage;
import org.springframework.mail.javamail.JavaMailSender;
import org.springframework.scheduling.annotation.Async;
import org.springframework.stereotype.Service;

import csc13001.plantpos.customer.CustomerType;
import csc13001.plantpos.discount.DiscountProgram;
import csc13001.plantpos.notification.dtos.CreateNotificationDTO;
import csc13001.plantpos.notification.dtos.NotificationAdminDTO;
import csc13001.plantpos.notification.dtos.NotificationDTO;
import csc13001.plantpos.notification.events.OtpEvent;
import csc13001.plantpos.notification.models.Notification;
import csc13001.plantpos.notification.models.NotificationUser;
import csc13001.plantpos.notification.models.Notification.NotificationType;
import csc13001.plantpos.product.Product;
import csc13001.plantpos.user.User;
import csc13001.plantpos.user.UserRepository;

@Service
@RequiredArgsConstructor
public class NotificationService {
    private final JavaMailSender mailSender;
    private final UserRepository userRepository;
    private final NotificationRepository notificationRepository;
    private final NotificationUserRepository notificationUserRepository;

    public List<NotificationAdminDTO> getAllNotification() {
        List<NotificationUser> notiUsers = notificationUserRepository.findAllWithNotificationAndUser();
        notiUsers = notiUsers.stream()
                .filter(nu -> nu.getNotification().getType() == NotificationType.OwnerAnnouncement).toList();

        Map<Long, List<NotificationUser>> grouped = notiUsers.stream()
                .collect(Collectors.groupingBy(nu -> nu.getNotification().getNotificationId()));

        List<NotificationAdminDTO> result = new ArrayList<>();

        for (List<NotificationUser> group : grouped.values()) {
            Notification noti = group.get(0).getNotification();

            NotificationAdminDTO dto = NotificationAdminDTO.builder()
                    .notificationUserId(group.get(0).getNotificationUserId())
                    .title(noti.getTitle())
                    .content(noti.getContent())
                    .type(noti.getType())
                    .typeName(noti.getType().getName())
                    .createdAt(noti.getCreatedAt())
                    .users(group.stream().map(NotificationUser::getUser).toList())
                    .build();

            result.add(dto);
        }

        return result;
    }

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
        List<User> to = null;
        if (notificationDTO.getTo() == null || notificationDTO.getTo().isEmpty()) {
            to = userRepository.findAllByIsAdmin(false);
        } else {
            to = userRepository.findAllById(notificationDTO.getTo());
        }

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
    public void handleProductEvent(Product product) {
        DecimalFormatSymbols symbols = new DecimalFormatSymbols(Locale.of("vi", "VN"));
        symbols.setGroupingSeparator('.');
        symbols.setDecimalSeparator(',');
        DecimalFormat df = new DecimalFormat("#,##0.##", symbols);
        String formattedPrice = df.format(product.getSalePrice());

        CreateNotificationDTO notification = CreateNotificationDTO.builder()
                .title(product.getName())
                .content("Sản phẩm " + product.getName() + " với giá " + formattedPrice
                        + "đ thuộc dòng cây " + product.getCategory().getName()
                        + " đã được thêm vào kho. Vui lòng tư vấn cho khách hàn quan tâm.")
                .type(NotificationType.NewProduct)
                .build();

        this.createNotification(notification);

    }

    @Async
    @EventListener
    public void handleDiscountProgramEvent(DiscountProgram discountProgram) {
        String customerContent = "";
        if (discountProgram.getApplicableCustomerType() != CustomerType.All) {
            customerContent = "hạng " + discountProgram.getApplicableCustomerType().getName();
        }

        CreateNotificationDTO notification = null;
        if (discountProgram.isActive()) {
            notification = CreateNotificationDTO.builder()
                    .title(discountProgram.getName())
                    .content("Chương trình giảm giá " + discountProgram.getDiscountRate()
                            + "% cho tất cả các khách hàng "
                            + customerContent + " diễn ra từ ngày " + discountProgram.getStartDate() + " đến hết "
                            + discountProgram.getEndDate() + ". Vui lòng thông báo cho khách hàng khi họ thanh toán.")
                    .type(NotificationType.NewPromotion)
                    .build();
        } else {
            notification = CreateNotificationDTO.builder()
                    .title("Chương trình " + discountProgram.getName() + " đã kết thúc")
                    .content("Chương trình giảm giá " + discountProgram.getDiscountRate()
                            + "% cho tất cả các khách hàng " + customerContent
                            + " đã chính thức kết thúc. Vui lòng không áp dụng ưu đãi này khi thanh toán.")
                    .type(NotificationType.ExpirationNotice)
                    .build();
        }

        this.createNotification(notification);
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
