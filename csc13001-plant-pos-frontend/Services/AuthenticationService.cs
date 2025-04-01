using System.Net.Http;
using System.Threading.Tasks;
using csc13001_plant_pos_frontend.Core.Contracts.Services;
using csc13001_plant_pos_frontend.Core.DTOs.Auth;
using csc13001_plant_pos_frontend.Core.Models;
using csc13001_plant_pos_frontend.Utils;

namespace csc13001_plant_pos_frontend.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly HttpClient _httpClient;

    public AuthenticationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<HttpResponse<object>?> RegisterAsync(string username, string password)
    {
        var payload = new { username = username, password = password };
        var content = JsonUtils.ToJsonContent(payload);

        var response = await _httpClient.PostAsync("auth/register", content);
        var json = await response.Content.ReadAsStringAsync();
        return JsonUtils.Deserialize<HttpResponse<object>>(json);
    }

    public async Task<HttpResponse<LoginResponseDTO>?> LoginAsync(string username, string password)
    {
        var payload = new { username = username, password = password };
        var content = JsonUtils.ToJsonContent(payload);

        var response = await _httpClient.PostAsync("auth/login", content);
        var json = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonUtils.Deserialize<HttpResponse<LoginResponseDTO>>(json);
        return apiResponse;
    }

    public async Task<HttpResponse<object>?> ForgotPasswordAsync(string username)
    {
        var payload = new { username = username };
        var content = JsonUtils.ToJsonContent(payload);

        var response = await _httpClient.PostAsync("auth/forgot-password", content);
        var json = await response.Content.ReadAsStringAsync();
        return JsonUtils.Deserialize<HttpResponse<object>>(json);
    }

    public async Task<HttpResponse<object>?> VerifyOTPAsync(string username, string otp)
    {
        var payload = new { username = username, otp = otp };
        var content = JsonUtils.ToJsonContent(payload);

        var response = await _httpClient.PostAsync("auth/verify-otp", content);
        var json = await response.Content.ReadAsStringAsync();
        return JsonUtils.Deserialize<HttpResponse<object>>(json);
    }

    public async Task<HttpResponse<object>?> ResetPasswordAsync(string username, string newPassword, string confirmPassword)
    {
        if (newPassword != confirmPassword)
        {
            return new HttpResponse<object>
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
        return JsonUtils.Deserialize<HttpResponse<object>>(json);
    }
}
