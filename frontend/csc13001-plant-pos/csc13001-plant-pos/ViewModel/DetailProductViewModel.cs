using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using csc13001_plant_pos.Model;
using csc13001_plant_pos.DTO;
using csc13001_plant_pos.Service;
using Windows.Storage;

namespace csc13001_plant_pos.ViewModel
{
    public partial class DetailProductViewModel : ObservableObject
    {
        [ObservableProperty]
        private Product currentProduct;

        [ObservableProperty]
        private bool isTopSelling;

        [ObservableProperty]
        private ObservableCollection<Category> categories;

        private readonly IProductService _productService;
        private readonly IStatisticService _statisticService;
        private readonly ICategoryService _categoryService;

        public DetailProductViewModel(IProductService productService, IStatisticService statisticService, ICategoryService categoryService)
        {
            _productService = productService;
            _statisticService = statisticService;
            _categoryService = categoryService;
            LoadCategoryAsync();
        }
        public async Task LoadCategoryAsync()
        {
            var response2 = await _categoryService.GetCategoriesAsync();
            if (response2?.Status == "success" && response2.Data != null)
            {
                Categories = new ObservableCollection<Category>(response2.Data);
            }
        }
        //public async Task LoadProductAsync(string productId)
        //{
        //    // Load chi tiết sản phẩm
        //    var productResponse = await _productService.GetProductByIdAsync(productId);
        //    if (productResponse?.Status == "success" && productResponse.Data != null)
        //    {
        //        CurrentProduct = productResponse.Data;
        //    }
        //    else
        //    {
        //        System.Diagnostics.Debug.WriteLine($"Failed to load product {productId}: {productResponse?.Message}");
        //    }

        //    // Load danh sách sản phẩm bán chạy (top 10)
        //    var statisticResponse = await _statisticService.GetProductsAsync();
        //    if (statisticResponse?.Status == "success" && statisticResponse.Data != null)
        //    {
        //        topSellingProducts = statisticResponse.Data
        //            .Take(10)
        //            .Select(dto => dto.Product)
        //            .ToList();

        //        IsTopSelling = topSellingProducts.Any(p => p.ProductId == CurrentProduct.ProductId);
        //    }
        //    else
        //    {
        //        System.Diagnostics.Debug.WriteLine($"Failed to load top selling products: {statisticResponse?.Message}");
        //        IsTopSelling = false;
        //    }
        //}

        public async Task UpdateProductAsync(Product product, StorageFile selectedFile)
        {
            var response = await _productService.UpdateProductAsync( product, selectedFile);
            
        }
    }
}