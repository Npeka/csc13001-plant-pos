using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using csc13001_plant_pos.DTO;
using csc13001_plant_pos.DTO.NotificationDTO;
using csc13001_plant_pos.Utils;

namespace csc13001_plant_pos.Service;

public interface INotificationService
{
    Task<ApiResponse<List<NotificationDto>>?> GetNotificationsAsync(string staffId);
    Task<bool> MarkNotificationAsReadAsync(int notificationUserId);
}

public class NotificationService : INotificationService
{
    private readonly HttpClient _httpClient;

    public NotificationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ApiResponse<List<NotificationDto>>?> GetNotificationsAsync(string staffId)
    {
        var response = await _httpClient.GetAsync($"notifications/{staffId}");
        var json = await response.Content.ReadAsStringAsync();
        return JsonUtils.Deserialize<ApiResponse<List<NotificationDto>>>(json);
    }

    public async Task<bool> MarkNotificationAsReadAsync(int notificationUserId)
    {
        var response = await _httpClient.PatchAsync($"notifications/{notificationUserId}/read", null);
        var json = await response.Content.ReadAsStringAsync();
        var jsonDoc = JsonDocument.Parse(json);
        var root = jsonDoc.RootElement;

        return root.GetProperty("status").GetString() == "success";
    }
}