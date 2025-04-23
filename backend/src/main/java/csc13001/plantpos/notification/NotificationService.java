package csc13001.plantpos.notification;

import lombok.RequiredArgsConstructor;

import java.io.UnsupportedEncodingException;
import java.text.DecimalFormat;
import java.text.DecimalFormatSymbols;
import java.util.ArrayList;
import java.util.List;
import java.util.Locale;
import java.util.Map;
import java.util.stream.Collectors;

import org.springframework.context.event.EventListener;
import org.springframework.mail.javamail.JavaMailSender;
import org.springframework.mail.javamail.MimeMessageHelper;
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
import csc13001.plantpos.statistic.dtos.SalesStatisticsDTO;
import csc13001.plantpos.user.User;
import csc13001.plantpos.user.UserRepository;
import jakarta.mail.MessagingException;
import jakarta.mail.internet.MimeMessage;

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
        String htmlContent = "<html>" +
                "<body>" +
                "<h2>Xác thực OTP</h2>" +
                "<p>Mã OTP của bạn là:</p>" +
                "<div style='font-size: 24px; font-weight: bold; color: #2c3e50;'>" + event.getOtp() + "</div>" +
                "<p>Vui lòng không chia sẻ mã này với bất kỳ ai.</p>" +
                "</body>" +
                "</html>";

        sendEmail(event.getEmail(), "Xác thực OTP", htmlContent);
    }

    @Async
    @EventListener
    public void handleSaleStatisticsEvent(SalesStatisticsDTO event) {
        DecimalFormatSymbols symbols = new DecimalFormatSymbols(Locale.of("vi", "VN"));
        symbols.setGroupingSeparator('.');
        symbols.setDecimalSeparator(',');

        DecimalFormat df = new DecimalFormat("#,##0.##", symbols);
        String content = """
                    <html>
                      <body style="font-family: Arial, sans-serif; color: #333;">
                        <div style="max-width: 600px; margin: auto; padding: 20px; border: 1px solid #eee; border-radius: 8px;">
                          <h2 style="color: #4CAF50; text-align: center;">📊 Thống kê doanh thu</h2>
                          <table style="width: 100%%; border-collapse: collapse; margin-top: 20px;">
                            <tr style="background-color: #E8F5E9;">
                              <td style="padding: 10px; font-weight: bold;">Doanh thu</td>
                              <td style="padding: 10px; text-align: right; color: #4CAF50;">%s đ</td>
                            </tr>
                            <tr>
                              <td style="padding: 10px; font-weight: bold;">Lợi nhuận</td>
                              <td style="padding: 10px; text-align: right;">%s đ</td>
                            </tr>
                            <tr style="background-color: #E8F5E9;">
                              <td style="padding: 10px; font-weight: bold;">Số đơn hàng</td>
                              <td style="padding: 10px; text-align: right;">%d</td>
                            </tr>
                            <tr>
                              <td style="padding: 10px; font-weight: bold;">Tăng trưởng doanh thu</td>
                              <td style="padding: 10px; text-align: right;">%s</td>
                            </tr>
                            <tr style="background-color: #E8F5E9;">
                              <td style="padding: 10px; font-weight: bold;">Tăng trưởng lợi nhuận</td>
                              <td style="padding: 10px; text-align: right;">%s</td>
                            </tr>
                            <tr>
                              <td style="padding: 10px; font-weight: bold;">Tăng trưởng số đơn hàng</td>
                              <td style="padding: 10px; text-align: right;">%s</td>
                            </tr>
                          </table>
                          <p style="text-align: center; margin-top: 30px; font-size: 12px; color: #888;">
                            Đây là email tự động từ hệ thống, vui lòng không phản hồi.
                          </p>
                        </div>
                      </body>
                    </html>
                """
                .formatted(
                        df.format(event.getRevenue()),
                        df.format(event.getProfit()),
                        event.getOrderCount(),
                        df.format(event.getRevenueGrowthRate()) + " %",
                        df.format(event.getProfitGrowthRate()) + " %",
                        String.valueOf(event.getOrderCountGrowthRate()) + " %");

        List<User> adminUsers = userRepository.findAllByIsAdmin(true);
        if (adminUsers.isEmpty()) {
            throw new RuntimeException("Người nhận không tồn tại");
        }

        adminUsers.forEach(user -> {
            sendEmail(user.getEmail(), "Thống kê doanh thu", content);
        });
    }

    private void sendEmail(String to, String subject, String htmlContent) {
        try {
            MimeMessage message = mailSender.createMimeMessage();
            MimeMessageHelper helper = new MimeMessageHelper(message, true, "UTF-8");

            helper.setFrom("seimeicc@gmail.com", "Plant POS");
            helper.setTo(to);
            helper.setSubject(subject);
            helper.setText(htmlContent, true);

            mailSender.send(message);
        } catch (MessagingException | UnsupportedEncodingException e) {
            throw new RuntimeException("Lỗi khi gửi email", e);
        }
    }

}
