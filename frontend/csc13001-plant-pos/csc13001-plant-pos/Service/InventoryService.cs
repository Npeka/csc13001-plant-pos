using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using csc13001_plant_pos.DTO;
using csc13001_plant_pos.DTO.InventoryDTO;
using csc13001_plant_pos.Utils;

namespace csc13001_plant_pos.Service;

public interface IInventoryService
{
    Task<ApiResponse<List<InventoryListDto>>?> GetAllInventoriesAsync();
    Task<bool> CreateInventoryAsync(InventoryCreateDto inventory);
}

public class InventoryService : IInventoryService
{
    private readonly HttpClient _httpClient;

    public InventoryService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ApiResponse<List<InventoryListDto>>?> GetAllInventoriesAsync()
    {
        var response = await _httpClient.GetAsync("inventories");
        var json = await response.Content.ReadAsStringAsync();
        return JsonUtils.Deserialize<ApiResponse<List<InventoryListDto>>>(json);
    }

    public async Task<bool> CreateInventoryAsync(InventoryCreateDto inventory)
    {
        try
        {
            var content = JsonUtils.ToJsonContent(inventory);
            var response = await _httpClient.PostAsync("inventories", content);
            var json = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonUtils.Deserialize<ApiResponse<object>>(json);
            System.Diagnostics.Debug.WriteLine($"Create inventory response: {json}");
            return apiResponse?.Status == "success";
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error creating inventory: {ex.Message}");
            return false;
        }
    }
}