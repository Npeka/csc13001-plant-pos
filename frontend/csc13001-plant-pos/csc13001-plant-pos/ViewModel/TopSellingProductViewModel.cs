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
        private bool isAscending = true;

        [ObservableProperty]
        private int itemsPerPage = 10;

        [ObservableProperty]
        private int currentPage = 1;

        [ObservableProperty]
        private int totalPages = 10;

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
                topSellingProducts = new ObservableCollection<ProductDto>(response.Data);
                filteredSellingProducts = new ObservableCollection<ProductDto>(response.Data);
            }
        }

        private void UpdatePagination()
        {
            filteredSellingProducts.Clear();
            var items = topSellingProducts.Skip((CurrentPage - 1) * ItemsPerPage).Take(ItemsPerPage);
            foreach (var item in items)
            {
                filteredSellingProducts.Add(item);
            }
        }
        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
            }
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage < TotalPages)
            {
                CurrentPage++;
            }
        }

        private void PositionFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // do nothing 
        }
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // do nothing
        }
        partial void OnSearchQueryChanged(string value)
        {
            Debug.WriteLine($"SearchQuery to '{value}'");
        }


    }
}
