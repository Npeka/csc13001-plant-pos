using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using csc13001_plant_pos.DTO.InventoryDTO;
using csc13001_plant_pos.Model;
using csc13001_plant_pos.Service;

namespace csc13001_plant_pos.ViewModel
{
    public partial class AddStockReceiptViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Product> products = new ObservableCollection<Product>();

        [ObservableProperty]
        private ObservableCollection<StockReceiptItem> items = new ObservableCollection<StockReceiptItem>();

        [ObservableProperty]
        private string supplierName = string.Empty;

        [ObservableProperty]
        private DateTimeOffset? purchaseDate = DateTimeOffset.Now;

        [ObservableProperty]
        private int totalItems;

        [ObservableProperty]
        private decimal total;

        private readonly IInventoryService _inventoryService;
        private readonly IProductService _productService;

        public AddStockReceiptViewModel(IInventoryService inventoryService, IProductService productService)
        {
            _inventoryService = inventoryService;
            _productService = productService;
            LoadProductsAsync();
            Items.CollectionChanged += (s, e) => CalculateSummary();
        }

        private async void LoadProductsAsync()
        {
            var apiResponse = await _productService.GetProductsAsync();
            if (apiResponse?.Status == "success" && apiResponse.Data != null)
            {
                Products.Clear();
                Items.Clear();
                foreach (var product in apiResponse.Data)
                {
                    var item = new StockReceiptItem { Product = product };
                    item.PropertyChanged += (s, e) =>
                    {
                        if (e.PropertyName == nameof(StockReceiptItem.PurchasePrice) && item.PurchasePrice <= 0)
                        {
                            item.Quantity = 0;
                            CalculateSummary();
                        }
                    };
                    Products.Add(product);
                    Items.Add(item);
                }
            }
        }

        private void CalculateSummary()
        {
            TotalItems = Items.Sum(i => i.Quantity);
            Total = Items.Sum(i => i.Quantity * i.PurchasePrice);
        }

        [RelayCommand]
        private void IncreaseQuantity(StockReceiptItem item)
        {
            if (item != null && item.PurchasePrice > 0)
            {
                item.Quantity++;
                CalculateSummary();
            }
        }

        [RelayCommand]
        private void DecreaseQuantity(StockReceiptItem item)
        {
            if (item != null && item.Quantity > 0 && item.PurchasePrice > 0)
            {
                item.Quantity--;
                CalculateSummary();
            }
        }

        [RelayCommand]
        private void ResetForm()
        {
            SupplierName = string.Empty;
            PurchaseDate = DateTimeOffset.Now;
            foreach (var item in Items)
            {
                item.Quantity = 0;
                item.PurchasePrice = 0;
            }
            CalculateSummary();
        }

        public async Task<bool> CreateStockReceiptAsync()
        {
            if (string.IsNullOrWhiteSpace(SupplierName) || !PurchaseDate.HasValue || !Items.Any(i => i.Quantity > 0))
            {
                return false;
            }

            var inventoryCreateDto = new InventoryCreateDto
            {
                Supplier = SupplierName,
                TotalPrice = Total,
                PurchaseDate = PurchaseDate.Value.DateTime,
                Items = Items
                    .Where(i => i.Quantity > 0)
                    .Select(item => new InventoryItemRequest
                    {
                        ProductId = item.Product.ProductId,
                        Quantity = item.Quantity,
                        PurchasePrice = item.PurchasePrice
                    }).ToList()
            };

            return await _inventoryService.CreateInventoryAsync(inventoryCreateDto);
        }
    }
}