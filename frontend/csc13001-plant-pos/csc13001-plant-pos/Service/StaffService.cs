using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using csc13001_plant_pos.DTO;
using csc13001_plant_pos.DTO.CustomerDTO;
using csc13001_plant_pos.DTO.OrderDTO;
using csc13001_plant_pos.DTO.StaffDTO;
using csc13001_plant_pos.Model;
using csc13001_plant_pos.Utils;
using Windows.Storage;

namespace csc13001_plant_pos.Service;

public interface IStaffService
{
    Task<ApiResponse<StaffUserDto>?> GetStaffByIdAsync(int staffId);
    Task<ApiResponse<List<OrderListDto>>?> GetStaffOrdersAsync(int staffId);

    Task<ApiResponse<List<User>>?> GetListStaffAsync();

    Task<bool> UpdateStaffAsync(User user, StorageFile file);

    Task<bool> AddStaffAsync(User user, StorageFile file);
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

    public async Task<bool> UpdateStaffAsync(User user, StorageFile file)
    {
        var content = new MultipartFormDataContent();

        // Serialize toàn bộ user thành JSON
        var json = JsonUtils.ToJson(user);
        var startDateKey = "\"startDate\":\"";
        var startDateIndex = json.IndexOf(startDateKey);

        if (startDateIndex != -1)
        {
            // Tìm vị trí kết thúc của giá trị startDate
            var startDateValueStart = startDateIndex + startDateKey.Length;
            var startDateValueEnd = json.IndexOf("\"", startDateValueStart);

            // Lấy giá trị startDate và chuyển sang định dạng chỉ ngày
            var startDateValue = json.Substring(startDateValueStart, startDateValueEnd - startDateValueStart);
            var formattedDate = DateTime.Parse(startDateValue).ToString("yyyy-MM-dd");

            // Thay thế giá trị startDate trong chuỗi JSON
            json = json.Remove(startDateValueStart, startDateValueEnd - startDateValueStart);
            json = json.Insert(startDateValueStart, formattedDate);
        }
        content.Add(new StringContent(json), "staff");
        if (file != null)
        {
            var stream = await file.OpenStreamForReadAsync();
            var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            content.Add(fileContent, "image", file.Name);
        }

        var response = await _httpClient.PutAsync($"staff/{user.UserId}", content);
        var responseJson = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonUtils.Deserialize<ApiResponse<object>>(responseJson);
        Debug.WriteLine(apiResponse?.Message);
        return apiResponse?.Status == "success";
    }


    public async Task<bool> AddStaffAsync(User user, StorageFile file)
    {
        var content = new MultipartFormDataContent();
        var json = JsonUtils.ToJson(user);
        var startDateKey = "\"startDate\":\"";
        var startDateIndex = json.IndexOf(startDateKey);

        if (startDateIndex != -1)
        {
            // Tìm vị trí kết thúc của giá trị startDate
            var startDateValueStart = startDateIndex + startDateKey.Length;
            var startDateValueEnd = json.IndexOf("\"", startDateValueStart);

            // Lấy giá trị startDate và chuyển sang định dạng chỉ ngày
            var startDateValue = json.Substring(startDateValueStart, startDateValueEnd - startDateValueStart);
            var formattedDate = DateTime.Parse(startDateValue).ToString("yyyy-MM-dd");

            // Thay thế giá trị startDate trong chuỗi JSON
            json = json.Remove(startDateValueStart, startDateValueEnd - startDateValueStart);
            json = json.Insert(startDateValueStart, formattedDate);
        }
        content.Add(new StringContent(json), "staff");
        Debug.WriteLine(json);
        if (file != null)
        {
            var stream = await file.OpenStreamForReadAsync();
            var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            content.Add(fileContent, "image", file.Name);
        }
        var response = await _httpClient.PostAsync("staff", content);
        var responseJson = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonUtils.Deserialize<ApiResponse<object>>(responseJson);
        return apiResponse?.Status == "success";
    }

}