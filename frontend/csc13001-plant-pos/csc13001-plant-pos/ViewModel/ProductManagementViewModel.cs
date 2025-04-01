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
        private string searchQuery = "";

        [ObservableProperty]
        private bool isAscending = true;

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
                productList = new ObservableCollection<Product>(response.Data);
                filteredProductList = new ObservableCollection<Product>(response.Data);
            }
        }

        public void SearchBox_TextChanged(string value)
        {
            //FilterProducts();
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
            if (productList == null || productList.Count == 0) return;

            var sortedList = isAscending
                ? new ObservableCollection<Product>(filteredProductList.OrderByDescending(p => p.SalePrice))
                : new ObservableCollection<Product>(filteredProductList.OrderBy(p => p.SalePrice));

            filteredProductList.Clear();
            foreach (var product in sortedList)
            {
                filteredProductList.Add(product);
            }

            isAscending = !isAscending;
        }

        public void ApplyFilter()
        {
            filteredProductList.Clear();
            var filtered = productList.AsEnumerable();
            if (!string.IsNullOrEmpty(searchQuery))
            {
                filtered = filtered.Where(emp =>
                emp.Name.ToLower().Contains(searchQuery) ||
                emp.ProductId.ToString().ToLower().Contains(searchQuery));
            }

            //if (startDateQuery != null)
            //{
            //    filtered = filtered.Where(emp => emp.CreatedAt.Date >= startDateQuery.Date);
            //}
            foreach (var product in filtered)
            {
                filteredProductList.Add(product);
            }
        }
    }
}
