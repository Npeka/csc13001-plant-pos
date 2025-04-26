using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using csc13001_plant_pos.DTO;
using csc13001_plant_pos.DTO.CustomerDTO;
using csc13001_plant_pos.DTO.OrderDTO;
using csc13001_plant_pos.Model;
using csc13001_plant_pos.Utils;

namespace csc13001_plant_pos.Service;

public interface ICustomerService
{
    Task<string?> AddCustomerAsync(CustomerCreateDto customerDto);
    Task<ApiResponse<CustomerDto>?> GetCustomerByIdAsync(string customerId);
    Task<ApiResponse<List<OrderListDto>>?> GetCustomerOrdersAsync(string customerId);

    Task<ApiResponse<List<CustomerDto>>?> GetListCustomersAsync();

    Task<string> DeleteCustomerAsync(string customerId);

    Task<string?> UpdateCustomerAsync(Customer customerDto);
}

public class CustomerService : ICustomerService
{
    private readonly HttpClient _httpClient;

    public CustomerService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string?> AddCustomerAsync(CustomerCreateDto customerDto)
    {
        var content = JsonUtils.ToJsonContent(customerDto);
        var response = await _httpClient.PostAsync("customers", content);
        var json = await response.Content.ReadAsStringAsync();
        var jsonDoc = JsonDocument.Parse(json);
        var root = jsonDoc.RootElement;
        if (root.GetProperty("status").GetString() == "success")
        {
            var customerId = root.GetProperty("data").GetProperty("customerId").GetInt32().ToString();
            return customerId;
        }
        return root.GetProperty("message").GetString();
    }
    public async Task<ApiResponse<CustomerDto>?> GetCustomerByIdAsync(string customerId)
    {
        var response = await _httpClient.GetAsync($"customers/{customerId}");
        var json = await response.Content.ReadAsStringAsync();
        return JsonUtils.Deserialize<ApiResponse<CustomerDto>>(json);
    }

    public async Task<ApiResponse<List<OrderListDto>>?> GetCustomerOrdersAsync(string customerId)
    {
        var response = await _httpClient.GetAsync($"orders/customer/{customerId}");
        var json = await response.Content.ReadAsStringAsync();
        return JsonUtils.Deserialize<ApiResponse<List<OrderListDto>>>(json);
    }

    public async Task<ApiResponse<List<CustomerDto>>?> GetListCustomersAsync()
    {
        var response = await _httpClient.GetAsync("customers");
        var json = await response.Content.ReadAsStringAsync();
        return JsonUtils.Deserialize<ApiResponse<List<CustomerDto>>>(json);
    }

    public async Task<string> DeleteCustomerAsync(string customerId)
    {
        var response = await _httpClient.DeleteAsync($"customers/{customerId}");
        var json = await response.Content.ReadAsStringAsync();
        var jsonDoc = JsonDocument.Parse(json);
        var root = jsonDoc.RootElement;
        Debug.WriteLine(root.GetProperty("message").GetString());
        return root.GetProperty("message").GetString();
    }

    public async Task<string?> UpdateCustomerAsync(Customer customerDto)
    {
        var content = JsonUtils.ToJsonContent(customerDto);

        var response = await _httpClient.PutAsync($"customers/{customerDto.CustomerId}", content);
        var json = await response.Content.ReadAsStringAsync();
        var jsonDoc = JsonDocument.Parse(json);
        var root = jsonDoc.RootElement;
        return root.GetProperty("message").GetString();
    }
}