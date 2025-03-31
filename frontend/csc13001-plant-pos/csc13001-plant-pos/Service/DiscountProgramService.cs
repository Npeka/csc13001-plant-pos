using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using csc13001_plant_pos.DTO;
using csc13001_plant_pos.Model;
using csc13001_plant_pos.Utils;

namespace csc13001_plant_pos.Service;

public interface IDiscountProgramService
{
    Task<ApiResponse<List<DiscountProgram>>?> GetDiscountsByPhoneAsync(string phoneNumber);
    Task<ApiResponse<List<DiscountProgram>>?> GetAllDiscountsAsync();
    Task<bool> CreateDiscountAsync(DiscountProgram discount);
    Task<bool> UpdateDiscountAsync(int id, DiscountProgram discount);
}

public class DiscountProgramService : IDiscountProgramService
{
    private readonly HttpClient _httpClient;

    public DiscountProgramService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ApiResponse<List<DiscountProgram>>?> GetDiscountsByPhoneAsync(string phoneNumber)
    {
        var response = await _httpClient.GetAsync($"discounts/customer/{phoneNumber}");
        var json = await response.Content.ReadAsStringAsync();
        return JsonUtils.Deserialize<ApiResponse<List<DiscountProgram>>>(json);
    }

    public async Task<ApiResponse<List<DiscountProgram>>?> GetAllDiscountsAsync()
    {
        var response = await _httpClient.GetAsync("discounts");
        var json = await response.Content.ReadAsStringAsync();
        return JsonUtils.Deserialize<ApiResponse<List<DiscountProgram>>>(json);
    }

    public async Task<bool> CreateDiscountAsync(DiscountProgram discount)
    {
        try
        {
            var content = JsonUtils.ToJsonContent(discount);
            var response = await _httpClient.PostAsync("discounts", content);
            var json = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonUtils.Deserialize<ApiResponse<object>>(json);
            return apiResponse?.Status == "success";
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error creating discount: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> UpdateDiscountAsync(int id, DiscountProgram discount)
    {
        try
        {
            var content = JsonUtils.ToJsonContent(discount);
            var response = await _httpClient.PutAsync($"discounts/{id}", content);
            var json = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonUtils.Deserialize<ApiResponse<object>>(json);
            return apiResponse?.Status == "success";
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error updating discount {id}: {ex.Message}");
            return false;
        }
    }
}