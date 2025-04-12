package csc13001.plantpos.notification.dtos;

import java.util.List;

import csc13001.plantpos.notification.models.Notification.NotificationType;
import io.swagger.v3.oas.annotations.media.Schema;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
public class CreateNotificationDTO {
    @NotBlank(message = "Tựa đề không được để trống")
    @Schema(example = "Thông báo mới")
    private String title;

    @NotBlank(message = "Nội dung không được để trống")
    @Schema(example = "Nội dung thông báo")
    private String content;

    @NotNull(message = "Người nhận không được để trống")
    @Schema(example = "[1, 2, 3]")
    private List<Long> to;

    @Builder.Default
    @Schema(example = "Summary")
    private NotificationType type = NotificationType.OwnerAnnouncement;
}
