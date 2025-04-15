using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using csc13001_plant_pos.Model;
using csc13001_plant_pos.Service;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System.Collections.ObjectModel;
using System.Diagnostics;
using csc13001_plant_pos.DTO.CustomerDTO;

namespace csc13001_plant_pos.ViewModel
{
    public partial class ProductManagementViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Product> productList;

        [ObservableProperty]
        private ObservableCollection<Product> filteredProductList;

        [ObservableProperty]
        private ObservableCollection<string> categoryList;

        [ObservableProperty]
        private ObservableCollection<Category> categories;

        [ObservableProperty]
        private string searchQuery;

        [ObservableProperty]
        private string? statusQuery;

        [ObservableProperty]
        private string? selectedCategory;

        [ObservableProperty]
        private int isAscendingPrice = 0;

        [ObservableProperty]
        private int isAscendingLevel = 0;

        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductManagementViewModel(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            LoadProductsDataAsync();
            _categoryService = categoryService;
        }

        public async void LoadProductsDataAsync()
        {
            var response = await _productService.GetProductsAsync();
            if (response?.Status == "success" && response.Data != null)
            {
                ProductList = new ObservableCollection<Product>(response.Data);
                FilteredProductList = new ObservableCollection<Product>(response.Data);
            }
            var response2 = await _categoryService.GetCategoriesAsync();
            if (response2?.Status == "success" && response2.Data != null)
            {
                Categories = new ObservableCollection<Category>(response2.Data);
                CategoryList = new ObservableCollection<string>();
                foreach (var category in Categories)
                {
                    CategoryList.Add(category.Name);
                }
            }
        }

        public void StockFilter_SelectionChanged(string value)
        {
            //FilterProducts();
        }
        public void CategoryFilter_SelectionChanged(string value)
        {
        }

        public void UpdateCategories_Click(string value)
        {
            // Xử lý cập nhật danh mục
        }

        public void SortByPrice_Click()
        {
            if (ProductList == null || ProductList.Count == 0) return;

            ObservableCollection<Product> sortedList;
            if (IsAscendingPrice == 0)
            {
                sortedList = new ObservableCollection<Product>(FilteredProductList.OrderByDescending(p => p.SalePrice));
                IsAscendingPrice++;
            }
            else
            {
                sortedList = new ObservableCollection<Product>(FilteredProductList.OrderBy(p => p.SalePrice));
                IsAscendingPrice++;
            }
                FilteredProductList.Clear();
            foreach (var product in sortedList)
            {
                FilteredProductList.Add(product);
            }
        }

        public void SortByLevel_Click()
        {
            if (ProductList == null || ProductList.Count == 0) return;

            ObservableCollection<Product> sortedList;
            if (IsAscendingLevel == 0)
            {
                sortedList = new ObservableCollection<Product>(FilteredProductList.OrderByDescending(p => p.CareLevel));
                IsAscendingLevel++;
            }
            else
            {
                sortedList = new ObservableCollection<Product>(FilteredProductList.OrderBy(p => p.CareLevel));
                IsAscendingLevel++;
            }
            FilteredProductList.Clear();
            foreach (var product in sortedList)
            {
                FilteredProductList.Add(product);
            }
        }

        public void ApplyFilter()
        {
            FilteredProductList.Clear();
            var filtered = ProductList.AsEnumerable();
            if (!string.IsNullOrEmpty(SearchQuery))
            {
                filtered = filtered.Where(emp =>
                emp.Name.ToLower().Contains(SearchQuery));
            }
            if (!string.IsNullOrEmpty(SelectedCategory))
            {
                filtered = filtered.Where(emp => emp.Category.Name.Equals(SelectedCategory));
            }
            if (!string.IsNullOrEmpty(StatusQuery) && StatusQuery != "All")
            {
                if (StatusQuery == "0")
                {
                    filtered = filtered.Where(emp => emp.Stock == 0);
                }
                else if (StatusQuery == "1")
                {
                    filtered = filtered.Where(emp => emp.Stock > 0);
                }
            }
            foreach (var product in filtered)
            {
                FilteredProductList.Add(product);
            }
        }

        public void ResetFilter()
        {
            FilteredProductList.Clear();
            foreach (var product in ProductList)
            {
                FilteredProductList.Add(product);
            }
            SearchQuery = "";
            SelectedCategory = null;
            IsAscendingLevel = 0;
            IsAscendingPrice = 0;
            StatusQuery = null;
        }

        public async Task<string?> AddCategoryAsync(Category data)
        {
            var CategoryCreateDto = new CategoryCreateDto
            {
                Name = data.Name,
                Description = data.Description
            };
            var response = await _categoryService.CreateCategoryAsync(CategoryCreateDto);
            if (response != null && int.TryParse(response, out int categoryId))
            {
                data.CategoryId = categoryId;
                Categories.Add(data);
                CategoryList.Add(data.Name);
                return "Tạo danh mục mới thành công";
            }
            return response;
        }

        public async Task<string?> DeleteCategoryAsync(Category data)
        {
            var response = await _categoryService.DeleteCategoryAsync(data.CategoryId.ToString());
            if (response != null)
            {
                var categoryToRemove = Categories.FirstOrDefault(c => c.CategoryId == data.CategoryId);
                if (categoryToRemove != null)
                {
                    Categories.Remove(categoryToRemove);
                }
                CategoryList.Remove(data.Name);
                return "Xóa danh mục thành công";
            }
            return "Không xóa danh mục thành công";
        }

        public async Task<string?> UpdateCategoryAsync(Category data)
        {
            var CategoryDto = new CategoryDto
            {
                CategoryId = data.CategoryId,
                Name = data.Name,
                Description = data.Description
            };
            var response = await _categoryService.UpdateCategoryAsync(CategoryDto);
            if (response != null)
            {
                LoadProductsDataAsync();
                var index = Categories.IndexOf(Categories.FirstOrDefault(c => c.CategoryId == data.CategoryId));
                if (index != -1)
                {
                    Categories[index] = data;
                }
                var index2 = CategoryList.IndexOf(CategoryList.FirstOrDefault(c => c == data.Name));
                if (index2 != -1)
                {
                    CategoryList[index2] = data.Name;
                }
                return "Sửa danh mục thành công";
            }
            return response;
        }

        partial void OnSearchQueryChanged(string value)
        {
            ApplyFilter();
        }

        partial void OnStatusQueryChanged(string value)
        {
            ApplyFilter();
        }

        partial void OnSelectedCategoryChanged(string value)
        {
            ApplyFilter();
        }
    }
}
