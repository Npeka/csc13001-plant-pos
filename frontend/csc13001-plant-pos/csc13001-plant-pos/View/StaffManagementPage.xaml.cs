using System.Collections.Generic;
using System.Diagnostics;
using csc13001_plant_pos.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Threading.Tasks;
using Windows.Foundation;
using System;
using static SkiaSharp.HarfBuzz.SKShaper;

namespace csc13001_plant_pos.View
{
    public sealed partial class StaffManagementPage : Page
    {
        public StaffManagementViewModel ViewModel { get; }

        public StaffManagementPage()
        {
            this.DataContext = ViewModel = App.GetService<StaffManagementViewModel>();
            this.InitializeComponent();
        }

        public async void AddNewStaff(object sender, RoutedEventArgs e)
        {
            var newUser = new csc13001_plant_pos.Model.User
            {
                Fullname = "",
                Email = "",
                Phone = "",
                Status = "Working",
                Gender = "Male",
                IsAdmin = false,
                StartDate = DateTime.Now
            };

            await ShowStaffDialogAsync(newUser, false);
        }

        public async void EditStaffInformation(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var user = button?.Tag as csc13001_plant_pos.Model.User;

            if (user == null)
            {
                return;
            }

            await ShowStaffDialogAsync(user, true);
        }

        private async Task ShowStaffDialogAsync(csc13001_plant_pos.Model.User user, bool isEdit)
        {
            TextBox fullNameTextBox = new TextBox
            {
                Header = "Họ Tên",
                Text = user.Fullname,
                Width = 300
            };
            TextBox emailTextBox = new TextBox
            {
                Header = "Email",
                Text = user.Email,
                Width = 300
            };
            TextBox phoneTextBox = new TextBox
            {
                Header = "Số điện thoại",
                Text = user.Phone,
                Width = 300
            };
            ComboBox statusComboBox = new ComboBox
            {
                Header = "Trạng thái",
                ItemsSource = new List<string> { "Working", "OnLeave", "Resigned" },
                SelectedItem = user.Status,
                Width = 300
            };
            ComboBox genderComboBox = new ComboBox
            {
                Header = "Giới tính",
                ItemsSource = new List<string> { "Male", "Female", "Unknown" },
                SelectedItem = user.Gender,
                Width = 300
            };
            ToggleSwitch isAdminToggleSwitch = new ToggleSwitch
            {
                Header = "Quyền quản trị",
                IsOn = user.IsAdmin,
                Width = 300
            };

            StackPanel dialogContent = new StackPanel
            {
                Spacing = 10,
                Children = { fullNameTextBox, emailTextBox, phoneTextBox, statusComboBox, genderComboBox, isAdminToggleSwitch }
            };

            ContentDialog dialog = new ContentDialog
            {
                Title = isEdit ? "Chỉnh sửa nhân viên" : "Thêm nhân viên",
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
                    statusComboBox.SelectedItem == null ||
                    genderComboBox.SelectedItem == null)
                {
                    await ShowErrorDialogAsync("Vui lòng nhập đầy đủ thông tin.");
                    return;
                }

                user.Fullname = fullNameTextBox.Text;
                user.Email = emailTextBox.Text;
                user.Phone = phoneTextBox.Text;
                user.Status = (string)statusComboBox.SelectedItem;
                user.Gender = (string)genderComboBox.SelectedItem;
                user.IsAdmin = isAdminToggleSwitch.IsOn;

                bool success = isEdit
                    ? await ViewModel.UpdateStaffAsync(user) // Assume UpdateStaffAsync is implemented in ViewModel
                    : await ViewModel.AddStaffAsync(user);   // Assume AddStaffAsync is implemented in ViewModel

                if (!success)
                {
                    await ShowErrorDialogAsync("Không thể lưu nhân viên. Vui lòng thử lại.");
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
