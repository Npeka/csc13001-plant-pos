using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using csc13001_plant_pos.DTO;
using csc13001_plant_pos.DTO.OrderDTO;
using csc13001_plant_pos.Model;

namespace csc13001_plant_pos.ViewModel
{
    public class CustomerProfilePageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Customer Customer { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalSpending { get; set; }
        public ObservableCollection<OrderListDto> CustomerOrders { get; set; } = new ObservableCollection<OrderListDto>();

        private readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:8080/") };

        public CustomerProfilePageViewModel()
        {
            LoadCustomerDataAsync();
            LoadCustomerOrdersAsync();
        }

        private async void LoadCustomerDataAsync()
        {
            try
            {
                string json = await _httpClient.GetStringAsync("api/customers/1");
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<Customer>>(json);
                System.Diagnostics.Debug.WriteLine($"Status: {apiResponse?.Status}, Message: {apiResponse?.Message}");

                if (apiResponse?.Data != null)
                {
                    Customer = apiResponse.Data;
                    TotalOrders = 0; // Có thể cập nhật từ API orders nếu cần
                    TotalSpending = 0; // Có thể cập nhật từ API orders nếu cần
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading customer: {ex.Message}");
            }
        }

        private async void LoadCustomerOrdersAsync()
        {
            try
            {
                string json = await _httpClient.GetStringAsync("api/orders/customer/1");
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<OrderListDto>>>(json);
                System.Diagnostics.Debug.WriteLine($"Status: {apiResponse?.Status}, Message: {apiResponse?.Message}");

                if (apiResponse?.Data != null)
                {
                    CustomerOrders.Clear();
                    foreach (var order in apiResponse.Data)
                    {
                        CustomerOrders.Add(order);
                    }
                    TotalOrders = CustomerOrders.Count;
                    TotalSpending = CustomerOrders.Sum(o => o.FinalPrice);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading customer orders: {ex.Message}");
            }
        }
    }
}