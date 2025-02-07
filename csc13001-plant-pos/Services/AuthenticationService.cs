using Microsoft.EntityFrameworkCore;

using csc13001_plant_pos.Utils;
using csc13001_plant_pos.Data.Contexts;
using csc13001_plant_pos.Core.Models;
using csc13001_plant_pos.Core.Contracts.Services;
using static System.Net.WebRequestMethods;

namespace csc13001_plant_pos.Services;
public class AuthenticationService : IAuthenticationService
{
    private readonly RedisService _redisService;
    private readonly ApplicationDbContext _context;

    public AuthenticationService(ApplicationDbContext context, RedisService redisService)
    {
        _context = context;
        _redisService = redisService;
    }

    public async Task<bool> RegisterAsync(string username, string password)
    {
        if (await _context.Users.AnyAsync(u => u.Username == username))
        {
            return false;
        }

        var salt = BCrypt.Net.BCrypt.GenerateSalt(12);
        var user = new User
        {
            Username = username,
            Password = BCrypt.Net.BCrypt.HashPassword(password, salt),
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<User?> LoginAsync(string username, string password)
    {
        //await RegisterAsync(username, password);
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            return user;
        }
        return null;
    }

    public async Task<bool> ForgotPassword(string username)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user != null)
        {
            var otp = RandomUtils.RamdomSixDigitOTP();
            var notiService = App.GetService<INotificationService>();
            await notiService.SendOTP(user.Email, otp.ToString());
            await _redisService.SetValueAsync($"OTP:{user.Username}", otp.ToString(), 5);
            return true;
        }
        return false;
    }

    public async Task<bool> VerifyOTP(string username, string otp)
    {
        var storedOtp = await _redisService.GetValueAsync($"OTP:{username}");
        if (string.IsNullOrEmpty(storedOtp) || storedOtp != otp)
        {
            return false;
        }

        await _redisService.DeleteKeyAsync($"OTP:{username}");
        return true;
    }

    public async Task<bool> ResetPassword(string username, string newPassword, string confirmPassword)
    {
        if (string.IsNullOrWhiteSpace(newPassword) ||
            string.IsNullOrWhiteSpace(confirmPassword) ||
            newPassword != confirmPassword)
        {
            return false;
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user != null)
        {
            var salt = BCrypt.Net.BCrypt.GenerateSalt(12);
            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword, salt);
            await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }
}
