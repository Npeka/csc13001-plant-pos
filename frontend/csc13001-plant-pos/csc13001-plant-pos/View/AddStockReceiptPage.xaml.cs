using System;
using System.Linq;
using csc13001_plant_pos.Model;
using csc13001_plant_pos.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

namespace csc13001_plant_pos.View
{
    public sealed partial class AddStockReceiptPage : Page
    {
        public AddStockReceiptViewModel ViewModel { get; }

        public AddStockReceiptPage()
        {
            this.InitializeComponent();
            this.DataContext = ViewModel = App.GetService<AddStockReceiptViewModel>();
        }

        private void PageGrid_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (FocusManager.GetFocusedElement(this.XamlRoot) is TextBox)
            {
                this.Focus(FocusState.Programmatic);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        private void PriceTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                string currentText = textBox.Text;
                string rawText = currentText.Replace(".", "");

                if (!string.IsNullOrEmpty(rawText) && !decimal.TryParse(rawText, out _))
                {
                    textBox.Text = "";
                }
                else if (string.IsNullOrEmpty(rawText))
                {
                    if (textBox.DataContext is StockReceiptItem item)
                    {
                        item.PurchasePrice = 0;
                    }
                }
            }
        }

        private async void AddStockReceiptButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ViewModel.SupplierName) || !ViewModel.PurchaseDate.HasValue || !ViewModel.Items.Any(i => i.Quantity > 0))
            {
                var dialog = new ContentDialog
                {
                    Title = "Thông báo",
                    Content = "Vui lòng nhập đầy đủ thông tin: nhà cung cấp, ngày nhập, và ít nhất một sản phẩm với số lượng lớn hơn 0!",
                    CloseButtonText = "Đóng",
                    DefaultButton = ContentDialogButton.Close,
                    XamlRoot = this.XamlRoot
                };
                await dialog.ShowAsync();
                return;
            }

            bool success = await ViewModel.CreateStockReceiptAsync();
            if (success)
            {
                ViewModel.ResetFormCommand.Execute(null);
                Frame.Navigate(typeof(WarehouseManagementPage));
            }
            else
            {
                var errorDialog = new ContentDialog
                {
                    Title = "Lỗi",
                    Content = "Không thể thêm phiếu nhập hàng. Vui lòng thử lại!",
                    CloseButtonText = "Đóng",
                    DefaultButton = ContentDialogButton.Close,
                    XamlRoot = this.XamlRoot
                };
                await errorDialog.ShowAsync();
            }
        }
    }
}