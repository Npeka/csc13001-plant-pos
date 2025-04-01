using csc13001_plant_pos.Core.Models;

namespace csc13001_plant_pos.Core.Contracts.Services;
public interface IAuthenticationService
{
    Task<bool> RegisterAsync(string username, string password);
    Task<User?> LoginAsync(string username, string password);
    Task<bool> ForgotPassword(string username);

    Task<bool> VerifyOTP(string username, string otp);
    Task<bool> ResetPassword(string username, string newPassword, string confirmPassword);
}
