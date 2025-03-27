using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using csc13001_plant_pos.DTO;
using csc13001_plant_pos.Model;

namespace csc13001_plant_pos.ViewModel
{
    public partial class SaleViewModel : ObservableRecipient
    {
        #region Properties
        public ObservableCollection<Category> Categories { get; set; } = new ObservableCollection<Category>();
        public ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();
        public ObservableCollection<CurrentOrder> CurrentOrders { get; set; } = new ObservableCollection<CurrentOrder>();

        private Category _selectedCategory;
        public Category SelectedCategory
        {
            get => _selectedCategory;
            set => SetProperty(ref _selectedCategory, value);
        }

        public IEnumerable<Product> FilteredProducts =>
            SelectedCategory == null ? Products : Products.Where(p => p.CategoryId == SelectedCategory.CategoryId);

        public int TotalItems => CurrentOrders.Sum(i => i.Quantity);
        public decimal SubTotal => CurrentOrders.Sum(i => i.Quantity * i.Price);
        public decimal DiscountAmount => SubTotal * 0.1m;  // Giảm giá 10%
        public decimal Total => SubTotal - DiscountAmount;

        private readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:8080/") };

        #endregion

        #region Constructor

        public SaleViewModel()
        {
            LoadCategoriesAsync();
            LoadProductsAsync();

            // Gọi UpdateOrderSummary khi danh sách thay đổi
            CurrentOrders.CollectionChanged += (s, e) => UpdateOrderSummary();
        }

        #endregion

        #region Helper Method

        private void UpdateOrderSummary()
        {
            OnPropertyChanged(nameof(TotalItems));
            OnPropertyChanged(nameof(SubTotal));
            OnPropertyChanged(nameof(DiscountAmount));
            OnPropertyChanged(nameof(Total));
        }

        #endregion

        #region API Calls

        private async void LoadCategoriesAsync()
        {
            try
            {
                string json = await _httpClient.GetStringAsync("api/categories");
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<Category>>>(json);
                System.Diagnostics.Debug.WriteLine($"Status: {apiResponse?.Status}, Message: {apiResponse?.Message}");

                if (apiResponse?.Data != null)
                {
                    Categories.Clear();
                    foreach (var category in apiResponse.Data)
                    {
                        Categories.Add(category);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading categories: {ex.Message}");
            }
        }

        private async void LoadProductsAsync()
        {
            try
            {
                string json = await _httpClient.GetStringAsync("api/products");
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<Product>>>(json);
                System.Diagnostics.Debug.WriteLine($"Status: {apiResponse?.Status}, Message: {apiResponse?.Message}");
                if (apiResponse?.Data != null)
                {
                    Products.Clear();
                    foreach (var product in apiResponse.Data)
                    {
                        Products.Add(product);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading products: {ex.Message}");
            }
        }

        #endregion

        #region Commands

        [RelayCommand]
        private void SelectCategory(Category category)
        {
            foreach (var cat in Categories)
            {
                cat.IsSelected = false;
            }

            if (SelectedCategory == category)
            {
                SelectedCategory = null;
            }
            else
            {
                SelectedCategory = category;
                SelectedCategory.IsSelected = true;
            }
        }

        [RelayCommand]
        private void AddToOrder(Product product)
        {
            if (product.Stock <= 0) return;

            var existingItem = CurrentOrders.FirstOrDefault(item => item.Id == product.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                CurrentOrders.Add(new CurrentOrder
                {
                    Id = product.ProductId,
                    Name = product.Name,
                    ImageUrl = product.ImageUrl,
                    Price = product.SalePrice,
                    Quantity = 1,
                });
            }

            product.Stock--;
            UpdateOrderSummary();  // Cập nhật UI
        }

        [RelayCommand]
        private void IncreaseQuantity(CurrentOrder item)
        {
            var product = Products.First(p => p.ProductId == item.Id);
            if (product.Stock > 0)
            {
                item.Quantity++;
                product.Stock--;
            }
            UpdateOrderSummary();  // Cập nhật UI
        }

        [RelayCommand]
        private void DecreaseQuantity(CurrentOrder item)
        {
            if (item.Quantity <= 0) return;

            var product = Products.First(p => p.ProductId == item.Id);
            item.Quantity--;
            product.Stock++;

            if (item.Quantity == 0)
            {
                CurrentOrders.Remove(item);
            }

            UpdateOrderSummary();  // Cập nhật UI
        }

        [RelayCommand]
        private void RemoveItem(CurrentOrder item)
        {
            var product = Products.First(p => p.ProductId == item.Id);
            product.Stock += item.Quantity;
            CurrentOrders.Remove(item);

            UpdateOrderSummary();  // Cập nhật UI
        }

        #endregion
    }
}
