using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using csc13001_plant_pos.ViewModel;
using csc13001_plant_pos.Model;

namespace csc13001_plant_pos.View { 
    public sealed partial class SalePage : Page
    {
        public SaleViewModel ViewModel { get; }
        public SalePage()
        {
            this.InitializeComponent();
            this.DataContext = ViewModel = App.GetService<SaleViewModel>();
        }

        private void ProductGridView_LostFocus(object sender, RoutedEventArgs e)
        {
            ProductGridView.SelectedItem = null; 
        }
        private void ProductItem_Tapped(object sender, Microsoft.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (sender is FrameworkElement element && element.Tag is Product product)
            {
                if (product.Stock == 0)
                {
                    var outOfStockDialog = new ContentDialog
                    {
                        Title = "Thông báo",
                        Content = "Sản phẩm đã hết hàng!",
                        CloseButtonText = "Đóng",
                        DefaultButton = ContentDialogButton.Close,
                        XamlRoot = this.XamlRoot
                    };
                    outOfStockDialog.ShowAsync();
                    return;
                }

                ViewModel.AddToOrderCommand.Execute(product);
            }
        }
        private async void NoteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var orderItem = button.DataContext as CurrentOrder;

            // Create TextBox
            var textBox = new TextBox
            {
                PlaceholderText = "Nhập ghi chú...",
                Text = orderItem.Note,
                TextWrapping = TextWrapping.Wrap,
                AcceptsReturn = true,
                Height = 100,
                Margin = new Thickness(0, 10, 0, 0)
            };

            // Create ContentDialog
            var dialog = new ContentDialog
            {
                Title = "Thêm ghi chú",
                Content = textBox,
                PrimaryButtonText = "Lưu",
                CloseButtonText = "Huỷ",
                DefaultButton = ContentDialogButton.Primary,
                XamlRoot = this.XamlRoot
            };

            // Show dialog and handle result
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                orderItem.Note = textBox.Text;
            }
        }
        private async void PhoneNumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ViewModel != null)
            {
                await ViewModel.LoadDiscountsAsync(PhoneNumberTextBox.Text);
            }
        }

        private async void CreateOrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel != null)
            {
                if (ViewModel.CurrentOrders.Count == 0)
                {
                    var noProductDialog = new ContentDialog
                    {
                        Title = "Thông báo",
                        Content = "Vui lòng thêm ít nhất một sản phẩm vào đơn hàng!",
                        CloseButtonText = "Đóng",
                        DefaultButton = ContentDialogButton.Close,
                        XamlRoot = this.XamlRoot
                    };

                    await noProductDialog.ShowAsync();
                    return;
                }
                var orderId = await ViewModel.CreateOrderAsync();
                if (orderId != null)
                {
                    Frame.Navigate(typeof(BillPage), orderId);
                }
                else
                {
                    var notFoundCustomerPhone = new ContentDialog
                    {
                        Title = "Thông báo",
                        Content = "Số điện thoại khách hàng không tồn tại!",
                        CloseButtonText = "Đóng",
                        DefaultButton = ContentDialogButton.Close,
                        XamlRoot = this.XamlRoot
                    };
                    await notFoundCustomerPhone.ShowAsync();
                }
            }
        }
    }
}
