using System;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using csc13001_plant_pos.Model;
using csc13001_plant_pos.Service;
using System.Collections.ObjectModel;
using System.Diagnostics;
using csc13001_plant_pos.DTO.CustomerDTO;
using csc13001_plant_pos.DTO.ProductDTO;
using System.Collections.Generic;
using System.Globalization;

namespace csc13001_plant_pos.ViewModel
{
    public partial class TopSellingProductViewModel : ObservableObject
    {

        [ObservableProperty]
        private ObservableCollection<ProductDto> topSellingProducts;

        [ObservableProperty]
        private ObservableCollection<ProductDto> filteredSellingProducts;

        [ObservableProperty]
        private string searchQuery = "";

        [ObservableProperty]
        private string sortType;

        [ObservableProperty]
        private string productCount;



        private readonly IStatisticService _statisticService;

        public TopSellingProductViewModel(IStatisticService statisticService)
        {
            _statisticService = statisticService;
            LoadTopSellingDataAsync();
        }

        public async void LoadTopSellingDataAsync()
        {
            var response = await _statisticService.GetProductsAsync();
            System.Diagnostics.Debug.WriteLine($"Status: {response?.Status}, Message: {response?.Message}");
            if (response?.Status == "success" && response.Data != null)
            {
                TopSellingProducts = new ObservableCollection<ProductDto>(response.Data);
                FilteredSellingProducts = new ObservableCollection<ProductDto>(response.Data);
                ProductCount = $"Total: {TopSellingProducts.Count} sản phẩm";
            }
        }

        partial void OnSearchQueryChanged(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                OnSortTypeChanged(sortType);
                return;
            }
            filteredSellingProducts.Clear();
            var lowerValue = value.Trim().ToLower();
            var filtered = topSellingProducts
                .Where(p => p.Product.Name.ToLower().Contains(lowerValue));

            if (sortType == "TopSelling")
                filtered = filtered.OrderByDescending(p => p.SalesQuantity);
            else if (sortType == "Remain")
                filtered = filtered.OrderByDescending(p => p.Product.Stock);
            foreach (var item in filtered)
            {
                filteredSellingProducts.Add(item);
            }
            ProductCount = $"Total: {filteredSellingProducts.Count} sản phẩm";
            OnPropertyChanged(nameof(ProductCount));
        }

        partial void OnSortTypeChanged(string value)
        {
            sortType = value;

            IEnumerable<ProductDto> sorted = topSellingProducts;
            filteredSellingProducts.Clear();
            if (value == "TopSelling")
            {
                sorted = sorted.OrderByDescending(p => p.SalesQuantity);
            }
            else if (value == "Remain")
            {
                sorted = sorted.OrderByDescending(p => p.Product.Stock);
            }
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                var lowerQuery = searchQuery.Trim().ToLower();
                sorted = sorted.Where(p => p.Product.Name.ToLower().Contains(lowerQuery));
            }
            foreach(var item in sorted)
            {
                filteredSellingProducts.Add(item);
            }
            ProductCount = $"Total: {filteredSellingProducts.Count} sản phẩm";
            OnPropertyChanged(nameof(ProductCount));
        }
    }
}
