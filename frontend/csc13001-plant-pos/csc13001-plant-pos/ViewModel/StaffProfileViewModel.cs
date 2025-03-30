using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using csc13001_plant_pos.DTO;
using csc13001_plant_pos.DTO.OrderDTO;
using csc13001_plant_pos.DTO.StaffDTO;
using csc13001_plant_pos.Model;
using csc13001_plant_pos.Service;

public partial class StaffProfileViewModel : ObservableObject
{
    private readonly UserSessionService _userSessionService;
    private readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:8080/") };

    [ObservableProperty]
    private User staffUser;

    [ObservableProperty]
    private int totalOrders;

    [ObservableProperty]
    private decimal totalRevenue;

    public ObservableCollection<OrderListDto> StaffOrders { get; } = new();

    public StaffProfileViewModel(UserSessionService userSessionService)
    {
        _userSessionService = userSessionService;
        LoadStaffDataAsync();
        LoadStaffOrdersAsync();
    }

    private async void LoadStaffDataAsync()
    {
        try
        {
            var userId = _userSessionService.CurrentUser?.UserId ?? 0;
            if (userId == 0) return;

            string json = await _httpClient.GetStringAsync($"api/staff/{userId}");
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
            var userId = _userSessionService.CurrentUser?.UserId ?? 0;
            if (userId == 0) return;

            string json = await _httpClient.GetStringAsync($"api/orders/staff/{userId}");
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
