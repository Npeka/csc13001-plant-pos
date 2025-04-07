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
using csc13001_plant_pos.Model;
using csc13001_plant_pos.ViewModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace csc13001_plant_pos.View
{
    public sealed partial class ProductManagementPage : Page
    {
        public ProductManagementViewModel ViewModel { get; }

        public ProductManagementPage()
        {
            this.DataContext = ViewModel = App.GetService<ProductManagementViewModel>();
            this.InitializeComponent();
        }

        private void ProductGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Ép kiểu item được click về Product
            var selectedProduct = e.ClickedItem as Product;
            if (selectedProduct != null)
            {
                // Điều hướng đến trang DetailProductPage và truyền đối tượng Product
                Frame.Navigate(typeof(DetailProductPage), selectedProduct);
            }
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

}
