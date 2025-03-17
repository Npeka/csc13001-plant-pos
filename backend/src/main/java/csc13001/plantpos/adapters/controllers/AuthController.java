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
import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.Parameter;

@RestController
@RequestMapping("/api/auth")
public class AuthController {

    @Autowired
    private AuthService authService;

    @Operation(summary = "Register a new account", description = "Create a new account with registration details")
    @PostMapping("/register")
    public ResponseEntity<?> register(
            @RequestBody @Validated RegisterDTO registerDTO,
            BindingResult bindingResult) {
        if (bindingResult.hasErrors()) {
            return HttpResponse.badRequest(bindingResult);
        }

        authService.register(registerDTO);

        return HttpResponse.ok("Registration successful");
    }

    @Operation(summary = "Login to the system", description = "Authenticate user with login credentials")
    @PostMapping("/login")
    public ResponseEntity<?> login(
            @RequestBody @Validated LoginDTO loginDTO,
            BindingResult bindingResult) {
        if (bindingResult.hasErrors()) {
            return HttpResponse.badRequest(bindingResult);
        }

        LoginResponseDTO loginResponseDTO = authService.login(loginDTO);

        return HttpResponse.ok("Login successful", loginResponseDTO);
    }

    @Operation(summary = "Forgot password", description = "Send password reset instructions to the user's email")
    @PostMapping("/forgot-password")
    public ResponseEntity<?> forgotPassword(
            @Parameter(description = "Username of the account") @RequestParam String username) {
        authService.forgotPassword(username);
        return HttpResponse.ok("Password reset instructions sent to your email");
    }

    @Operation(summary = "Verify OTP", description = "Verify the OTP sent to the user's email")
    @PostMapping("/verify-otp")
    public ResponseEntity<?> verifyOtp(
            @Parameter(description = "Username of the account") @RequestParam String username,
            @Parameter(description = "OTP sent to the user's email") @RequestParam String otp) {
        boolean result = authService.verifyOtp(username, otp);

        if (!result) {
            return HttpResponse.badRequest("Invalid Username or OTP");
        }

        return HttpResponse.ok("OTP verified successfully", result);
    }

    @Operation(summary = "Reset password", description = "Reset the user's password using the provided details")
    @PostMapping("/reset-password")
    public ResponseEntity<?> resetPassword(
            @RequestBody @Validated ResetPassworDTO resetPassworDTO,
            BindingResult bindingResult) {
        if (bindingResult.hasErrors()) {
            return HttpResponse.badRequest(bindingResult);
        }

        authService.resetPassword(resetPassworDTO);

        return HttpResponse.ok("Password reset successfully");
    }
}
