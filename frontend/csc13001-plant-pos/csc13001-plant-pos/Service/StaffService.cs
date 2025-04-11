using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using csc13001_plant_pos.DTO;
using csc13001_plant_pos.DTO.OrderDTO;
using csc13001_plant_pos.DTO.StaffDTO;
using csc13001_plant_pos.Model;
using csc13001_plant_pos.Utils;

namespace csc13001_plant_pos.Service;

public interface IStaffService
{
    Task<ApiResponse<StaffUserDto>?> GetStaffByIdAsync(int staffId);
    Task<ApiResponse<List<OrderListDto>>?> GetStaffOrdersAsync(int staffId);

    Task<ApiResponse<List<User>>?> GetListStaffAsync();

    Task<bool> UpdateStaffAsync(User user);

    Task<bool> AddStaffAsync(User user);
}

public class StaffService : IStaffService
{
    private readonly HttpClient _httpClient;

    public StaffService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ApiResponse<StaffUserDto>?> GetStaffByIdAsync(int staffId)
    {
        var response = await _httpClient.GetAsync($"staff/{staffId}");
        var json = await response.Content.ReadAsStringAsync();
        return JsonUtils.Deserialize<ApiResponse<StaffUserDto>>(json);
    }

    public async Task<ApiResponse<List<OrderListDto>>?> GetStaffOrdersAsync(int staffId)
    {
        var response = await _httpClient.GetAsync($"orders/staff/{staffId}");
        var json = await response.Content.ReadAsStringAsync();
        return JsonUtils.Deserialize<ApiResponse<List<OrderListDto>>>(json);
    }

    public async Task<ApiResponse<List<User>>?> GetListStaffAsync()
    {
        var response = await _httpClient.GetAsync("staff");
        var json = await response.Content.ReadAsStringAsync();
        return JsonUtils.Deserialize<ApiResponse<List<User>>>(json);
    }

    public async Task<bool> UpdateStaffAsync(User user)
    {
        var content = JsonUtils.ToJsonContent(user);
        var response = await _httpClient.PutAsync($"staff/{user.UserId}", content);
        var responseJson = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonUtils.Deserialize<ApiResponse<object>>(responseJson);
        return apiResponse?.Status == "success";
    }

    public async Task<bool> AddStaffAsync(User user)
    {
        var content = JsonUtils.ToJsonContent(user);
        var response = await _httpClient.PostAsync("staff", content);
        var responseJson = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonUtils.Deserialize<ApiResponse<object>>(responseJson);
        return apiResponse?.Status == "success";
    }
}