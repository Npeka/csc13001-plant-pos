package csc13001.plantpos.chat;

import org.springframework.stereotype.Service;

import csc13001.plantpos.user.User;
import csc13001.plantpos.user.UserRepository;
import lombok.RequiredArgsConstructor;

import java.util.List;
import java.util.Map;

@Service
@RequiredArgsConstructor
public class MessageService {
    private final OpenAIService openAIService;
    private final UserRepository userRepository;
    private final MessageRepository messageRepository;

    public List<Message> getMessagesByUserId(Long userId) {
        return messageRepository.findByUser_UserId(userId);
    }

    public Message createMessage(Message message) {
        User user = userRepository.findById(message.getUser().getUserId())
                .orElseThrow(() -> new RuntimeException("Người dùng không tồn tại"));

        messageRepository.save(message);
        String role = user.getRole(); // "ADMIN" | "STAFF"
        String userMessage = message.getMessage().toLowerCase();

        // ========================== ADMIN XỬ LÝ DOANH THU ==========================
        if (user.isAdmin() && userMessage.contains("doanh thu")) {
            String reply = mockDoanhThuReply(userMessage);
            return saveAndReturnBotMessage(user, reply);
        }

        // ========================== STAFF DÙNG OPENAI ==========================
        if (!user.isAdmin()) {
            String systemPrompt = getSystemPrompt(role);
            List<Map<String, String>> messages = List.of(
                    Map.of("role", "system", "content", systemPrompt),
                    Map.of("role", "user", "content", userMessage));
            String reply = openAIService.chat(messages);
            return saveAndReturnBotMessage(user, reply);
        }

        // ========================== KHÔNG XỬ LÝ ĐƯỢC ==========================
        String fallback = "Xin lỗi, tôi không thể giúp bạn với yêu cầu này.";
        return saveAndReturnBotMessage(user, fallback);
    }

    private Message saveAndReturnBotMessage(User user, String reply) {
        return messageRepository.save(
                Message.builder()
                        .user(user)
                        .message(reply)
                        .fromBot(true)
                        .build());
    }

    private String getSystemPrompt(String role) {
        return switch (role) {
            case "ADMIN" ->
                "Bạn là trợ lý cho quản trị viên cửa hàng. Trả lời các câu hỏi về doanh thu, thống kê, đơn hàng. Nếu không liên quan, từ chối.";
            case "STAFF" ->
                "Bạn là trợ lý cho nhân viên chăm sóc cây. Hướng dẫn chăm cây, xử lý sâu bệnh, thời tiết ảnh hưởng cây, tưới nước, v.v.";
            default -> "Bạn là trợ lý ảo.";
        };
    }

    private String mockDoanhThuReply(String message) {
        if (message.contains("hôm nay"))
            return "Doanh thu hôm nay là 5.200.000đ.";
        if (message.contains("hôm qua"))
            return "Doanh thu hôm qua là 4.700.000đ.";
        if (message.contains("tháng"))
            return "Doanh thu tháng này là 125.000.000đ.";
        if (message.contains("năm"))
            return "Doanh thu năm nay là 1.560.000.000đ.";
        if (message.matches(".*\\d{4}-\\d{2}-\\d{2}.*"))
            return "Doanh thu ngày đó là 3.200.000đ.";
        return "Tôi không hiểu yêu cầu doanh thu của bạn. Vui lòng hỏi rõ hơn.";
    }
}
