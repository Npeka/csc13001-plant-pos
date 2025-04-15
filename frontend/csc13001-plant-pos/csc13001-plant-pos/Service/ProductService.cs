using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using csc13001_plant_pos.DTO;
using csc13001_plant_pos.Model;
using csc13001_plant_pos.Utils;
using Windows.Storage;
using Windows.System;

namespace csc13001_plant_pos.Service;

public interface IProductService
{
    Task<ApiResponse<List<Product>>?> GetProductsAsync();
    Task<ApiResponse<Product>?> GetProductByIdAsync(string id);
    Task<bool?> DeleteProductAsync(string id);
    Task<string?> CreateProductAsync(Product product);
    Task<string?> UpdateProductAsync(Product product, StorageFile file);
}

public class ProductService : IProductService
{
    private readonly HttpClient _httpClient;

    public ProductService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ApiResponse<List<Product>>?> GetProductsAsync()
    {
        var response = await _httpClient.GetAsync("products");
        var json = await response.Content.ReadAsStringAsync();
        return JsonUtils.Deserialize<ApiResponse<List<Product>>>(json);
    }

    public async Task<ApiResponse<Product>?> GetProductByIdAsync(string id)
    {
        var response = await _httpClient.GetAsync($"products/{id}");
        var json = await response.Content.ReadAsStringAsync();
        return JsonUtils.Deserialize<ApiResponse<Product>>(json);
    }

    public async Task<bool?> DeleteProductAsync(string id)
    {
        var response = await _httpClient.DeleteAsync($"products/{id}");
        var json = await response.Content.ReadAsStringAsync();
        var jsonDoc = JsonDocument.Parse(json);
        var root = jsonDoc.RootElement;
        if (root.GetProperty("status").GetString() == "success")
        {
            return true;
        }
        return false;
    }

    public async Task<string?> CreateProductAsync(Product product)
    {
        var content = JsonUtils.ToJsonContent(product);
        var response = await _httpClient.PostAsync("products", content);
        var json = await response.Content.ReadAsStringAsync();
        var jsonDoc = JsonDocument.Parse(json);
        var root = jsonDoc.RootElement;
        if (root.GetProperty("status").GetString() == "success")
        {
            var productId = root.GetProperty("data").GetProperty("productId").GetInt32().ToString();
            return productId;
        }
        return null;
    }

    public async Task<string?> UpdateProductAsync(Product product, StorageFile file)
    {
        var content = new MultipartFormDataContent();
        var jsonContent = JsonUtils.ToJson(product);

        content.Add(new StringContent(jsonContent), "product");
        Debug.WriteLine(jsonContent);
        if (file != null)
        {
            var stream = await file.OpenStreamForReadAsync();
            var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            content.Add(fileContent, "image", file.Name);
        }
        var response = await _httpClient.PutAsync($"products/{product.ProductId.ToString()}", content);
        var json = await response.Content.ReadAsStringAsync();
        var jsonDoc = JsonDocument.Parse(json);
        var root = jsonDoc.RootElement;
        if (root.GetProperty("status").GetString() == "success")
        {
            var productId = root.GetProperty("data").GetProperty("productId").GetInt32().ToString();
            return productId;
        }
        return null;
    }
}
