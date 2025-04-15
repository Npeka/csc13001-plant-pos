using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using csc13001_plant_pos.Model;
using csc13001_plant_pos.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace csc13001_plant_pos.View
{
    public sealed partial class DiscountManagementPage : Page
    {
        public DiscountManagementViewModel ViewModel { get; }

        public DiscountManagementPage()
        {
            this.InitializeComponent();
            ViewModel = App.GetService<DiscountManagementViewModel>();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.LoadDiscountsAsync();
        }

        private async void AddDiscountButton_Click(object sender, RoutedEventArgs e)
        {
            await ShowDiscountDialogAsync(null);
        }

        private async void EditDiscountButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as HyperlinkButton;
            var discount = button?.DataContext as DiscountProgram;
            if (discount != null)
            {
                await ShowDiscountDialogAsync(discount);
            }
        }

        private async Task ShowDiscountDialogAsync(DiscountProgram? existingDiscount)
        {
            bool isEdit = existingDiscount != null;
            TextBox nameTextBox = new TextBox
            {
                Header = "Tên chương trình",
                Text = isEdit ? existingDiscount.Name : "",
                Width = 300
            };
            TextBox discountRateTextBox = new TextBox
            {
                Header = "Tỷ lệ giảm giá (%) (1 - 100)",
                Text = isEdit ? existingDiscount.DiscountRate.ToString() : "0",
                Width = 300
            };
            CalendarDatePicker startDatePicker = new CalendarDatePicker
            {
                Header = "Ngày bắt đầu",
                Date = isEdit ? existingDiscount.StartDate : DateTimeOffset.Now,
                Width = 300
            };
            CalendarDatePicker endDatePicker = new CalendarDatePicker
            {
                Header = "Ngày kết thúc",
                Date = isEdit ? existingDiscount.EndDate : DateTimeOffset.Now.AddMonths(1),
                Width = 300
            };
            ComboBox customerTypeComboBox = new ComboBox
            {
                Header = "Loại khách hàng áp dụng",
                ItemsSource = new List<string> { "All", "Bronze", "Silver", "Gold", "Platinum" },
                SelectedItem = isEdit ? existingDiscount.ApplicableCustomerType : "All",
                Width = 300
            };
            StackPanel dialogContent = new StackPanel
            {
                Spacing = 10,
                Children = { nameTextBox, discountRateTextBox, startDatePicker, endDatePicker, customerTypeComboBox }
            };
            ContentDialog dialog = new ContentDialog
            {
                Title = isEdit ? "Sửa chương trình giảm giá" : "Thêm chương trình giảm giá",
                Content = dialogContent,
                PrimaryButtonText = isEdit ? "Lưu" : "Thêm",
                CloseButtonText = "Hủy",
                DefaultButton = ContentDialogButton.Primary,
                XamlRoot = this.XamlRoot
            };

            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                if (string.IsNullOrWhiteSpace(nameTextBox.Text) ||
                    !double.TryParse(discountRateTextBox.Text, out double discountRate) ||
                    discountRate < 0 || discountRate > 100 ||
                    !startDatePicker.Date.HasValue || !endDatePicker.Date.HasValue ||
                    customerTypeComboBox.SelectedItem == null)
                {
                    await ShowErrorDialogAsync("Vui lòng nhập đầy đủ và đúng định dạng thông tin.");
                    return;
                }
                var discount = new DiscountProgram
                {
                    DiscountId = isEdit ? existingDiscount.DiscountId : 0,
                    Name = nameTextBox.Text,
                    DiscountRate = discountRate,
                    StartDate = startDatePicker.Date.Value.DateTime,
                    EndDate = endDatePicker.Date.Value.DateTime,
                    ApplicableCustomerType = customerTypeComboBox.SelectedItem.ToString()
                };

                bool success = isEdit
                    ? await ViewModel.UpdateDiscountAsync(discount)
                    : await ViewModel.CreateDiscountAsync(discount);

                if (!success)
                {
                    await ShowErrorDialogAsync("Không thể lưu chương trình. Vui lòng thử lại.");
                }
            }
        }

        private async Task ShowErrorDialogAsync(string message)
        {
            ContentDialog errorDialog = new ContentDialog
            {
                Title = "Lỗi",
                Content = message,
                CloseButtonText = "Đóng",
                XamlRoot = this.XamlRoot
            };
            await errorDialog.ShowAsync();
        }
    }
}