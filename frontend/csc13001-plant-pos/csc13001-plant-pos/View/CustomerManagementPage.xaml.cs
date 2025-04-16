using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using csc13001_plant_pos.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using csc13001_plant_pos.DTO.CustomerDTO;
using csc13001_plant_pos.Model;

namespace csc13001_plant_pos.View
{

    public sealed partial class CustomerManagementPage : Page
    {
        public CustomerManagementViewModel ViewModel { get; }

        public CustomerManagementPage()
        {
            this.DataContext = ViewModel = App.GetService<CustomerManagementViewModel>();
            this.InitializeComponent();
        }

        public async void AddNewCustomer(object sender, RoutedEventArgs e)
        {
            var newCustomer = new CustomerDto
            {
                Customer = new Customer
                {
                    CustomerId = 0, // Temporary ID (assign real ID after API call)
                    Name = "",
                    Phone = "",
                    Email = "",
                    Gender = "Male",
                    Address = "",
                    BirthDate = null,
                    LoyaltyPoints = 0,
                    LoyaltyCardType = "None",
                    CreateAt = DateTime.Now
                },
                TotalOrders = 0,
                TotalSpent = 0
            };

            await ShowCustomerDialogAsync(newCustomer, false);
        }

        public void ViewCustomer(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var customer = button?.Tag as CustomerDto;
            if (customer != null)
            {
                Frame.Navigate(typeof(CustomerProfilePage), customer.Customer.CustomerId.ToString());
            }
        }

        public async void EditCustomerInformation(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var customer = button?.Tag as CustomerDto;

            if (customer == null)
            {
                return;
            }

            await ShowCustomerDialogAsync(customer, true);
        }

        public async void DeleteCustomer(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var customer = button?.Tag as CustomerDto;
            if (customer == null)
            {
                return;
            }
            ContentDialog deleteDialog = new ContentDialog
            {
                Title = "Xóa khách hàng",
                Content = $"Bạn có chắc chắn muốn xóa khách hàng {customer.Customer.Name} không?",
                PrimaryButtonText = "Xóa",
                CloseButtonText = "Hủy",
                XamlRoot = this.XamlRoot
            };
            var result = await deleteDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                bool success = await ViewModel.DeleteCustomerAsync(customer.Customer.CustomerId.ToString());
                if (!success)
                {
                    await ShowErrorDialogAsync("Không thể xóa khách hàng. Vui lòng thử lại.");
                }
            }
        }

        private async Task ShowCustomerDialogAsync(CustomerDto customer, bool isEdit)
        {
            TextBox fullNameTextBox = new TextBox
            {
                Header = "Họ Tên",
                Text = customer.Customer.Name,
                Width = 300
            };
            TextBox emailTextBox = new TextBox
            {
                Header = "Email",
                Text = customer.Customer.Email,
                Width = 300
            };
            TextBox phoneTextBox = new TextBox
            {
                Header = "Số điện thoại",
                Text = customer.Customer.Phone,
                Width = 300
            };
            ComboBox genderComboBox = new ComboBox
            {
                Header = "Giới tính",
                ItemsSource = new List<string> { "Male", "Female", "Unknown" },
                SelectedItem = customer.Customer.Gender,
                Width = 300
            };
            TextBox addressTextBox = new TextBox
            {
                Header = "Địa chỉ",
                Text = customer.Customer.Address,
                Width = 300
            };
            DatePicker birthDateBox = new DatePicker
            {
                Header = "Ngày sinh",
                Date = customer.Customer.BirthDate.HasValue ? new DateTimeOffset(customer.Customer.BirthDate.Value) : new DateTimeOffset(),
                Width = 300
            };

            StackPanel dialogContent = new StackPanel
            {
                Spacing = 10,
                Children = { fullNameTextBox, emailTextBox, phoneTextBox, addressTextBox, genderComboBox, birthDateBox }
            };

            ContentDialog dialog = new ContentDialog
            {
                Title = isEdit ? "Chỉnh sửa khách hàng" : "Thêm khách hàng",
                Content = dialogContent,
                PrimaryButtonText = isEdit ? "Lưu" : "Thêm",
                CloseButtonText = "Hủy",
                DefaultButton = ContentDialogButton.Primary,
                XamlRoot = this.XamlRoot
            };

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                if (string.IsNullOrWhiteSpace(fullNameTextBox.Text) ||
                    string.IsNullOrWhiteSpace(emailTextBox.Text) ||
                    string.IsNullOrWhiteSpace(phoneTextBox.Text) ||
                    string.IsNullOrEmpty(addressTextBox.Text))
                {
                    await ShowErrorDialogAsync("Vui lòng nhập đầy đủ thông tin.");
                    return;
                }

                customer.Customer.Name = fullNameTextBox.Text;
                customer.Customer.Email = emailTextBox.Text;
                customer.Customer.Phone = phoneTextBox.Text;
                customer.Customer.Address = addressTextBox.Text;
                customer.Customer.Gender = (string)genderComboBox.SelectedItem;
                customer.Customer.BirthDate = birthDateBox.Date.DateTime;

                string? success = isEdit
                    ? await ViewModel.UpdateCustomerAsync(customer)
                    : await ViewModel.AddCustomerAsync(customer);


                await ShowErrorDialogAsync(success);
            }
        }

        private async Task ShowErrorDialogAsync(string? message)
        {
            ContentDialog errorDialog = new ContentDialog
            {
                Title = "Thông báo",
                Content = message,
                CloseButtonText = "Đóng",
                XamlRoot = this.XamlRoot
            };

            await errorDialog.ShowAsync();
        }

        private async void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            var currentWindow = ((App)Application.Current).GetMainWindow();
            await ViewModel.ExportToExcelAsync(currentWindow);
        }


    }
}
