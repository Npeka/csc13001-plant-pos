using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using csc13001_plant_pos.DTO;
using csc13001_plant_pos.DTO.InventoryDTO;

namespace csc13001_plant_pos.ViewModel
{
    public class WarehouseManagementViewModel
    {

        public ObservableCollection<InventoryListDto> InventoryOrders { get; set; } = new ObservableCollection<InventoryListDto>();

        private readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:8080/") };

        public WarehouseManagementViewModel()
        {
            LoadInventoryOrdersAsync();
        }

        private async void LoadInventoryOrdersAsync()
        {
            try
            {
                string json = await _httpClient.GetStringAsync("api/inventories");
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<InventoryListDto>>>(json);
                System.Diagnostics.Debug.WriteLine($"Status: {apiResponse?.Status}, Message: {apiResponse?.Message}");

                if (apiResponse?.Data != null)
                {
                    InventoryOrders.Clear();
                    foreach (var order in apiResponse.Data)
                    {
                        InventoryOrders.Add(order);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading inventory orders: {ex.Message}");
            }
        }
    }
}