package csc13001.plantpos.notification;

import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import csc13001.plantpos.notification.dtos.CreateNotificationDTO;
import csc13001.plantpos.notification.dtos.NotificationDTO;
import csc13001.plantpos.utils.http.HttpResponse;
import io.swagger.v3.oas.annotations.Parameter;
import lombok.RequiredArgsConstructor;

import java.util.List;

import org.springframework.http.ResponseEntity;
import org.springframework.validation.BindingResult;
import org.springframework.validation.annotation.Validated;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PatchMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;

@RestController
@RequiredArgsConstructor
@RequestMapping("/api/notifications")
public class NotificationController {
    private final NotificationService notificationService;

    @GetMapping("{staffId}")
    public ResponseEntity<?> getNotificationByStaffId(@Parameter(example = "2") @PathVariable Long staffId) {
        List<NotificationDTO> notificationUsers = notificationService.getNotificationByStaffId(staffId);
        return HttpResponse.ok("Lấy thông báo thành công", notificationUsers);
    }

    @PostMapping
    public ResponseEntity<?> createNotification(
            @RequestBody @Validated CreateNotificationDTO notificationDTO,
            BindingResult bindingResult) {
        if (bindingResult.hasErrors()) {
            return ResponseEntity.badRequest().body(bindingResult.getAllErrors());
        }

        notificationService.createNotification(notificationDTO);
        return HttpResponse.ok("Tạo thông báo thành công");
    }

    @PatchMapping("{notificationUserId}/read")
    public ResponseEntity<?> markAsRead(@Parameter(example = "3") @PathVariable Long notificationUserId) {
        notificationService.markAsRead(notificationUserId);
        return HttpResponse.ok("Đánh dấu thông báo đã đọc thành công");
    }
}
