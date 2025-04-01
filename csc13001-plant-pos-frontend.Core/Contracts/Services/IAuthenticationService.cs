using csc13001_plant_pos_frontend.Core.DTOs.Auth;
using csc13001_plant_pos_frontend.Core.Models;

namespace csc13001_plant_pos_frontend.Core.Contracts.Services;

public interface IAuthenticationService
{
    Task<HttpResponse<object>?> RegisterAsync(string username, string password);
    Task<HttpResponse<LoginResponseDTO>?> LoginAsync(string username, string password);
    Task<HttpResponse<object>?> ForgotPasswordAsync(string username);
    Task<HttpResponse<object>?> VerifyOTPAsync(string username, string otp);
    Task<HttpResponse<object>?> ResetPasswordAsync(string username, string newPassword, string confirmPassword);
}
