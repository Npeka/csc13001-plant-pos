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
using CommunityToolkit.Mvvm.Input;
using static OfficeOpenXml.ExcelErrorValue;

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

        [ObservableProperty]
        private int currentPage = 1;

        [ObservableProperty]
        private int pageSize = 10;

        [ObservableProperty]
        private int totalPages;

        public ObservableCollection<int> PageSizeOptions { get; set; } = new ObservableCollection<int> { 5, 10, 20 };


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
                ApplyFilters();
            }
        }
        public void ApplyFilters()
        {
            FilteredSellingProducts.Clear();
            var filtered = TopSellingProducts.AsEnumerable();

            if (!string.IsNullOrEmpty(SearchQuery))
            {
                filtered = filtered.Where(emp =>
                emp.Product.Name.ToLower().Contains(SearchQuery) ||
                emp.Product.ProductId.ToString().ToLower().Contains(SearchQuery));
            }
            if (SortType == "TopSelling")
            {
                filtered = filtered.OrderByDescending(p => p.SalesQuantity);
            }
            else if (SortType == "Remain")
            {
                filtered = filtered.OrderByDescending(p => p.Product.Stock);
            }
            var totalItems = filtered.Count();
            TotalPages = Math.Max(1, (int)Math.Ceiling((double)totalItems / PageSize));

            if (CurrentPage > TotalPages) CurrentPage = TotalPages > 0 ? TotalPages : 1;
            filtered = filtered
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize);
            foreach (var user in filtered)
            {
                FilteredSellingProducts.Add(user);
            }
            ProductCount = $"Tổng cộng: {FilteredSellingProducts.Count} sản phẩm";
            OnPropertyChanged(nameof(ProductCount));
            OnPropertyChanged(nameof(FilteredSellingProducts));
        }

        partial void OnSearchQueryChanged(string value)
        {
            ApplyFilters();
        }

        partial void OnSortTypeChanged(string value)
        {
            ApplyFilters();
        }
        [RelayCommand]
        private void NextPage()
        {
            if (CurrentPage < TotalPages)
            {
                CurrentPage++;
                ApplyFilters();
            }
        }

        [RelayCommand]
        private void PreviousPage()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                ApplyFilters();
            }
        }

        partial void OnPageSizeChanged(int value)
        {
            CurrentPage = 1;
            ApplyFilters();
        }
    }
}
