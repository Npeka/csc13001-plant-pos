using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using csc13001_plant_pos.Model;
using csc13001_plant_pos.DTO;
using System.Collections.Generic;

namespace csc13001_plant_pos.ViewModel
{
    public partial class DiscountManagementPageViewModel : ObservableRecipient
    {
        public ObservableCollection<DiscountProgram> Discounts { get; set; } = new ObservableCollection<DiscountProgram>();

        private readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:8080/") };

        public DiscountManagementPageViewModel()
        {
            LoadDiscountsAsync();
        }

        private async void LoadDiscountsAsync()
        {
            try
            {
                string json = await _httpClient.GetStringAsync("api/discounts");
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<DiscountProgram>>>(json);
                System.Diagnostics.Debug.WriteLine($"Status: {apiResponse?.Status}, Message: {apiResponse?.Message}");

                if (apiResponse?.Data != null)
                {
                    Discounts.Clear();
                    foreach (var discount in apiResponse.Data)
                    {
                        Discounts.Add(discount);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading discounts: {ex.Message}");
            }
        }
    }
}