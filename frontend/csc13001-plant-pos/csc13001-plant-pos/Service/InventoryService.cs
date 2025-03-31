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
}