using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
namespace csc13001_plant_pos.ViewModel
{
    #region Models

    public class Category : ObservableObject
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }
    }

    public class Product : ObservableObject
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string PriceFormatted => $"${Price:F2}";

        private int _stock;
        public int Stock
        {
            get => _stock;
            set => SetProperty(ref _stock, value);
        }

        public string StockFormatted => $"{Stock} Product(s)";
        public string Image { get; set; }
    }

    public class OrderItem : ObservableObject
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }

        private string _note;
        public string Note
        {
            get => _note;
            set => SetProperty(ref _note, value);
        }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (SetProperty(ref _quantity, value))
                {
                    OnPropertyChanged(nameof(TotalPrice));
                    OnPropertyChanged(nameof(TotalPriceFormatted));
                }
            }
        }

        public double TotalPrice => Price * Quantity;
        public string TotalPriceFormatted => $"${TotalPrice:F2}";
    }

    #endregion

    public partial class SaleViewModel : ObservableRecipient
    {
        #region Properties

        public ObservableCollection<Category> Categories { get; set; }
        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<OrderItem> OrderItems { get; set; }

        private Category _selectedCategory;
        public Category SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (SetProperty(ref _selectedCategory, value))
                {
                    OnPropertyChanged(nameof(FilteredProducts));
                    OnPropertyChanged(nameof(SelectedCategory));
                }
            }
        }

        public IEnumerable<Product> FilteredProducts =>
            SelectedCategory == null ? Products : Products.Where(p => p.CategoryId == SelectedCategory.CategoryId);

        public int TotalItems => OrderItems.Sum(i => i.Quantity);
        public string TotalItemsFormatted => $"Số lượng ({TotalItems})";

        public double SubTotal => OrderItems.Sum(i => i.TotalPrice);
        public string SubTotalFormatted => $"${SubTotal:F2}";

        public double Tax => SubTotal * 0.1;
        public string TaxFormatted => $"${Tax:F2}";

        public double Total => SubTotal + Tax;
        public string TotalFormatted => $"${Total:F2}";

        #endregion

        #region Constructor

        public SaleViewModel()
        {
            Categories = new ObservableCollection<Category>
            {
                new Category { CategoryId = 1, Name = "Terrarium", Description = "Mini ecosystem" },
                new Category { CategoryId = 2, Name = "Aquatic Plants", Description = "For aquariums" }
            };

            Products = new ObservableCollection<Product>
            {
                new Product
                {
                    ProductId = 1,
                    CategoryId = 1,
                    Name = "Cây tùng bồng lai",
                    Price = 25.99,
                    Stock = 10,
                    Image = "https://res.cloudinary.com/djyugezvf/image/upload/v1742618091/csc13001/pifd9aimwfdzw8zyqmjd.jpg"
                },
                new Product
                {
                    ProductId = 2,
                    CategoryId = 2,
                    Name = "Cây phát tài",
                    Price = 15.50,
                    Stock = 5,
                    Image = "https://res.cloudinary.com/djyugezvf/image/upload/v1742618630/csc13001/kfgj97onhvytghw10twl.jpg"
                },
                new Product
                {
                    ProductId = 3,
                    CategoryId = 2,
                    Name = "Cây ngũ gia bì cẩm thạch",
                    Price = 15.50,
                    Stock = 5,
                    Image = "https://res.cloudinary.com/djyugezvf/image/upload/v1742618783/csc13001/xldw2yuc0oeqizcosesx.jpg"
                },
                new Product
                {
                    ProductId = 4,
                    CategoryId = 2,
                    Name = "Cây kim ngân ba thân",
                    Price = 15.50,
                    Stock = 5,
                    Image = "https://res.cloudinary.com/djyugezvf/image/upload/v1742618922/csc13001/phpg3zxrxwaokhcc9ppv.jpg"
                },
                new Product
                {
                    ProductId = 5,
                    CategoryId = 2,
                    Name = "Cây trầu bà cột",
                    Price = 15.50,
                    Stock = 5,
                    Image = "https://res.cloudinary.com/djyugezvf/image/upload/v1742619025/csc13001/nua3alzyfo6siz6ehbgf.jpg"
                },
                new Product
                {
                    ProductId = 6,
                    CategoryId = 2,
                    Name = "Cây bông giấy Sakura",
                    Price = 15.50,
                    Stock = 5,
                    Image = "https://res.cloudinary.com/djyugezvf/image/upload/v1742619211/csc13001/pkxeuclvhbwogvafpupg.jpg"
                },
                new Product
                {
                    ProductId = 7,
                    CategoryId = 2,
                    Name = "Chậu hoa cúc mâm xôi",
                    Price = 15.50,
                    Stock = 5,
                    Image = "https://res.cloudinary.com/djyugezvf/image/upload/v1742619323/csc13001/wyu6wyqgtskx0el6fqhx.jpg"
                },
                new Product
                {
                    ProductId = 8,
                    CategoryId = 2,
                    Name = "Cây bông giấy thái ghép",
                    Price = 15.50,
                    Stock = 5,
                    Image = "https://res.cloudinary.com/djyugezvf/image/upload/v1742619689/csc13001/daffahluekpeuprklh0e.jpg"
                },
                new Product
                {
                    ProductId = 9,
                    CategoryId = 2,
                    Name = "Cây bàng đài loan cẩm thạch",
                    Price = 15.50,
                    Stock = 5,
                    Image = "https://res.cloudinary.com/djyugezvf/image/upload/v1742619826/csc13001/y3hojjoeqdjvfvjx9org.jpg"
                },
                new Product
                {
                    ProductId = 10,
                    CategoryId = 2,
                    Name = "Cây trúc mặt trời ‘Compacta’",
                    Price = 15.50,
                    Stock = 5,
                    Image = "https://res.cloudinary.com/djyugezvf/image/upload/v1742619995/csc13001/zuuqihzd7uyy4bmppvpp.jpg"
                },
                new Product
                {
                    ProductId = 11,
                    CategoryId = 2,
                    Name = "Cây phú quý’",
                    Price = 15.50,
                    Stock = 5,
                    Image = "https://res.cloudinary.com/djyugezvf/image/upload/v1742620683/csc13001/hchjebf3su3nhjymjp74.jpg"
                },
                new Product
                {
                    ProductId = 12,
                    CategoryId = 2,
                    Name = "Cây lưỡi hổ Thái ‘Futura superba’",
                    Price = 15.50,
                    Stock = 5,
                    Image = "https://res.cloudinary.com/djyugezvf/image/upload/v1742620086/csc13001/mztukw97mql8vvaubcdr.jpg"
                },
                new Product
                {
                    ProductId = 13,
                    CategoryId = 2,
                    Name = "Cây vạn lộc son",
                    Price = 15.50,
                    Stock = 5,
                    Image = "https://res.cloudinary.com/djyugezvf/image/upload/v1742620373/csc13001/mbgdqaflohsx8e126csy.jpg"
                },
                new Product
                {
                    ProductId = 14,
                    CategoryId = 2,
                    Name = "Cây ngọc ngân",
                    Price = 15.50,
                    Stock = 5,
                    Image = "https://res.cloudinary.com/djyugezvf/image/upload/v1742620467/csc13001/xply6ushku54nhgizncx.jpg"
                },
            };

            OrderItems = new ObservableCollection<OrderItem>();
        }

        #endregion

        #region Commands

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

            var existingItem = OrderItems.FirstOrDefault(item => item.ProductId == product.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                OrderItems.Add(new OrderItem
                {
                    ProductId = product.ProductId,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = 1,
                    Image = product.Image
                });
            }

            product.Stock--;
            UpdateTotals();
        }

        [RelayCommand]
        private void IncreaseQuantity(OrderItem item)
        {
            var product = Products.First(p => p.ProductId == item.ProductId);
            if (product.Stock > 0)
            {
                item.Quantity++;
                product.Stock--;
                UpdateTotals();
            }
        }

        [RelayCommand]
        private void DecreaseQuantity(OrderItem item)
        {
            if (item.Quantity <= 0) return;

            var product = Products.First(p => p.ProductId == item.ProductId);
            item.Quantity--;
            product.Stock++;

            if (item.Quantity == 0)
            {
                OrderItems.Remove(item);
            }

            UpdateTotals();
        }

        [RelayCommand]
        private void RemoveItem(OrderItem item)
        {
            var product = Products.First(p => p.ProductId == item.ProductId);
            product.Stock += item.Quantity;
            OrderItems.Remove(item);
            UpdateTotals();
        }

        #endregion

        #region Helper Methods

        private void UpdateTotals()
        {
            OnPropertyChanged(nameof(TotalItems));
            OnPropertyChanged(nameof(TotalItemsFormatted));
            OnPropertyChanged(nameof(SubTotal));
            OnPropertyChanged(nameof(SubTotalFormatted));
            OnPropertyChanged(nameof(Tax));
            OnPropertyChanged(nameof(TaxFormatted));
            OnPropertyChanged(nameof(Total));
            OnPropertyChanged(nameof(TotalFormatted));
        }

        #endregion
    }
}