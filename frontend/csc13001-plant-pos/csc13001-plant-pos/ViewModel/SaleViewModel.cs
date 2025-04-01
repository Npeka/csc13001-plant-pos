using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using csc13001_plant_pos.DTO.OrderDTO;
using csc13001_plant_pos.Model;
using csc13001_plant_pos.Service;

namespace csc13001_plant_pos.ViewModel
{
    public partial class SaleViewModel : ObservableRecipient
    {
        [ObservableProperty]
        private ObservableCollection<Category> categories = new ObservableCollection<Category>();

        [ObservableProperty]
        private ObservableCollection<Product> products = new ObservableCollection<Product>();

        [ObservableProperty]
        private ObservableCollection<CurrentOrder> currentOrders = new ObservableCollection<CurrentOrder>();

        [ObservableProperty]
        private Category? selectedCategory;

        [ObservableProperty]
        private string phoneNumber = string.Empty;

        [ObservableProperty]
        private ObservableCollection<DiscountProgram> availableDiscounts = new ObservableCollection<DiscountProgram>();

        [ObservableProperty]
        private DiscountProgram? selectedDiscount;

        [ObservableProperty]
        private int totalItems;

        [ObservableProperty]
        private decimal subTotal;

        [ObservableProperty]
        private decimal discountAmount;

        [ObservableProperty]
        private decimal total;

        public IEnumerable<Product> FilteredProducts =>
            SelectedCategory == null ? Products : Products.Where(p => p.Category.CategoryId == SelectedCategory.CategoryId);

        private readonly IOrderService _orderService;
        private readonly IDiscountProgramService _discountProgramService;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly UserSessionService _userSession;

        public SaleViewModel(
            IOrderService orderService,
            IDiscountProgramService discountProgramService,
            IProductService productService,
            ICategoryService categoryService,
            UserSessionService userSession)
        {
            _orderService = orderService;
            _discountProgramService = discountProgramService;
            _productService = productService;
            _categoryService = categoryService;
            _userSession = userSession;

            LoadCategoriesAsync();
            LoadProductsAsync();
            CurrentOrders.CollectionChanged += (s, e) => CalculateOrderSummary();
            AvailableDiscounts.Add(new DiscountProgram
            {
                DiscountId = -1,
                Name = "Không áp dụng giảm giá",
                DiscountRate = 0,
                ApplicableCustomerType = null,
            });
            SelectedDiscount = AvailableDiscounts.First();
        }

        private void CalculateOrderSummary()
        {
            TotalItems = CurrentOrders.Sum(i => i.Quantity);
            SubTotal = CurrentOrders.Sum(i => i.Quantity * i.Price);
            DiscountAmount = SelectedDiscount != null ? SubTotal * ((int)SelectedDiscount.DiscountRate / 100m) : 0;
            Total = SubTotal - DiscountAmount;
        }

        public async Task LoadDiscountsAsync(string phoneNumber)
        {
            AvailableDiscounts.Clear();
            AvailableDiscounts.Add(new DiscountProgram
            {
                DiscountId = -1,
                Name = "Không áp dụng giảm giá",
                DiscountRate = 0,
                ApplicableCustomerType = null,
            });

            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                SelectedDiscount = AvailableDiscounts.First();
                CalculateOrderSummary();
                return;
            }

            var apiResponse = await _discountProgramService.GetDiscountsByPhoneAsync(phoneNumber);
            if (apiResponse?.Status == "success" && apiResponse.Data != null)
            {
                foreach (var discount in apiResponse.Data)
                {
                    AvailableDiscounts.Add(discount);
                }
                SelectedDiscount = AvailableDiscounts.First();
            }
            else
            {
                SelectedDiscount = AvailableDiscounts.First();
            }
            CalculateOrderSummary();
        }

        private async void LoadCategoriesAsync()
        {
            var apiResponse = await _categoryService.GetCategoriesAsync();
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

        private async void LoadProductsAsync()
        {
            var apiResponse = await _productService.GetProductsAsync();
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
            CalculateOrderSummary();
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
            CalculateOrderSummary();
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
            CalculateOrderSummary();
        }

        [RelayCommand]
        private void RemoveItem(CurrentOrder item)
        {
            var product = Products.First(p => p.ProductId == item.Id);
            product.Stock += item.Quantity;
            CurrentOrders.Remove(item);
            CalculateOrderSummary();
        }

        [RelayCommand]
        public async Task<string?> CreateOrderAsync()
        {
            if (CurrentOrders.Count == 0)
            {
                return null;
            }
            var orderRequest = new OrderCreateDto
            {
                CustomerPhone = string.IsNullOrWhiteSpace(PhoneNumber) ? null : PhoneNumber,
                TotalPrice = Total,
                DiscountId = SelectedDiscount.DiscountId == -1 ? null : SelectedDiscount.DiscountId,
                StaffId = _userSession.CurrentUser.UserId,
                Items = CurrentOrders.Select(item => new OrderItemRequest
                {
                    ProductId = item.Id,
                    Quantity = item.Quantity
                }).ToList()
            };
            var orderId = await _orderService.CreateOrderAsync(orderRequest);
            return orderId;
        }
    }
}