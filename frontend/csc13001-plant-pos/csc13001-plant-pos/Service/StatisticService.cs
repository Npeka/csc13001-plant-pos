using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using csc13001_plant_pos.DTO;
using csc13001_plant_pos.DTO.ProductDTO;
using csc13001_plant_pos.DTO.StatisticDTO;
using csc13001_plant_pos.Utils;
using System.Diagnostics;
namespace csc13001_plant_pos.Service;

public interface IStatisticService
{
    Task<ApiResponse<List<ProductDto>>?> GetProductsAsync();

    Task<ApiResponse<StatisticReviewDto?>> GetListReviewAsync();

    Task<ApiResponse<StatisticDto?>> GetStatisticAsync(StatisticQueryDto statisticQuery);
}

public class StatisticService : IStatisticService
{
    private readonly HttpClient _httpClient;

    public StatisticService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ApiResponse<List<ProductDto>>?> GetProductsAsync()
    {
        var response = await _httpClient.GetAsync("statistics/products");
        var json = await response.Content.ReadAsStringAsync();
        return JsonUtils.Deserialize<ApiResponse<List<ProductDto>>>(json);
    }

    public async Task<ApiResponse<StatisticReviewDto?>> GetListReviewAsync()
    {
        var response = await _httpClient.GetAsync("statistics/products-review");
        var json = await response.Content.ReadAsStringAsync();
        return JsonUtils.Deserialize<ApiResponse<StatisticReviewDto>>(json);
    }

    public async Task<ApiResponse<StatisticDto?>> GetStatisticAsync(StatisticQueryDto statisticQuery)
    {
        var queryString = $"?timeType={statisticQuery.TimeType}&startDate={statisticQuery.StartDate}&endDate={statisticQuery.EndDate}";
        Debug.WriteLine($"statistics/sales{queryString}");
        var response = await _httpClient.GetAsync($"statistics/sales{queryString}");
        var json = await response.Content.ReadAsStringAsync();
        return JsonUtils.Deserialize<ApiResponse<StatisticDto>>(json);
    }

}