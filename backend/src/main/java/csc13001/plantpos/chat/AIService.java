package csc13001.plantpos.chat;

import org.springframework.beans.factory.annotation.Value;
import org.springframework.http.*;
import org.springframework.stereotype.Service;
import org.springframework.web.reactive.function.client.WebClient;
import com.fasterxml.jackson.core.type.TypeReference;
import com.fasterxml.jackson.databind.ObjectMapper;

import java.time.LocalDateTime;
import java.util.List;
import java.util.Map;

@Service
public class AIService {

    private final String apiUrl;
    private final WebClient client;
    private final ObjectMapper objectMapper = new ObjectMapper();

    private static final String BASE_SYSTEM_PROMPT = """
            Bạn là một hệ thống POS thanh toán cho cửa hàng bán cây cảnh tên là PlantPOS.
            Nhiệm vụ của bạn là trả lời các câu hỏi liên quan đến cửa hàng, sản phẩm, dịch vụ và các vấn đề liên quan đến cửa hàng bán cây.

            Bạn chỉ được phép trả lời các câu hỏi liên quan đến:
            - Các sản phẩm cây cảnh, cách chăm sóc cây, cây cảnh có sẵn trong cửa hàng.
            - Thông tin về doanh thu, lợi nhuận, số lượng đơn hàng và các thông tin về hoạt động kinh doanh của cửa hàng (với quyền truy cập của người hỏi).
            - Thông tin về dịch vụ của cửa hàng, các chương trình khuyến mãi, hoặc dịch vụ hỗ trợ khách hàng.
            - Các vấn đề kỹ thuật liên quan đến hệ thống bán hàng của cửa hàng.

            Bạn **không được phép** trả lời bất kỳ câu hỏi nào không liên quan đến cửa hàng bán cây, ví dụ như:
            - Các câu hỏi về công nghệ, lịch sử, văn hóa, chính trị, tôn giáo, hay các vấn đề ngoài phạm vi của cửa hàng.
            - Các câu hỏi cá nhân hoặc các câu hỏi không liên quan đến việc bán cây.
            - Cung cấp thông tin nhạy cảm hoặc bảo mật như mật khẩu, thông tin tài chính, hoặc bất kỳ thông tin bảo mật nào của hệ thống.

            Khi người hỏi là quản trị viên (Admin), bạn có thể cung cấp thông tin nhạy cảm như doanh thu, lợi nhuận, số lượng đơn hàng, và các thông tin liên quan đến hoạt động của cửa hàng, nhưng luôn tuân thủ nguyên tắc không chia sẻ các thông tin bảo mật.

            Khi người hỏi là nhân viên, bạn có thể cung cấp các thông tin chi tiết về sản phẩm, dịch vụ và các vấn đề kỹ thuật hỗ trợ công việc của họ, nhưng luôn đảm bảo rằng các câu trả lời phải dễ hiểu và thân thiện.
            Hãy tập trung vào việc giải đáp các câu hỏi liên quan đến cây cảnh và các vấn đề của cửa hàng.
            """;

    private static final String REVENUE_SYSTEM_PROMPT = """
            Bạn là một hệ thống phân tích truy vấn doanh thu.
            Nhiệm vụ của bạn là đọc câu hỏi của người dùng, xác định khoảng thời gian mà họ hỏi về doanh thu, và trả về một JSON như sau:

            {
              "timeType": "DAILY|MONTHLY|YEARLY",
              "startDate": "yyyy-MM-dd'T'HH:mm:ss",
              "endDate": "yyyy-MM-dd'T'HH:mm:ss"
            }

            Nếu người dùng không nói rõ thời gian, hãy giả định timeType = "DAILY" và dùng ngày hôm nay (giờ 00:00:00 đến 23:59:59).
            Thời gian hiện tại ở thế giới thực là %s, hãy lưu ý điều này khi phân tích câu hỏi của người dùng.
            Chỉ trả về JSON, không cần giải thích gì thêm.
            """;

    public AIService(@Value("${gemini.api.key}") String apiKey) {
        this.apiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key="
                + apiKey;
        this.client = WebClient.builder()
                .defaultHeader(HttpHeaders.CONTENT_TYPE, MediaType.APPLICATION_JSON_VALUE)
                .build();
    }

    public String chat(String prompt) {
        return sendRequest(
                List.of(
                        Map.of("role", "user", "parts", List.of(Map.of("text", BASE_SYSTEM_PROMPT))),
                        Map.of("role", "user", "parts", List.of(Map.of("text", prompt)))));
    }

    public Map<String, String> extractRevenueQueryParams(String message) {
        String json = sendRequest(
                List.of(
                        Map.of("role", "user", "parts",
                                List.of(Map.of("text", String.format(REVENUE_SYSTEM_PROMPT, LocalDateTime.now())))),
                        Map.of("role", "user", "parts", List.of(Map.of("text", message)))));

        json = json.strip();
        if (json.startsWith("```")) {
            int firstNewline = json.indexOf('\n');
            int lastBackticks = json.lastIndexOf("```");
            if (firstNewline != -1 && lastBackticks > firstNewline) {
                json = json.substring(firstNewline + 1, lastBackticks).trim();
            }
        }

        try {
            return objectMapper.readValue(json, new TypeReference<>() {
            });
        } catch (Exception e) {
            throw new RuntimeException("Lỗi khi parse JSON từ Gemini: " + json, e);
        }
    }

    private String sendRequest(List<Map<String, Object>> contents) {
        Map<String, Object> body = Map.of("contents", contents);

        // LOG request body
        try {
            System.out.println("==== Gemini REQUEST ====");
            System.out.println(objectMapper.writerWithDefaultPrettyPrinter().writeValueAsString(body));
        } catch (Exception e) {
            System.out.println("Lỗi khi log request: " + e.getMessage());
        }

        Map<String, Object> response = client.post()
                .uri(apiUrl)
                .contentType(MediaType.APPLICATION_JSON)
                .bodyValue(body)
                .retrieve()
                .bodyToMono(Map.class)
                .block();

        // LOG raw response
        try {
            System.out.println("==== Gemini RESPONSE ====");
            System.out.println(objectMapper.writerWithDefaultPrettyPrinter().writeValueAsString(response));
        } catch (Exception e) {
            System.out.println("Lỗi khi log response: " + e.getMessage());
        }

        List<Map<String, Object>> candidates = (List<Map<String, Object>>) response.get("candidates");
        if (candidates == null || candidates.isEmpty())
            return "Không có phản hồi từ Gemini.";

        Map<String, Object> content = (Map<String, Object>) candidates.get(0).get("content");
        List<Map<String, Object>> parts = (List<Map<String, Object>>) content.get("parts");
        return (String) parts.get(0).get("text");
    }

}
