package csc13001.plantpos.chat;

import csc13001.plantpos.utils.http.HttpResponse;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.validation.BindingResult;
import org.springframework.validation.annotation.Validated;
import org.springframework.web.bind.annotation.*;
import java.util.List;

@RestController
@RequestMapping("/api/messages")
public class MessageController {

    @Autowired
    private MessageService messageService;

    @GetMapping("/user/{userId}")
    public ResponseEntity<?> getMessagesByUserId(@PathVariable Long userId) {
        List<Message> messages = messageService.getMessagesByUserId(userId);
        return HttpResponse.ok("Lấy danh sách tin nhắn thành công", messages);
    }

    @PostMapping
    public ResponseEntity<?> createMessage(
            @RequestBody @Validated MessageDTO messageDTO,
            BindingResult bindingResult) {
        if (bindingResult.hasErrors()) {
            return HttpResponse.badRequest(bindingResult);
        }
        Message createdMessage = messageService.createMessage(messageDTO);
        return HttpResponse.ok("Tạo tin nhắn thành công", createdMessage);
    }

}
