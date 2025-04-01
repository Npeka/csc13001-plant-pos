using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using csc13001_plant_pos.DTO;
using csc13001_plant_pos.DTO.OrderDTO;
using csc13001_plant_pos.Utils;

namespace csc13001_plant_pos.Service;

public interface IOrderService
{
    Task<ApiResponse<OrderListDto>?> GetOrderByIdAsync(string orderId);
    Task<string?> CreateOrderAsync(OrderCreateDto orderRequest);
    Task<ApiResponse<List<OrderListDto>>?> GetAllOrdersAsync();
    Task<bool> DeleteOrderAsync(string orderId);
}

public class OrderService : IOrderService
{
    private readonly HttpClient _httpClient;

    public OrderService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ApiResponse<OrderListDto>?> GetOrderByIdAsync(string orderId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"orders/{orderId}");
            var json = await response.Content.ReadAsStringAsync();
            return JsonUtils.Deserialize<ApiResponse<OrderListDto>>(json);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error retrieving order: {ex.Message}");
            return null;
        }
    }

    public async Task<string?> CreateOrderAsync(OrderCreateDto orderRequest)
    {
        var content = JsonUtils.ToJsonContent(orderRequest);
        var response = await _httpClient.PostAsync("orders", content);
        var json = await response.Content.ReadAsStringAsync();

        var jsonDoc = JsonDocument.Parse(json);
        var root = jsonDoc.RootElement;
        if (root.GetProperty("status").GetString() == "success")
        {
            var orderId = root.GetProperty("data").GetProperty("orderId").GetInt32().ToString();
            return orderId;
        }
        return null;
    }

    public async Task<ApiResponse<List<OrderListDto>>?> GetAllOrdersAsync()
    {
        var response = await _httpClient.GetAsync("orders");
        var json = await response.Content.ReadAsStringAsync();
        return JsonUtils.Deserialize<ApiResponse<List<OrderListDto>>>(json);
    }

    public async Task<bool> DeleteOrderAsync(string orderId)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"orders/{orderId}");
            var json = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonUtils.Deserialize<ApiResponse<object>>(json);
            return apiResponse?.Status == "success";
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error deleting order {orderId}: {ex.Message}");
            return false;
        }
    }
}