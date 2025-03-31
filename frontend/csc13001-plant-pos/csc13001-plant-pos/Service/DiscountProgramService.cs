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
}