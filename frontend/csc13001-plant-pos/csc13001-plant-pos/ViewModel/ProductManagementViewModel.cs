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

namespace csc13001_plant_pos.ViewModel
{
    public partial class ProductManagementViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Product> productList;

        [ObservableProperty]
        public ObservableCollection<Product> filteredProductList;

        [ObservableProperty]
        private string searchQuery;

        [ObservableProperty]
        private string statusQuery;

        [ObservableProperty]
        private int isAscending = 0;

        private readonly IProductService _productService;

        public ProductManagementViewModel(IProductService productService)
        {
            _productService = productService;
            LoadProductsDataAsync();
        }

        public async void LoadProductsDataAsync()
        {
            var response = await _productService.GetProductsAsync();
            System.Diagnostics.Debug.WriteLine($"Status: {response?.Status}, Message: {response?.Message}");
            if (response?.Status == "success" && response.Data != null)
            {
                ProductList = new ObservableCollection<Product>(response.Data);
                FilteredProductList = new ObservableCollection<Product>(response.Data);
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
            if (IsAscending == 0)
            {
                sortedList = new ObservableCollection<Product>(FilteredProductList.OrderByDescending(p => p.SalePrice));
                IsAscending++;
            }
            else if (IsAscending == 1)
            {
                sortedList = new ObservableCollection<Product>(FilteredProductList.OrderBy(p => p.SalePrice));
                IsAscending++;
            }
            else
            {
                sortedList = ProductList;
                IsAscending = 0;
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

        public void resetFilter()
        {
            FilteredProductList.Clear();
            foreach (var product in ProductList)
            {
                FilteredProductList.Add(product);
            }
            SearchQuery = "";
            IsAscending = 0;
            StatusQuery = "All";
        }

        partial void OnSearchQueryChanged(string value)
        {
            ApplyFilter();
        }

        partial void OnStatusQueryChanged(string value)
        {
            ApplyFilter();
        }

    }
}
