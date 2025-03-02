package csc13001.plantpos.application.services;

import csc13001.plantpos.adapters.repositories.UserRepository;
import csc13001.plantpos.application.dtos.auth.LoginDTO;
import csc13001.plantpos.application.dtos.auth.LoginResponseDTO;
import csc13001.plantpos.application.dtos.auth.RegisterDTO;
import csc13001.plantpos.application.dtos.auth.ResetPassworDTO;
import csc13001.plantpos.config.JwtUtil;
import csc13001.plantpos.domain.models.User;
import csc13001.plantpos.exception.auth.AuthException;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.ApplicationEventPublisher;
import org.springframework.security.crypto.bcrypt.BCryptPasswordEncoder;
import org.springframework.stereotype.Service;

import java.util.Optional;

@Service
public class AuthService {
    @Autowired
    private JwtUtil jwtUtil;

    @Autowired
    private UserRepository userRepository;

    @Autowired
    private ApplicationEventPublisher eventPublisher;

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

        Optional<User> userOptional = userRepository.findByUsername(username);
        if (userOptional.isEmpty()) {
            throw new AuthException.UserNotFoundException();
        }

        User user = userOptional.get();
        if (!bCryptPasswordEncoder.matches(password, user.getPassword())) {
            throw new AuthException.InvalidPasswordException();
        }

        user.setPassword(null);
        String accessToken = jwtUtil.generateToken(username, user.getRole());
        return new LoginResponseDTO(user, accessToken);
    }

    public void forgotPassword(String username) {
        boolean isExist = userRepository.findByUsername(username).isPresent();
        if (!isExist) {
            throw new AuthException.UserNotFoundException();
        }
    }

    public boolean verifyOtp(String username, String otp) {
        return true;
    }

    public void resetPassword(ResetPassworDTO resetPassworDTO) {
        String username = resetPassworDTO.getUsername();
        String newPassword = resetPassworDTO.getNewPassword();
        String confirmPassword = resetPassworDTO.getConfirmPassword();

        if (!newPassword.equals(confirmPassword)) {
            throw new AuthException.InvalidPasswordException();
        }

        Optional<User> userOptional = userRepository.findByUsername(username);
        if (userOptional.isPresent()) {
            User user = userOptional.get();
            user.setPassword(bCryptPasswordEncoder.encode(newPassword));
            userRepository.save(user);
            // eventPublisher.publishEvent();
        } else {
            throw new AuthException.UserNotFoundException();
        }
    }
}
