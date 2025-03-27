using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using csc13001_plant_pos.DTO;
using csc13001_plant_pos.DTO.OrderDTO;
using csc13001_plant_pos.DTO.StaffDTO;
using csc13001_plant_pos.Model;

namespace csc13001_plant_pos.ViewModel
{
    public class StaffProfilePageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public StaffUser StaffUser { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public ObservableCollection<OrderListDto> StaffOrders { get; set; } = new ObservableCollection<OrderListDto>();

        private readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:8080/") };

        public StaffProfilePageViewModel()
        {
            LoadStaffDataAsync();
            LoadStaffOrdersAsync();
        }

        private async void LoadStaffDataAsync()
        {
            try
            {
                string json = await _httpClient.GetStringAsync("api/staff/2");
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<StaffUserDto>>(json);
                System.Diagnostics.Debug.WriteLine($"Status: {apiResponse?.Status}, Message: {apiResponse?.Message}");

                if (apiResponse?.Data != null)
                {
                    StaffUser = apiResponse.Data.User;
                    TotalOrders = apiResponse.Data.TotalOrders;
                    TotalRevenue = apiResponse.Data.TotalRevenue;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private async void LoadStaffOrdersAsync()
        {
            try
            {
                string json = await _httpClient.GetStringAsync("api/orders/staff/2");

                var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<OrderListDto>>>(json);

                System.Diagnostics.Debug.WriteLine($"Status: {apiResponse?.Status}, Message: {apiResponse?.Message}");
                if (apiResponse?.Data != null)
                {
                    StaffOrders.Clear();
                    foreach (var order in apiResponse.Data)
                    {
                        StaffOrders.Add(order);
                    }
                }
            }
            catch (JsonException jex)
            {
                Console.WriteLine($"Deserialize Error: {jex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}