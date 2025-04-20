package csc13001.plantpos.chat;

import org.springframework.stereotype.Service;

import csc13001.plantpos.statistic.StatisticsService;
import csc13001.plantpos.statistic.TimeType;
import csc13001.plantpos.statistic.dtos.SalesStatisticsDTO;
import csc13001.plantpos.user.User;
import csc13001.plantpos.user.UserRepository;
import lombok.RequiredArgsConstructor;

import java.time.LocalDateTime;
import java.util.List;
import java.util.Map;

@Service
@RequiredArgsConstructor
public class MessageService {
    private final AIService aiService;
    private final UserRepository userRepository;
    private final MessageRepository messageRepository;
    private final StatisticsService statisticsService;

    public List<Message> getMessagesByUserId(Long userId) {
        return messageRepository.findByUser_UserId(userId);
    }

    public Message createMessage(MessageDTO messageDTO) {
        User user = userRepository.findById(messageDTO.getUserId())
                .orElseThrow(() -> new RuntimeException("Người dùng không tồn tại"));

        Message message = Message.builder()
                .user(user)
                .message(messageDTO.getMessage())
                .fromBot(false)
                .build();

        messageRepository.save(message);
        String userMessage = message.getMessage().toLowerCase();

        // ========================== ADMIN XỬ LÝ DOANH THU ==========================
        if (user.isAdmin()) {
            if (userMessage.contains("doanh thu")) {
                try {
                    Map<String, String> queryParams = aiService.extractRevenueQueryParams(userMessage);
                    System.err.println("Query Params: " + queryParams);

                    TimeType timeType = TimeType.valueOf(queryParams.get("timeType").toUpperCase());
                    LocalDateTime startDate = LocalDateTime.parse(queryParams.get("startDate"));
                    LocalDateTime endDate = LocalDateTime.parse(queryParams.get("endDate"));

                    // gọi service lấy thống kê
                    SalesStatisticsDTO stats = statisticsService.getSalesStatistics(timeType, startDate, endDate);

                    String prompt = String.format(
                            """
                                    Từ ngày %s đến %s:
                                    - Doanh thu: %.0f VND
                                    - Lợi nhuận: %.0f VND
                                    - Số đơn hàng: %d

                                    Hãy viết một câu trả lời thân thiện phù hợp với người hỏi là admin của hệ thống, dễ hiểu để phản hồi người dùng với các thông tin trên.
                                    """,
                            startDate, endDate,
                            stats.getRevenue(),
                            stats.getProfit(),
                            stats.getOrderCount());

                    String reply = aiService.chat(prompt);
                    return saveAndReturnBotMessage(user, reply);
                } catch (Exception e) {
                    e.printStackTrace();
                    return saveAndReturnBotMessage(user, "Xin lỗi, tôi không hiểu yêu cầu doanh thu của bạn.");
                }
            } else {
                String prompt = String.format(
                        """
                                Admin của hệ thống hỏi: %s
                                Hãy viết một câu trả lời thân thiện phù hợp với người hỏi là admin của hệ thống, dễ hiểu để phản hồi người dùng với các thông tin trên.
                                Thông tin admin: %s
                                """,
                        message.getMessage(), user.toString());
                String reply = aiService.chat(prompt);
                return saveAndReturnBotMessage(user, reply);
            }
        }

        // ========================== STAFF DÙNG OPENAI ==========================
        if (!user.isAdmin()) {
            String prompt = String.format(
                    """
                            Nhân viên của hệ thống hỏi: %s
                            Hãy viết một câu trả lời thân thiện phù hợp với người hỏi là nhân viên của hệ thống, dễ hiểu để phản hồi người dùng với các thông tin trên.
                            Thông tin nhân viên: %s
                            """,
                    message.getMessage(), user.toString());
            String reply = aiService.chat(prompt);
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
}
