using System;
using csc13001_plant_pos.DTO.OrderDTO;
using csc13001_plant_pos.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace csc13001_plant_pos.View
{
    public sealed partial class OrderPage : Page
    {
        public OrderViewModel ViewModel { get; }

        public OrderPage()
        {
            this.InitializeComponent();
            ViewModel = App.GetService<OrderViewModel>();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.LoadOrdersAsync();
        }

        private void ViewBillButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var orderData = button.DataContext as OrderListDto;
            if (orderData != null)
            {
                string orderId = orderData.OrderId.ToString();
                Frame.Navigate(typeof(BillPage), orderId);
            }
        }

        private async void DeleteOrderButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var orderData = button.DataContext as OrderListDto;
            if (orderData == null) return;
            var dialog = new ContentDialog
            {
                Title = "Xác nhận xóa đơn hàng",
                Content = $"Bạn có chắc chắn muốn xóa đơn hàng {orderData.OrderId} không? Hành động này không thể hoàn tác.",
                PrimaryButtonText = "Xóa",
                CloseButtonText = "Hủy",
                DefaultButton = ContentDialogButton.Primary,
                XamlRoot = this.XamlRoot
            };

            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                bool success = await ViewModel.DeleteOrderAsync(orderData);
                if (!success)
                {
                    ContentDialog errorDialog = new ContentDialog
                    {
                        Title = "Lỗi",
                        Content = "Không thể xóa đơn hàng. Vui lòng thử lại sau.",
                        CloseButtonText = "Đóng",
                        XamlRoot = this.XamlRoot
                    };
                    await errorDialog.ShowAsync();
                }
            }
        }
    }
}