package csc13001.plantpos.authentication;

import csc13001.plantpos.authentication.dtos.LoginDTO;
import csc13001.plantpos.authentication.dtos.LoginResponseDTO;
import csc13001.plantpos.authentication.dtos.RegisterDTO;
import csc13001.plantpos.authentication.dtos.ResetPassworDTO;
import csc13001.plantpos.authentication.exception.AuthException;
import csc13001.plantpos.config.JwtUtil;
import csc13001.plantpos.domain.events.OtpEvent;
import csc13001.plantpos.user.User;
import csc13001.plantpos.user.UserRepository;
import csc13001.plantpos.user.WorkLogRepository;
import csc13001.plantpos.user.models.WorkLog;
import io.jsonwebtoken.Claims;
import lombok.RequiredArgsConstructor;

import org.springframework.context.ApplicationEventPublisher;
import org.springframework.data.redis.core.StringRedisTemplate;
import org.springframework.security.crypto.bcrypt.BCryptPasswordEncoder;
import org.springframework.stereotype.Service;

import java.time.Duration;
import java.time.LocalDateTime;
import java.util.concurrent.ThreadLocalRandom;
import java.util.concurrent.TimeUnit;

@Service
@RequiredArgsConstructor
public class AuthService {
    private final JwtUtil jwtUtil;
    private final UserRepository userRepository;
    private final WorkLogRepository workLogRepository;
    private final ApplicationEventPublisher eventPublisher;
    private final StringRedisTemplate redisTemplate;
    private static final long OTP_EXPIRATION_TIME = 5;

    private final BCryptPasswordEncoder bCryptPasswordEncoder = new BCryptPasswordEncoder();

    public void register(RegisterDTO registerDTO) {
        String fullname = registerDTO.getFullname();
        String username = registerDTO.getUsername();
        String password = registerDTO.getPassword();

        if (userRepository.findByUsername(username).isPresent()) {
            throw new AuthException.UsernameExistsException();
        }

        String hashedPassword = bCryptPasswordEncoder.encode(password);
        User newUser = new User(fullname, username, hashedPassword);
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

    public void logout(String token) {
        Claims claims = jwtUtil.extractClaims(token);
        if (claims == null) {
            throw new AuthException.InvalidTokenException();
        }

        User user = userRepository.findByUsername(claims.getSubject())
                .orElseThrow(AuthException.UserNotFoundException::new);

        String key = "blacklist:" + token;
        long expiration = claims.getExpiration().getTime() - System.currentTimeMillis();
        redisTemplate.opsForValue().set(key, "blacklisted", expiration, TimeUnit.MILLISECONDS);

        LocalDateTime loginTime = LocalDateTime
                .ofInstant(claims.getIssuedAt().toInstant(), java.time.ZoneId.systemDefault());
        LocalDateTime logoutTime = LocalDateTime
                .ofInstant(claims.getExpiration().toInstant(), java.time.ZoneId.systemDefault());

        Duration duration = Duration.between(loginTime, logoutTime);
        String workDuration = String.format("%02d:%02d:%02d",
                duration.toHours(),
                duration.toMinutesPart(),
                duration.toSecondsPart());
        WorkLog workLog = WorkLog.builder()
                .user(user)
                .loginTime(loginTime)
                .logoutTime(logoutTime)
                .workDuration(workDuration)
                .build();

        workLogRepository.save(workLog);
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
            throw new AuthException.InvalidOtpException();
        }

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

        String key = "otp:" + username + ":otp";
        redisTemplate.delete(key);
    }
}
