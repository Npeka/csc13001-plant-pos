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
    Task<string?> CreateProductAsync(Product product, StorageFile file);
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

    public async Task<string?> CreateProductAsync(Product product, StorageFile file)
    {
        var jsonContent = JsonSerializer.Serialize(new
        {
            productId = product.ProductId,
            name = product.Name,
            description = product.Description,
            imageUrl = product.ImageUrl,
            salePrice = product.SalePrice,
            purchasePrice = product.PurchasePrice,
            stock = product.Stock,
            size = product.Size,
            careLevel = product.CareLevel,
            lightRequirement = product.LightRequirement,
            wateringSchedule = product.WateringSchedule,
            environmentType = product.EnvironmentType,
            category = new
            {
                categoryId = product.Category.CategoryId,
                name = product.Category.Name,
                description = product.Category.Description
            }
        });

        var content = new MultipartFormDataContent();
        content.Add(new StringContent(jsonContent), "product");
        Debug.WriteLine(jsonContent);
        if (file != null)
        {
            var stream = await file.OpenStreamForReadAsync();
            var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            content.Add(fileContent, "image", file.Name);
        }
        var response = await _httpClient.PostAsync("products", content);
        var json = await response.Content.ReadAsStringAsync();
        var jsonDoc = JsonDocument.Parse(json);
        var root = jsonDoc.RootElement;
        Debug.WriteLine(root.GetProperty("message").GetString());
        return root.GetProperty("message").GetString();
    }

    public async Task<string?> UpdateProductAsync(Product product, StorageFile file)
    {
        var jsonContent = JsonSerializer.Serialize(new
        {
            productId = product.ProductId,
            name = product.Name,
            description = product.Description,
            imageUrl = product.ImageUrl,
            salePrice = product.SalePrice,
            purchasePrice = product.PurchasePrice,
            stock = product.Stock,
            size = product.Size,
            careLevel = product.CareLevel,
            lightRequirement = product.LightRequirement,
            wateringSchedule = product.WateringSchedule,
            environmentType = product.EnvironmentType,
            category = new
            {
                categoryId = product.Category.CategoryId,
                name = product.Category.Name,
                description = product.Category.Description
            }
        });

        var content = new MultipartFormDataContent();
        content.Add(new StringContent(jsonContent), "product");
        if (file != null)
        {
            var stream = await file.OpenStreamForReadAsync();
            var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            content.Add(fileContent, "image", file.Name);
        }
        var response = await _httpClient.PutAsync($"products/{product.ProductId.ToString()}", content);
        Debug.WriteLine($"Response: {response.StatusCode}");
        var json = await response.Content.ReadAsStringAsync();
        var jsonDoc = JsonDocument.Parse(json);
        var root = jsonDoc.RootElement;
        return root.GetProperty("message").GetString();
    }
}
