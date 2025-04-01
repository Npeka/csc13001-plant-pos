package csc13001.plantpos.authentication;

import csc13001.plantpos.authentication.dtos.ForgotPassworDTO;
import csc13001.plantpos.authentication.dtos.LoginDTO;
import csc13001.plantpos.authentication.dtos.LoginResponseDTO;
import csc13001.plantpos.authentication.dtos.RegisterDTO;
import csc13001.plantpos.authentication.dtos.ResetPassworDTO;
import csc13001.plantpos.authentication.dtos.VerifyOtpDTO;
import csc13001.plantpos.utils.http.HttpResponse;
import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.Parameter;
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

    @Operation(summary = "Đăng ký tài khoản mới", description = "Tạo tài khoản mới với thông tin đăng ký")
    @PostMapping("/register")
    public ResponseEntity<?> register(
            @RequestBody @Validated RegisterDTO registerDTO,
            BindingResult bindingResult) {
        if (bindingResult.hasErrors()) {
            return HttpResponse.badRequest(bindingResult);
        }

        authService.register(registerDTO);

        return HttpResponse.ok("Đăng ký thành công");
    }

    @Operation(summary = "Đăng nhập vào hệ thống", description = "Xác thực người dùng với thông tin đăng nhập")
    @PostMapping("/login")
    public ResponseEntity<?> login(
            @RequestBody @Validated LoginDTO loginDTO,
            BindingResult bindingResult) {
        if (bindingResult.hasErrors()) {
            return HttpResponse.badRequest(bindingResult);
        }

        LoginResponseDTO loginResponseDTO = authService.login(loginDTO);

        return HttpResponse.ok("Đăng nhập thành công", loginResponseDTO);
    }

    @PostMapping("/logout")
    public ResponseEntity<?> logout(@RequestHeader("Authorization") String token) {
        String tokenPrefix = "Bearer ";
        if (token.startsWith(tokenPrefix)) {
            token = token.substring(tokenPrefix.length());
        }
        authService.logout(token);
        return HttpResponse.ok("Đăng xuất thành công");
    }

    @Operation(summary = "Quên mật khẩu", description = "Gửi hướng dẫn đặt lại mật khẩu đến email của người dùng")
    @PostMapping("/forgot-password")
    public ResponseEntity<?> forgotPassword(
            @Parameter(description = "Tên đăng nhập của tài khoản") @RequestBody @Validated ForgotPassworDTO forgotPassworDTO,
            BindingResult bindingResult) {
        if (bindingResult.hasErrors()) {
            return HttpResponse.badRequest(bindingResult);
        }

        authService.forgotPassword(forgotPassworDTO.getUsername());
        return HttpResponse.ok("Hướng dẫn đặt lại mật khẩu đã được gửi đến email của bạn");
    }

    @Operation(summary = "Xác thực OTP", description = "Xác thực mã OTP đã gửi đến email của người dùng")
    @PostMapping("/verify-otp")
    public ResponseEntity<?> verifyOtp(
            @Parameter(description = "Tên đăng nhập của tài khoản") @RequestBody @Validated VerifyOtpDTO verifyOtpDTO,
            BindingResult bindingResult) {
        if (bindingResult.hasErrors()) {
            return HttpResponse.badRequest(bindingResult);
        }

        String username = verifyOtpDTO.getUsername();
        String otp = verifyOtpDTO.getOtp();
        authService.verifyOtp(username, otp);

        return HttpResponse.ok("Xác thực OTP thành công");
    }

    @Operation(summary = "Đặt lại mật khẩu", description = "Đặt lại mật khẩu của người dùng bằng thông tin được cung cấp")
    @PostMapping("/reset-password")
    public ResponseEntity<?> resetPassword(
            @RequestBody @Validated ResetPassworDTO resetPassworDTO,
            BindingResult bindingResult) {
        if (bindingResult.hasErrors()) {
            return HttpResponse.badRequest(bindingResult);
        }

        authService.resetPassword(resetPassworDTO);

        return HttpResponse.ok("Đặt lại mật khẩu thành công");
    }
}
