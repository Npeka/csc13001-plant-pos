using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using csc13001_plant_pos.DTO;
using csc13001_plant_pos.DTO.AuthDTO;
using csc13001_plant_pos.Utils;

namespace csc13001_plant_pos.Service;

public interface IAuthenticationService
{
    Task<ApiResponse<object>?> RegisterAsync(string username, string password);
    Task<ApiResponse<LoginResponseDTO>?> LoginAsync(string username, string password);
    Task<ApiResponse<object>?> ForgotPasswordAsync(string username);
    Task<ApiResponse<object>?> VerifyOTPAsync(string username, string otp);
    Task<ApiResponse<object>?> ResetPasswordAsync(string username, string newPassword, string confirmPassword);
}

public class AuthenticationService
{
    private readonly HttpClient _httpClient;

    public AuthenticationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ApiResponse<object>?> RegisterAsync(string username, string password)
    {
        var payload = new { username = username, password = password };
        var content = JsonUtils.ToJsonContent(payload);

        var response = await _httpClient.PostAsync("auth/register", content);
        var json = await response.Content.ReadAsStringAsync();
        return JsonUtils.Deserialize<ApiResponse<object>>(json);
    }

    public async Task<ApiResponse<LoginResponseDTO>?> LoginAsync(string username, string password)
    {
        var payload = new { username = username, password = password };
        var content = JsonUtils.ToJsonContent(payload);

        var response = await _httpClient.PostAsync("auth/login", content);
        var json = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonUtils.Deserialize<ApiResponse<LoginResponseDTO>>(json);
        return apiResponse;
    }

    public async Task<ApiResponse<object>?> ForgotPasswordAsync(string username)
    {
        var payload = new { username = username };
        var content = JsonUtils.ToJsonContent(payload);

        var response = await _httpClient.PostAsync("auth/forgot-password", content);
        var json = await response.Content.ReadAsStringAsync();
        return JsonUtils.Deserialize<ApiResponse<object>>(json);
    }

    public async Task<ApiResponse<object>?> VerifyOTPAsync(string username, string otp)
    {
        var payload = new { username = username, otp = otp };
        var content = JsonUtils.ToJsonContent(payload);

        var response = await _httpClient.PostAsync("auth/verify-otp", content);
        var json = await response.Content.ReadAsStringAsync();
        return JsonUtils.Deserialize<ApiResponse<object>>(json);
    }

    public async Task<ApiResponse<object>?> ResetPasswordAsync(string username, string newPassword, string confirmPassword)
    {
        if (newPassword != confirmPassword)
        {
            return new ApiResponse<object>
            {
                Status = "error",
                Message = "Passwords do not match",
                Data = null
            };
        }

        var payload = new { Username = username, NewPassword = newPassword };
        var content = JsonUtils.ToJsonContent(payload);

        var response = await _httpClient.PostAsync("auth/reset-password", content);
        var json = await response.Content.ReadAsStringAsync();
        return JsonUtils.Deserialize<ApiResponse<object>>(json);
    }
}
