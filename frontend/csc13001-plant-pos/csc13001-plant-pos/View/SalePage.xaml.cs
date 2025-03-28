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
            ViewModel = new SaleViewModel();
            DataContext = ViewModel;
            this.InitializeComponent();
        }
        private void ProductItem_Tapped(object sender, Microsoft.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (sender is FrameworkElement element && element.Tag is Product product)
            {
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
    }
}
