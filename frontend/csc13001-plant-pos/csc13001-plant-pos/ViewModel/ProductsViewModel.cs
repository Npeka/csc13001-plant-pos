using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using csc13001_plant_pos.Model;
using csc13001_plant_pos.Service;

namespace csc13001_plant_pos.ViewModel
{
    public partial class ProductsViewModel : ObservableRecipient
    {
        [ObservableProperty]
        private ObservableCollection<Category> categoriesWithProducts = new ObservableCollection<Category>();

        [ObservableProperty]
        private ObservableCollection<Category> filteredCategoriesWithProducts = new ObservableCollection<Category>();

        [ObservableProperty]
        private string searchQuery = string.Empty;

        [ObservableProperty]
        private int wateringFilter = 0;

        [ObservableProperty]
        private int careLevelFilter = 0;

        [ObservableProperty]
        private int lightFilter = 0;

        [ObservableProperty]
        private int sizeFilter = 0;

        [ObservableProperty]
        private int priceFilter = 0;

        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private List<Product> allProducts;

        public ProductsViewModel(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName is nameof(SearchQuery) or nameof(WateringFilter) or nameof(CareLevelFilter) or nameof(LightFilter) or nameof(SizeFilter) or nameof(PriceFilter))
                {
                    ApplyFilters();
                }
            };

            LoadDataAsync();
        }

        public async void LoadDataAsync()
        {
            var categoryResponse = await _categoryService.GetCategoriesAsync();
            if (categoryResponse?.Data != null)
            {
                CategoriesWithProducts.Clear();
                foreach (var category in categoryResponse.Data)
                {
                    category.Products = new ObservableCollection<Product>();
                    CategoriesWithProducts.Add(category);
                }
            }

            var productResponse = await _productService.GetProductsAsync();
            if (productResponse?.Data != null)
            {
                allProducts = productResponse.Data.ToList();
                foreach (var product in allProducts)
                {
                    var category = CategoriesWithProducts.FirstOrDefault(c => c.CategoryId == product.Category.CategoryId);
                    if (category != null)
                    {
                        category.Products.Add(product);
                    }
                }
            }
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            FilteredCategoriesWithProducts.Clear();
            foreach (var category in CategoriesWithProducts)
            {
                var filteredProducts = allProducts
                    .Where(p => p.Category.CategoryId == category.CategoryId)
                    .Where(p => string.IsNullOrEmpty(SearchQuery) ||
                                p.Name.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                                p.EnvironmentType.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase))
                    .Where(p => WateringFilter == 0 || p.WateringSchedule == WateringFilter)
                    .Where(p => CareLevelFilter == 0 || p.CareLevel == CareLevelFilter)
                    .Where(p => LightFilter == 0 || p.LightRequirement == LightFilter)
                    .Where(p => SizeFilter == 0 || p.Size == SizeFilter)
                    .Where(p => PriceFilter == 0 || IsInPriceRange(p.SalePrice, PriceFilter))
                    .ToList();

                if (filteredProducts.Any())
                {
                    var filteredCategory = new Category
                    {
                        CategoryId = category.CategoryId,
                        Name = category.Name,
                        Description = category.Description,
                        Products = new ObservableCollection<Product>(filteredProducts)
                    };
                    FilteredCategoriesWithProducts.Add(filteredCategory);
                }
            }
        }

        private bool IsInPriceRange(decimal price, int filter)
        {
            return filter switch
            {
                1 => price < 100000,
                2 => price >= 100000 && price <= 300000,
                3 => price > 300000 && price <= 500000,
                4 => price > 500000 && price <= 1000000,
                5 => price > 1000000,
                _ => true
            };
        }

        [RelayCommand]
        private void ResetFilters()
        {
            SearchQuery = string.Empty;
            WateringFilter = 0;
            CareLevelFilter = 0;
            LightFilter = 0;
            SizeFilter = 0;
            PriceFilter = 0;
            ApplyFilters();
        }
    }
}