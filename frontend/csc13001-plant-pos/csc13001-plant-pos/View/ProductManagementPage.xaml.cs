using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace csc13001_plant_pos.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProductManagementPage : Page
    {
        public ObservableCollection<Product> Products { get; set; }

        public ProductManagementPage()
        {
            this.InitializeComponent();
            //LoadData();
        }

        //private void LoadData()
        //{
        //    Products = new ObservableCollection<Product>
        //    {
        //        // Điện thoại
        //        new Product { Name = "iPhone 15 Pro Max", Price = 32990000, OriginalPrice = 35990000, IsDiscounted = true, Stock = "Còn 10 sản phẩm", StockColor = "Green", ImageUrl = "https://cdn.dribbble.com/userupload/4948969/file/original-ed38ad62858ef39f6545da92751b51ec.png" },
        //        new Product { Name = "Samsung Galaxy S24 Ultra", Price = 29990000, OriginalPrice = 29990000, IsDiscounted = false, Stock = "Còn 7 sản phẩm", StockColor = "Green", ImageUrl = "https://cdn.dribbble.com/userupload/4948969/file/original-ed38ad62858ef39f6545da92751b51ec.png" },
        //        new Product { Name = "Xiaomi 13 Ultra", Price = 21990000, OriginalPrice = 23990000, IsDiscounted = true, Stock = "Còn 5 sản phẩm", StockColor = "Green", ImageUrl = "https://cdn.dribbble.com/userupload/4948969/file/original-ed38ad62858ef39f6545da92751b51ec.png" },

        //        // Laptop
        //        new Product { Name = "MacBook Air M2", Price = 25990000, OriginalPrice = 28990000, IsDiscounted = true, Stock = "Còn 4 sản phẩm", StockColor = "Green", ImageUrl = "https://cdn.dribbble.com/userupload/4948969/file/original-ed38ad62858ef39f6545da92751b51ec.png" },
        //        new Product { Name = "Asus ROG Zephyrus G14", Price = 37990000, OriginalPrice = 39990000, IsDiscounted = true, Stock = "Còn 2 sản phẩm", StockColor = "Green", ImageUrl = "https://cdn.dribbble.com/userupload/4948969/file/original-ed38ad62858ef39f6545da92751b51ec.png" },
        //        new Product { Name = "Dell XPS 13 Plus", Price = 40990000, OriginalPrice = 40990000, IsDiscounted = false, Stock = "Hết hàng", StockColor = "Red", ImageUrl = "https://cdn.tgdd.vn/Products/Images/44/296047/dell-xps-13-plus-9320-thumb-600x600.jpg" },

        //        // Phụ kiện
        //        new Product { Name = "AirPods Pro 2", Price = 5499000, OriginalPrice = 5999000, IsDiscounted = true, Stock = "Còn 15 sản phẩm", StockColor = "Green", ImageUrl = "https://cdn.tgdd.vn/Products/Images/54/289704/apple-airpods-pro-2nd-gen-thumb-600x600.jpg" },
        //        new Product { Name = "Bàn phím cơ Keychron K6", Price = 1799000, OriginalPrice = 1799000, IsDiscounted = false, Stock = "Còn 8 sản phẩm", StockColor = "Green", ImageUrl = "https://cdn.tgdd.vn/Products/Images/86/278996/keychron-k6-thumb-600x600.jpg" },
        //        new Product { Name = "Chuột Logitech MX Master 3S", Price = 2599000, OriginalPrice = 2799000, IsDiscounted = true, Stock = "Còn 12 sản phẩm", StockColor = "Green", ImageUrl = "https://cdn.tgdd.vn/Products/Images/86/306789/logitech-mx-master-3s-thumb-600x600.jpg" }
        //    };

        //    ProductGridView.ItemsSource = Products;
        //}


        //private bool isAscending = true;

        private void SortByPrice_Click(object sender, RoutedEventArgs e)
        {
            //if (Products == null || Products.Count == 0) return;

            //var sortedList = isAscending
            //    ? new ObservableCollection<Product>(Products.OrderByDescending(p => p.Price))
            //    : new ObservableCollection<Product>(Products.OrderBy(p => p.Price));

            //Products.Clear();
            //foreach (var product in sortedList)
            //{
            //    Products.Add(product);
            //}

            //isAscending = !isAscending;
        }

        private void FilterProducts()
        {
            //if (Products == null || Products.Count == 0)
            //    return;

            //var searchQuery = SearchBox.Text.ToLower();
            //var stockFilter = StockFilter.SelectedItem as ComboBoxItem;
            //var stockFilterValue = stockFilter?.Content.ToString();

            //var filteredProducts = Products.Where(p => p.Name.ToLower().Contains(searchQuery));

            //if (stockFilterValue == "Còn hàng")
            //{
            //    filteredProducts = filteredProducts.Where(p => p.StockColor == "Green");
            //}
            //else if (stockFilterValue == "Hết hàng")
            //{
            //    filteredProducts = filteredProducts.Where(p => p.StockColor == "Red");
            //}

            //Products.Clear();
            //foreach (var product in filteredProducts)
            //{
            //    Products.Add(product);
            //}
        }

        private async void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            //AddProductDialog.XamlRoot = this.XamlRoot; // Đảm bảo hộp thoại hiển thị đúng trong ứng dụng WinUI
            //await AddProductDialog.ShowAsync();
        }
        private void AddProductDialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // Xử lý khi người dùng nhấn nút đóng (Close)
            // Nếu không cần xử lý gì, có thể để trống hoặc chỉ đóng dialog
        }

        private void AddProductDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // Lấy dữ liệu từ form
            //string name = ProductNameBox.Text;
            //int price = int.TryParse(ProductPriceBox.Text, out int p) ? p : 0;
            //string imageUrl = ProductImageBox.Text;
            //int stock = int.TryParse(ProductStockBox.Text, out int s) ? s : 0;

            //// Kiểm tra hợp lệ
            //if (string.IsNullOrWhiteSpace(name) || price <= 0 || stock < 0 || string.IsNullOrWhiteSpace(imageUrl))
            //{
            //    return; // Không thêm nếu dữ liệu không hợp lệ
            //}

            //// Thêm sản phẩm mới vào danh sách
            //Products.Add(new Product
            //{
            //    Name = name,
            //    Price = price,
            //    OriginalPrice = price,
            //    IsDiscounted = false,
            //    Stock = $"Còn {stock} sản phẩm",
            //    StockColor = stock > 0 ? "Green" : "Red",
            //    ImageUrl = imageUrl
            //});

            // Đóng hộp thoại (mặc định đã đóng sau khi nhấn PrimaryButton)
        }


        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //FilterProducts();
        }

        private void StockFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //FilterProducts();
        }
        private void CategoryFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void UpdateCategories_Click(object sender, RoutedEventArgs e)
        {
            // Xử lý cập nhật danh mục
        }
    }

    //public class Product
    //{
    //    public string Name { get; set; }
    //    public int Price { get; set; }
    //    public int OriginalPrice { get; set; }
    //    public bool IsDiscounted { get; set; }
    //    public string Stock { get; set; }
    //    public string StockColor { get; set; }
    //    public string ImageUrl { get; set; }
    //}
}
