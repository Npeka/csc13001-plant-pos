using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Text.Json;
using csc13001_plant_pos.DTO;
using csc13001_plant_pos.DTO.OrderDTO;

namespace csc13001_plant_pos.ViewModel
{
    public class OrderViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<OrderListDto> Orders { get; set; } = new ObservableCollection<OrderListDto>();

        private readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:8080/") };

        public OrderViewModel()
        {
            LoadOrdersAsync();
        }

        private async void LoadOrdersAsync()
        {
            try
            {
                string json = await _httpClient.GetStringAsync("api/orders");
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<OrderListDto>>>(json);
                System.Diagnostics.Debug.WriteLine($"Status: {apiResponse?.Status}, Message: {apiResponse?.Message}");

                if (apiResponse?.Data != null)
                {
                    Orders.Clear();
                    foreach (var order in apiResponse.Data)
                    {
                        Orders.Add(order);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading orders: {ex.Message}");
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}