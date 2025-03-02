package csc13001.plantpos.adapters.controllers;

import csc13001.plantpos.application.dtos.auth.LoginDTO;
import csc13001.plantpos.application.dtos.auth.LoginResponseDTO;
import csc13001.plantpos.application.dtos.auth.RegisterDTO;
import csc13001.plantpos.application.dtos.auth.ResetPassworDTO;
import csc13001.plantpos.application.services.AuthService;
import csc13001.plantpos.utils.http.HttpResponse;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.validation.BindingResult;
import org.springframework.validation.annotation.Validated;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/api/auth")
public class AuthController {

    @Autowired
    private AuthService authService;

    @PostMapping("/register")
    public ResponseEntity<?> register(
            @RequestBody @Validated RegisterDTO registerDTO,
            BindingResult bindingResult) {
        if (bindingResult.hasErrors()) {
            return HttpResponse.invalidInputData();
        }

        authService.register(registerDTO);

        return HttpResponse.ok("Registration successful");
    }

    @PostMapping("/login")
    public ResponseEntity<?> login(
            @RequestBody @Validated LoginDTO loginDTO,
            BindingResult bindingResult) {
        if (bindingResult.hasErrors()) {
            return HttpResponse.invalidInputData();
        }

        LoginResponseDTO loginResponseDTO = authService.login(loginDTO);

        return HttpResponse.ok("Login successful", loginResponseDTO);
    }

    @PostMapping("/forgot-password")
    public ResponseEntity<?> forgotPassword(@RequestParam String username) {
        authService.forgotPassword(username);
        return HttpResponse.ok("Password reset instructions sent to your email");
    }

    @PostMapping("/verify-otp")
    public ResponseEntity<?> verifyOtp(
            @RequestParam String username,
            @RequestParam String otp) {
        boolean result = authService.verifyOtp(username, otp);

        if (!result) {
            return HttpResponse.invalidInputData();
        }

        return HttpResponse.ok("OTP verified successfully", result);
    }

    @PostMapping("/reset-password")
    public ResponseEntity<?> resetPassword(
            @RequestBody @Validated ResetPassworDTO resetPassworDTO,
            BindingResult bindingResult) {
        if (bindingResult.hasErrors()) {
            return HttpResponse.invalidInputData();
        }

        authService.resetPassword(resetPassworDTO);

        return HttpResponse.ok("Password reset successfully");
    }

    @GetMapping("/staff/test-jwt")
    public ResponseEntity<?> jwtStaff() {
        return HttpResponse.ok("Test JWT Staff successful");
    }

    @GetMapping("/admin/test-jwt")
    public ResponseEntity<?> jwtAdmin() {
        return HttpResponse.ok("Test JWT Admin successful");
    }

}
