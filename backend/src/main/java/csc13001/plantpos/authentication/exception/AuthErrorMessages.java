package csc13001.plantpos.authentication.exception;

public class AuthErrorMessages {
    public static final String USERNAME_EXISTS = "Tên đăng nhập đã tồn tại.";
    public static final String EMAIL_EXISTS = "Email đã tồn tại.";
    public static final String PHONE_EXISTS = "Số điện thoại đã tồn tại.";
    public static final String USERNAME_EMAIL_PHONE_EXISTS = "Tên đăng nhập, email hoặc số điện thoại đã tồn tại.";
    public static final String USER_NOT_FOUND = "Không tìm thấy người dùng.";
    public static final String INVALID_PASSWORD = "Mật khẩu và xác nhận mật khẩu không khớp.";
    public static final String INVALID_OTP = "Mã OTP không hợp lệ.";
    public static final String OTP_EXPIRED = "Mã OTP đã hết hạn.";
    public static final String INVALID_TOKEN = "Token không hợp lệ.";
}
