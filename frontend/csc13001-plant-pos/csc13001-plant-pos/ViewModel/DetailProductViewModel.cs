using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using csc13001_plant_pos.Model;
using csc13001_plant_pos.Service;

namespace csc13001_plant_pos.ViewModel
{
    public partial class DetailProductViewModel : ObservableObject
    {
        [ObservableProperty]
        private Product currentProduct;

        [ObservableProperty]
        private bool isTopSelling;

        private List<Product> topSellingProducts = new List<Product>();
        private readonly IProductService _productService;
        private readonly IStatisticService _statisticService;

        public DetailProductViewModel(IProductService productService, IStatisticService statisticService)
        {
            _productService = productService;
            _statisticService = statisticService;
        }

        public async Task LoadProductAsync(string productId)
        {
            // Load chi tiết sản phẩm
            var productResponse = await _productService.GetProductByIdAsync(productId);
            if (productResponse?.Status == "success" && productResponse.Data != null)
            {
                CurrentProduct = productResponse.Data;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load product {productId}: {productResponse?.Message}");
            }

            // Load danh sách sản phẩm bán chạy (top 10)
            var statisticResponse = await _statisticService.GetProductsAsync();
            if (statisticResponse?.Status == "success" && statisticResponse.Data != null)
            {
                topSellingProducts = statisticResponse.Data
                    .Take(10)
                    .Select(dto => dto.Product)
                    .ToList();

                IsTopSelling = topSellingProducts.Any(p => p.ProductId == CurrentProduct.ProductId);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load top selling products: {statisticResponse?.Message}");
                IsTopSelling = false;
            }
        }
    }
}