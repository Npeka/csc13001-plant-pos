using System.Collections.Generic;
using System.Diagnostics;
using csc13001_plant_pos.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Threading.Tasks;
using Windows.Foundation;
using System;
using static SkiaSharp.HarfBuzz.SKShaper;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Storage.Pickers;
using Windows.Storage;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage.Streams;
using Windows.ApplicationModel.Contacts;

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

        private async Task<string> ConvertImageToBase64Async(StorageFile file)
        {
            var stream = await file.OpenReadAsync();
            var bytes = new byte[stream.Size];
            await stream.ReadAsync(bytes.AsBuffer(), (uint)stream.Size, InputStreamOptions.None);

            string base64 = Convert.ToBase64String(bytes);

            var contentType = file.ContentType;

            return $"data:{contentType};base64,{base64}";
        }

        private async Task ShowStaffDialogAsync(csc13001_plant_pos.Model.User user, bool isEdit)
        {
           string image64 = null;
            StorageFile selectedFile = null;
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
                ItemsSource = new List<string> { "Male", "Female"},
                SelectedItem = user.Gender,
                Width = 300
            };
            ToggleSwitch isAdminToggleSwitch = new ToggleSwitch
            {
                Header = "Quyền quản trị",
                IsOn = user.IsAdmin,
                Width = 300,
                IsEnabled = false
            };

            ToggleSwitch canManageDiscounts = new ToggleSwitch
            {
                Header = "Quyền quản lý giảm giá",
                IsOn = user.CanManageDiscounts,
                Width = 300
            };

            ToggleSwitch canManageInventory = new ToggleSwitch
            {
                Header = "Quyền quản lý kho",
                IsOn = user.CanManageInventory,
                Width = 300
            };

            Button selectImageButton = new Button
            {
                Content = "Chọn ảnh",
                Width = 100
            };
            Image selectedImagePreview = new Image
            {
                Width = 100,
                Height = 100,
                Margin = new Thickness(0, 10, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center,
                Source = string.IsNullOrEmpty(user.ImageUrl) ? null : new BitmapImage(new Uri(user.ImageUrl))
            };

            selectImageButton.Click += async (sender, e) =>
            {

                var picker = new FileOpenPicker();
                picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                picker.FileTypeFilter.Add(".jpg");
                picker.FileTypeFilter.Add(".png");
                var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(((App)Application.Current).GetMainWindow());
                WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

                var file = await picker.PickSingleFileAsync();
                if (file != null)
                {
                    selectedFile = file;
                    image64 = await ConvertImageToBase64Async(file);
                    var stream = await file.OpenAsync(FileAccessMode.Read);
                    var bitmap = new BitmapImage();
                    bitmap.SetSource(stream);
                    selectedImagePreview.Source = bitmap;

                }
            };

            StackPanel dialogContent = new StackPanel
            {
                Spacing = 10,
                Children = { fullNameTextBox, emailTextBox, phoneTextBox, statusComboBox, genderComboBox, isAdminToggleSwitch, canManageDiscounts, canManageInventory, selectImageButton, selectedImagePreview }
            };
            var scrollViewer = new ScrollViewer
            {
                Content = dialogContent,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                MaxHeight = 700
            };
            ContentDialog dialog = new ContentDialog
            {
                Title = isEdit ? "Chỉnh sửa nhân viên" : "Thêm nhân viên",
                Content = scrollViewer,
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
                user.CanManageDiscounts = canManageDiscounts.IsOn;
                user.CanManageInventory = canManageInventory.IsOn;
                bool success = isEdit
                    ? await ViewModel.UpdateStaffAsync(user, selectedFile)
                    : await ViewModel.AddStaffAsync(user, selectedFile);

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

        private async void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            var currentWindow = ((App)Application.Current).GetMainWindow();
            await ViewModel.ExportToExcelAsync(currentWindow);
        }
    }

}
