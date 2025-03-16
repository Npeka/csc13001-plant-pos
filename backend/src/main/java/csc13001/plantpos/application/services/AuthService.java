package csc13001.plantpos.application.services;

import csc13001.plantpos.adapters.repositories.UserRepository;
import csc13001.plantpos.application.dtos.auth.LoginDTO;
import csc13001.plantpos.application.dtos.auth.LoginResponseDTO;
import csc13001.plantpos.application.dtos.auth.RegisterDTO;
import csc13001.plantpos.application.dtos.auth.ResetPassworDTO;
import csc13001.plantpos.config.JwtUtil;
import csc13001.plantpos.domain.events.OtpEvent;
import csc13001.plantpos.domain.models.User;
import csc13001.plantpos.exception.auth.AuthException;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.ApplicationEventPublisher;
import org.springframework.data.redis.core.StringRedisTemplate;
import org.springframework.security.crypto.bcrypt.BCryptPasswordEncoder;
import org.springframework.stereotype.Service;

import java.util.concurrent.ThreadLocalRandom;
import java.util.concurrent.TimeUnit;

@Service
public class AuthService {
    @Autowired
    private JwtUtil jwtUtil;

    @Autowired
    private UserRepository userRepository;

    @Autowired
    private ApplicationEventPublisher eventPublisher;

    @Autowired
    private StringRedisTemplate redisTemplate;
    private static final long OTP_EXPIRATION_TIME = 5;

    private final BCryptPasswordEncoder bCryptPasswordEncoder = new BCryptPasswordEncoder();

    public void register(RegisterDTO registerDTO) {
        String username = registerDTO.getUsername();
        String password = registerDTO.getPassword();

        if (userRepository.findByUsername(username).isPresent()) {
            throw new AuthException.UsernameExistsException();
        }

        String hashedPassword = bCryptPasswordEncoder.encode(password);
        User newUser = new User(username, hashedPassword);
        userRepository.save(newUser);
    }

    public LoginResponseDTO login(LoginDTO loginDTO) {
        String username = loginDTO.getUsername();
        String password = loginDTO.getPassword();

        User user = userRepository.findByUsername(username)
                .orElseThrow(AuthException.UserNotFoundException::new);

        if (!bCryptPasswordEncoder.matches(password, user.getPassword())) {
            throw new AuthException.InvalidPasswordException();
        }

        user.setPassword(null);
        String accessToken = jwtUtil.generateToken(username, user.getRole());
        return new LoginResponseDTO(user, accessToken);
    }

    public void forgotPassword(String username) {
        User user = userRepository.findByUsername(username)
                .orElseThrow(AuthException.UserNotFoundException::new);

        String key = "otp:" + username + ":otp";
        int otp = ThreadLocalRandom.current().nextInt(100000, 1000000);
        redisTemplate.opsForValue().set(key, String.valueOf(otp), OTP_EXPIRATION_TIME, TimeUnit.MINUTES);

        OtpEvent event = new OtpEvent(user.getEmail(), String.valueOf(otp));
        eventPublisher.publishEvent(event);
    }

    public boolean verifyOtp(String username, String otp) {
        String key = "otp:" + username + ":otp";
        String storedOtp = redisTemplate.opsForValue().get(key);
        if (storedOtp == null || !storedOtp.equals(otp)) {
            return false;
        }

        redisTemplate.delete(key);
        return true;
    }

    public void resetPassword(ResetPassworDTO resetPassworDTO) {
        String username = resetPassworDTO.getUsername();
        String newPassword = resetPassworDTO.getNewPassword();
        String confirmPassword = resetPassworDTO.getConfirmPassword();

        if (!newPassword.equals(confirmPassword)) {
            throw new AuthException.InvalidPasswordException();
        }

        User user = userRepository.findByUsername(username)
                .orElseThrow(AuthException.UserNotFoundException::new);

        user.setPassword(bCryptPasswordEncoder.encode(newPassword));
        userRepository.save(user);
    }
}
