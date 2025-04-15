using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using csc13001_plant_pos.DTO;
using csc13001_plant_pos.DTO.CustomerDTO;
using csc13001_plant_pos.Model;
using csc13001_plant_pos.Utils;

namespace csc13001_plant_pos.Service;

public interface ICategoryService
{
    Task<ApiResponse<List<Category>>?> GetCategoriesAsync();
    Task<ApiResponse<Category>?> GetCategoryByIdAsync(string id);
    Task<bool?> DeleteCategoryAsync(string id);
    Task<string?> CreateCategoryAsync(CategoryCreateDto category);
    Task<string?> UpdateCategoryAsync(CategoryCreateDto category);
}

public class CategoryService : ICategoryService
{
    private readonly HttpClient _httpClient;

    public CategoryService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ApiResponse<List<Category>>?> GetCategoriesAsync()
    {
        var response = await _httpClient.GetAsync("categories");
        var json = await response.Content.ReadAsStringAsync();
        return JsonUtils.Deserialize<ApiResponse<List<Category>>>(json);
    }

    public async Task<ApiResponse<Category>?> GetCategoryByIdAsync(string id)
    {
        var response = await _httpClient.GetAsync($"categories/{id}");
        var json = await response.Content.ReadAsStringAsync();
        return JsonUtils.Deserialize<ApiResponse<Category>>(json);
    }

    public async Task<bool?> DeleteCategoryAsync(string id)
    {
        var response = await _httpClient.DeleteAsync($"categories/{id}");
        var json = await response.Content.ReadAsStringAsync();
        var jsonDoc = JsonDocument.Parse(json);
        var root = jsonDoc.RootElement;
        if (root.GetProperty("status").GetString() == "success")
        {
            return true;
        }
        return false;
    }

    public async Task<string?> CreateCategoryAsync(CategoryCreateDto category)
    {
        var content = JsonUtils.ToJsonContent(category);
        Debug.WriteLine(content);
        var response = await _httpClient.PostAsync("categories", content);
        var json = await response.Content.ReadAsStringAsync();
        var jsonDoc = JsonDocument.Parse(json);
        var root = jsonDoc.RootElement;
        if (root.GetProperty("status").GetString() == "success")
        {
            var categoryId = root.GetProperty("data").GetProperty("categoryId").GetInt32().ToString();
            return categoryId;
        }
        return null;
    }

    public async Task<string?> UpdateCategoryAsync( CategoryCreateDto category)
    {
        var content = JsonUtils.ToJsonContent(category);
        string json11 = await content.ReadAsStringAsync();
        Debug.WriteLine(json11);
        
        var response = await _httpClient.PutAsync($"categories/{category.CategoryId.ToString()}", content);
        var json = await response.Content.ReadAsStringAsync();
        var jsonDoc = JsonDocument.Parse(json);
        var root = jsonDoc.RootElement;
        if (root.GetProperty("status").GetString() == "success")
        {
            return category.CategoryId.ToString();
        }
        return null;
    }
}