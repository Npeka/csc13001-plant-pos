using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using csc13001_plant_pos.Model;
using csc13001_plant_pos.ViewModel;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;

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
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.LoadStaffsDataAsync();
        }
        private async void ShowWorkLogListDialogAsync(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var user = button?.Tag as csc13001_plant_pos.Model.User;
            if (user?.WorkLogs == null || user.WorkLogs.Count == 0)
            {
                await ShowErrorDialogAsync("Không có thông tin để hiển thị.");
                return;
            }

            StackPanel dialogContent = new StackPanel { Spacing = 8 };

            foreach (var worklog in user.WorkLogs)
            {
                var border = new Border
                {
                    BorderThickness = new Thickness(1),
                    BorderBrush = new SolidColorBrush(Colors.Gray),
                    Padding = new Thickness(8),
                    CornerRadius = new CornerRadius(4),
                    Child = new StackPanel
                    {
                        Children =
                {
                    new TextBlock { Text = $"🕒 Đăng nhập: {worklog.LogInTime}" },
                    new TextBlock { Text = $"🕘 Đăng xuất: {worklog.LogOutTime}" },
                    new TextBlock { Text = $"⏱️ Thời gian làm việc: {worklog.WorkDuration}" }
                }
                    }
                };

                dialogContent.Children.Add(border);
            }

            ContentDialog dialog = new ContentDialog
            {
                Title = "Lịch sử làm việc của nhân viên",
                Content = new ScrollViewer
                {
                    Content = dialogContent,
                    VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                    Height = 400,
                    Width = 400
                },
                CloseButtonText = "Đóng",
                XamlRoot = this.XamlRoot
            };

            await dialog.ShowAsync();
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

        public void ViewStaff(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var staff = button?.Tag as User;
            if (staff != null)
            {
                Frame.Navigate(typeof(StaffProfilePage), staff.UserId.ToString());
            }
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
            string username = "";
            string password = "";

            // Tạo Dialog bằng XAML để có giao diện nhất quán và dễ bảo trì
            var dialogContent = new Grid();

            // Tạo nội dung XAML
            var xamlContent = $@"
    <Grid xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'
          xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'>
        <Grid.Resources>
            <Style TargetType='TextBox'>
                <Setter Property='Margin' Value='0,0,0,12' />
                <Setter Property='Width' Value='300' />
                <Setter Property='HorizontalAlignment' Value='Stretch' />
            </Style>
            <Style TargetType='ComboBox'>
                <Setter Property='Margin' Value='0,0,0,12' />
                <Setter Property='Width' Value='300' />
                <Setter Property='HorizontalAlignment' Value='Stretch' />
            </Style>
            <Style TargetType='ToggleSwitch'>
                <Setter Property='Margin' Value='0,0,0,12' />
                <Setter Property='Width' Value='300' />
                <Setter Property='HorizontalAlignment' Value='Stretch' />
            </Style>
        </Grid.Resources>

        <ScrollViewer MaxHeight='500' VerticalScrollBarVisibility='Auto'>
            <StackPanel Spacing='12' Margin='0,0,0,20'>
                <!-- Profile Image Section -->
                <Grid HorizontalAlignment='Center' Margin='0,0,0,15'>
                    <Grid.RowDefinitions>
                        <RowDefinition Height='Auto'/>
                        <RowDefinition Height='Auto'/>
                    </Grid.RowDefinitions>
                    
                    <Border x:Name='ImageBorder' 
                            Grid.Row='0'
                            Width='120' Height='120' 
                            BorderThickness='2'
                            BorderBrush='{{ThemeResource SystemAccentColor}}'
                            CornerRadius='60'>
                        <Border.Background>
                            <SolidColorBrush Color='#FFEEEEEE'/>
                        </Border.Background>
                        
                        <Grid>
                            <FontIcon x:Name='DefaultPersonIcon' 
                                      Glyph='&#xE77B;' 
                                      FontSize='50' 
                                      Foreground='#99000000'
                                      HorizontalAlignment='Center' 
                                      VerticalAlignment='Center'/>
                            
                            <Image x:Name='StaffImage' 
                                   Stretch='UniformToFill'/>
                        </Grid>
                    </Border>
                    
                    <Button x:Name='SelectImageButton'
                            Grid.Row='1'
                            Content='Chọn ảnh'
                            Margin='0,10,0,0'
                            HorizontalAlignment='Center'/>
                </Grid>
                
                <!-- Divider -->
                <Rectangle Height='1' Fill='{{ThemeResource SystemBaseLowColor}}' Margin='0,0,0,10'/>
                
                <!-- Info Section -->
                <TextBlock Text='Thông tin cá nhân' 
                           FontWeight='SemiBold' 
                           FontSize='16' 
                           Margin='0,0,0,10'/>
                
            <TextBox x:Name='UsernameTextBox' 
                                 Header='Tên đăng nhập' 
                                 PlaceholderText='Nhập tên đăng nhập'
                                 Visibility='{(isEdit ? "Collapsed" : "Visible")}'/>
            
            <TextBox x:Name='PasswordTextBox' 
                     Header='Mật khẩu' 
                     PlaceholderText='Nhập mật khẩu'
                     Visibility='{(isEdit ? "Collapsed" : "Visible")}'
                    />
                <TextBox x:Name='FullNameTextBox' 
                         Header='Họ Tên' 
                         PlaceholderText='Nhập họ tên đầy đủ'/>
                
                    
                    <ComboBox x:Name='GenderComboBox' 
                              Header='Giới tính'
                              Margin='0,0,5,12'/>
                    
                    <ComboBox x:Name='StatusComboBox'
                              Header='Trạng thái'
                              Margin='5,0,0,12'/>
                
                <TextBox x:Name='EmailTextBox' 
                         Header='Email' 
                         PlaceholderText='example@domain.com'/>
                
                <TextBox x:Name='PhoneTextBox' 
                         Header='Số điện thoại' 
                         PlaceholderText='Nhập số điện thoại'/>
                
                <!-- Divider -->
                <Rectangle Height='1' Fill='{{ThemeResource SystemBaseLowColor}}' Margin='0,10,0,10'/>
                
                <!-- Permissions Section -->
                <TextBlock Text='Phân quyền' 
                           FontWeight='SemiBold' 
                           FontSize='16' 
                           Margin='0,0,0,10'/>
                
                <ToggleSwitch x:Name='CanManageDiscountsToggleSwitch' 
                              Header='Quyền quản lý giảm giá'
                              OffContent='Không'
                              OnContent='Có'/>
                
                <ToggleSwitch x:Name='CanManageInventoryToggleSwitch' 
                              Header='Quyền quản lý kho'
                              OffContent='Không'
                              OnContent='Có'/>
            </StackPanel>
        </ScrollViewer>
    </Grid>";

            // Parse XAML để tạo UI
            dialogContent = XamlReader.Load(xamlContent) as Grid;

            // Lấy các tham chiếu đến điều khiển
            var fullNameTextBox = dialogContent.FindName("FullNameTextBox") as TextBox;
            var emailTextBox = dialogContent.FindName("EmailTextBox") as TextBox;
            var phoneTextBox = dialogContent.FindName("PhoneTextBox") as TextBox;
            var statusComboBox = dialogContent.FindName("StatusComboBox") as ComboBox;
            var genderComboBox = dialogContent.FindName("GenderComboBox") as ComboBox;
            var canManageDiscountsToggleSwitch = dialogContent.FindName("CanManageDiscountsToggleSwitch") as ToggleSwitch;
            var canManageInventoryToggleSwitch = dialogContent.FindName("CanManageInventoryToggleSwitch") as ToggleSwitch;
            var selectImageButton = dialogContent.FindName("SelectImageButton") as Button;
            var staffImage = dialogContent.FindName("StaffImage") as Image;
            var defaultPersonIcon = dialogContent.FindName("DefaultPersonIcon") as FontIcon;
            var usernameTextBox = dialogContent.FindName("UsernameTextBox") as TextBox;
            var passwordTextBox = dialogContent.FindName("PasswordTextBox") as TextBox;

            // Thiết lập dữ liệu
            fullNameTextBox.Text = user.Fullname;
            emailTextBox.Text = user.Email;
            phoneTextBox.Text = user.Phone;

            statusComboBox.ItemsSource = new List<string> { "Working", "OnLeave", "Resigned" };
            statusComboBox.SelectedItem = user.Status;

            genderComboBox.ItemsSource = new List<string> { "Male", "Female" };
            genderComboBox.SelectedItem = user.Gender;

            canManageDiscountsToggleSwitch.IsOn = user.CanManageDiscounts;
            canManageInventoryToggleSwitch.IsOn = user.CanManageInventory;


            // Cài đặt hiển thị ảnh
            if (!string.IsNullOrEmpty(user.ImageUrl))
            {
                staffImage.Source = new BitmapImage(new Uri(user.ImageUrl));
                defaultPersonIcon.Visibility = Visibility.Collapsed;
            }
            else
            {
                defaultPersonIcon.Visibility = Visibility.Visible;
            }

            // Xử lý sự kiện chọn ảnh
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
                    await bitmap.SetSourceAsync(stream);
                    staffImage.Source = bitmap;
                    defaultPersonIcon.Visibility = Visibility.Collapsed;
                }
            };

            // Tạo hộp thoại với giao diện đã cải thiện
            ContentDialog dialog = new ContentDialog
            {
                Title = isEdit ? "Chỉnh sửa nhân viên" : "Thêm nhân viên",
                Content = dialogContent,
                PrimaryButtonText = isEdit ? "Lưu thay đổi" : "Thêm nhân viên",
                CloseButtonText = "Hủy",
                DefaultButton = ContentDialogButton.Primary,
                XamlRoot = this.XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                PrimaryButtonStyle = Application.Current.Resources["AccentButtonStyle"] as Style
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
                if (!isEdit)
                {
                    if (string.IsNullOrWhiteSpace(usernameTextBox.Text) ||
                        string.IsNullOrWhiteSpace(passwordTextBox.Text))
                    {
                        await ShowErrorDialogAsync("Vui lòng nhập tên đăng nhập và mật khẩu.");
                        return;
                    }
                }
                if (!isEdit)
                {
                    username = usernameTextBox.Text;
                    password = passwordTextBox.Text;
                }
                if (!IsValidEmail(emailTextBox.Text))
                {
                    await ShowErrorDialogAsync("Email không hợp lệ.");
                    return;
                }
                if (!IsValidPhoneNumber(phoneTextBox.Text))
                {
                    await ShowErrorDialogAsync("Số điện thoại không hợp lệ.");
                    return;
                }

                user.Fullname = fullNameTextBox.Text;
                user.Email = emailTextBox.Text;
                user.Phone = phoneTextBox.Text;
                user.Status = (string)statusComboBox.SelectedItem;
                user.Gender = (string)genderComboBox.SelectedItem;
                user.CanManageDiscounts = canManageDiscountsToggleSwitch.IsOn;
                user.CanManageInventory = canManageInventoryToggleSwitch.IsOn;

                bool success = isEdit
                    ? await ViewModel.UpdateStaffAsync(user, selectedFile)
                    : await ViewModel.AddStaffAsync(user, selectedFile, username, password);

                if (success)
                {
                    var successDialog = new ContentDialog
                    {
                        Title = "Thành công",
                        Content = isEdit ? "Đã cập nhật thông tin nhân viên." : "Đã thêm nhân viên mới.",
                        CloseButtonText = "Đóng",
                        XamlRoot = this.XamlRoot,
                        Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style
                    };
                    await successDialog.ShowAsync();
                }
                else
                {
                    await ShowErrorDialogAsync("Không thể lưu thông tin nhân viên. Vui lòng thử lại.");
                }
            }
        }
        private async Task ShowErrorDialogAsync(string message)
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

        public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public bool IsValidPhoneNumber(string phone)
        {
            // Số điện thoại Việt Nam bắt đầu bằng 0 và có 10 chữ số
            return System.Text.RegularExpressions.Regex.IsMatch(phone, @"^0\d{9}$");
        }

    }

}
