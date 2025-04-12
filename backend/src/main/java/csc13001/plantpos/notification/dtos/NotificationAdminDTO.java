package csc13001.plantpos.notification.dtos;

import java.time.LocalDateTime;
import java.util.List;

import com.fasterxml.jackson.annotation.JsonFormat;

import csc13001.plantpos.notification.models.Notification.NotificationType;
import csc13001.plantpos.user.User;
import lombok.Data;
import lombok.Builder;
import lombok.AllArgsConstructor;
import lombok.NoArgsConstructor;

@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
public class NotificationAdminDTO {
    private Long notificationUserId;
    private String title;
    private String content;
    private NotificationType type;
    private String typeName;

    @JsonFormat(shape = JsonFormat.Shape.STRING, pattern = "yyyy-MM-dd HH:mm:ss")
    private LocalDateTime createdAt;

    private List<User> users;
}
