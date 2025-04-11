package csc13001.plantpos.chat;

import org.springframework.stereotype.Service;

import lombok.RequiredArgsConstructor;

import java.util.List;

@Service
@RequiredArgsConstructor
public class MessageService {
    private final MessageRepository messageRepository;

    public List<Message> getMessagesByUserId(Long userId) {
        return messageRepository.findByUser_UserId(userId);
    }

    public Message createMessage(Message message) {
        return messageRepository.save(message);
    }
}
