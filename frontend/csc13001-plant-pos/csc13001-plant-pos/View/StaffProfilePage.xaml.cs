using Microsoft.UI.Xaml.Controls;
using csc13001_plant_pos.ViewModel;
using csc13001_plant_pos.DTO.OrderDTO;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.Storage;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using Windows.UI;


namespace csc13001_plant_pos.View
{
    public sealed partial class StaffProfilePage : Page
    {
        public StaffProfileViewModel ViewModel { get; }
        public StaffProfilePage()
        {
            this.InitializeComponent();
            ViewModel = App.GetService<StaffProfileViewModel>();
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
        private async Task<string> ConvertImageToBase64Async(StorageFile file)
        {
            var stream = await file.OpenReadAsync();
            var bytes = new byte[stream.Size];
            await stream.ReadAsync(bytes.AsBuffer(), (uint)stream.Size, InputStreamOptions.None);

            string base64 = Convert.ToBase64String(bytes);

            var contentType = file.ContentType;

            return $"data:{contentType};base64,{base64}";
        }

        private async void ShowStaffDialogAsync(object sender, RoutedEventArgs e)
        {
            string image64 = null;
            StorageFile selectedFile = null;

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
            var selectImageButton = dialogContent.FindName("SelectImageButton") as Button;
            var staffImage = dialogContent.FindName("StaffImage") as Image;
            var defaultPersonIcon = dialogContent.FindName("DefaultPersonIcon") as FontIcon;

            // Thiết lập dữ liệu
            fullNameTextBox.Text = ViewModel.StaffUser.Fullname;
            emailTextBox.Text = ViewModel.StaffUser.Email;
            phoneTextBox.Text = ViewModel.StaffUser.Phone;

            statusComboBox.ItemsSource = new List<string> { "Working", "OnLeave", "Resigned" };
            statusComboBox.SelectedItem = ViewModel.StaffUser.Status;

            genderComboBox.ItemsSource = new List<string> { "Male", "Female" };
            genderComboBox.SelectedItem = ViewModel.StaffUser.Gender;


            // Cài đặt hiển thị ảnh
            if (!string.IsNullOrEmpty(ViewModel.StaffUser.ImageUrl))
            {
                staffImage.Source = new BitmapImage(new Uri(ViewModel.StaffUser.ImageUrl));
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
                Title = "Chỉnh sửa thông tin",
                Content = dialogContent,
                PrimaryButtonText = "Lưu thay đổi",
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

                ViewModel.StaffUser.Fullname = fullNameTextBox.Text;
                ViewModel.StaffUser.Email = emailTextBox.Text;
                ViewModel.StaffUser.Phone = phoneTextBox.Text;
                ViewModel.StaffUser.Status = (string)statusComboBox.SelectedItem;
                ViewModel.StaffUser.Gender = (string)genderComboBox.SelectedItem;

                bool success = await ViewModel.UpdateStaffAsync(ViewModel.StaffUser, selectedFile);

                if (success)
                {
                    var successDialog = new ContentDialog
                    {
                        Title = "Thành công",
                        Content = "Đã cập nhật thông tin nhân viên.",
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
        private async void ShowWorkLogListDialogAsync(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var user = ViewModel.StaffUser;
            if (user?.WorkLogs == null || user.WorkLogs.Count == 0)
            {
                await ShowErrorDialogAsync("Không có thông tin để hiển thị.");
                return;
            }


            // Tạo container chính cho nội dung
            StackPanel dialogContent = new StackPanel { Spacing = 12 };

            // Định dạng lại thời gian và hiển thị
            foreach (var worklog in user.WorkLogs)
            {
                // Định dạng thời gian đăng nhập
                DateTime loginTime = DateTime.Parse(worklog.LogInTime);
                string dayOfWeek = GetVietnameseDayOfWeek(loginTime.DayOfWeek);
                string loginDate = loginTime.ToString("dd/MM/yyyy");
                string loginTimeStr = loginTime.ToString("HH:mm");
                string formattedLoginTime = $"{dayOfWeek}, ngày {loginDate}, vào lúc {loginTimeStr}";

                // Định dạng thời gian đăng xuất
                DateTime logoutTime = DateTime.Parse(worklog.LogOutTime);
                string logoutTimeStr = logoutTime.ToString("HH:mm");

                // Nếu cùng ngày thì chỉ hiển thị giờ, nếu khác ngày thì hiển thị cả ngày
                string formattedLogoutTime;
                if (loginTime.Date == logoutTime.Date)
                {
                    formattedLogoutTime = $"lúc {logoutTimeStr} cùng ngày";
                }
                else
                {
                    string logoutDayOfWeek = GetVietnameseDayOfWeek(logoutTime.DayOfWeek);
                    string logoutDate = logoutTime.ToString("dd/MM/yyyy");
                    formattedLogoutTime = $"{logoutDayOfWeek}, ngày {logoutDate}, lúc {logoutTimeStr}";
                }

                // Định dạng thời gian làm việc
                string[] durationParts = worklog.WorkDuration.Split(':');
                string formattedDuration;

                if (durationParts.Length == 3)
                {
                    int hours = int.Parse(durationParts[0]);
                    int minutes = int.Parse(durationParts[1]);
                    int seconds = int.Parse(durationParts[2]);

                    if (hours > 0)
                    {
                        formattedDuration = $"{hours} giờ {minutes} phút {seconds} giây";
                    }
                    else if (minutes > 0)
                    {
                        formattedDuration = $"{minutes} phút {seconds} giây";
                    }
                    else
                    {
                        formattedDuration = $"{seconds} giây";
                    }
                }
                else
                {
                    formattedDuration = worklog.WorkDuration;
                }

                // Tạo card hiển thị thông tin
                var grid = new Grid();
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(32) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                // Thời gian đăng nhập
                var loginIcon = new FontIcon
                {
                    Glyph = "\uE823",  // Biểu tượng đồng hồ (Clock)
                    FontSize = 16,
                    VerticalAlignment = VerticalAlignment.Center,
                    Foreground = new SolidColorBrush(Colors.DarkSlateBlue)
                };
                Grid.SetRow(loginIcon, 0);
                Grid.SetColumn(loginIcon, 0);

                var loginText = new TextBlock
                {
                    Text = $"Đăng nhập: {formattedLoginTime}",
                    VerticalAlignment = VerticalAlignment.Center,
                    FontSize = 14,
                    Margin = new Thickness(0, 6, 0, 6),
                    TextWrapping = TextWrapping.Wrap
                };
                Grid.SetRow(loginText, 0);
                Grid.SetColumn(loginText, 1);

                // Thời gian đăng xuất
                var logoutIcon = new FontIcon
                {
                    Glyph = "\uE8DE",  // Biểu tượng kết thúc (Timeout)
                    FontSize = 16,
                    VerticalAlignment = VerticalAlignment.Center,
                    Foreground = new SolidColorBrush(Colors.DarkRed)
                };
                Grid.SetRow(logoutIcon, 1);
                Grid.SetColumn(logoutIcon, 0);

                var logoutText = new TextBlock
                {
                    Text = $"Đăng xuất: {formattedLogoutTime}",
                    VerticalAlignment = VerticalAlignment.Center,
                    FontSize = 14,
                    Margin = new Thickness(0, 6, 0, 6),
                    TextWrapping = TextWrapping.Wrap
                };
                Grid.SetRow(logoutText, 1);
                Grid.SetColumn(logoutText, 1);

                // Thời gian làm việc
                var durationIcon = new FontIcon
                {
                    Glyph = "\uEC92",  // Biểu tượng đồng hồ cát (timer)
                    FontSize = 16,
                    VerticalAlignment = VerticalAlignment.Center,
                    Foreground = new SolidColorBrush(Colors.DarkGreen)
                };
                Grid.SetRow(durationIcon, 2);
                Grid.SetColumn(durationIcon, 0);

                var durationText = new TextBlock
                {
                    Text = $"Đã làm việc: {formattedDuration}",
                    VerticalAlignment = VerticalAlignment.Center,
                    FontWeight = FontWeights.SemiBold,
                    FontSize = 14,
                    Margin = new Thickness(0, 6, 0, 6)
                };
                Grid.SetRow(durationText, 2);
                Grid.SetColumn(durationText, 1);

                grid.Children.Add(loginIcon);
                grid.Children.Add(loginText);
                grid.Children.Add(logoutIcon);
                grid.Children.Add(logoutText);
                grid.Children.Add(durationIcon);
                grid.Children.Add(durationText);

                var border = new Border
                {
                    BorderThickness = new Thickness(1),
                    BorderBrush = new SolidColorBrush(Colors.LightGray),
                    Padding = new Thickness(12),
                    CornerRadius = new CornerRadius(8),
                    Background = new SolidColorBrush(Color.FromArgb(15, 0, 0, 0)),
                    Child = grid
                };

                dialogContent.Children.Add(border);
            }

            // Tạo và hiển thị dialog
            ContentDialog dialog = new ContentDialog
            {
                Title = $"Lịch sử làm việc của {user.Fullname}",
                Content = new ScrollViewer
                {
                    Content = dialogContent,
                    VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                    HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                    Padding = new Thickness(16),
                    Height = 450,
                    Width = 500 // Tăng độ rộng để hiển thị tốt hơn
                },
                CloseButtonText = "Đóng",
                XamlRoot = this.XamlRoot,
                DefaultButton = ContentDialogButton.Close
            };

            await dialog.ShowAsync();
        }
        private string GetVietnameseDayOfWeek(DayOfWeek day)
        {
            switch (day)
            {
                case DayOfWeek.Monday:
                    return "Thứ hai";
                case DayOfWeek.Tuesday:
                    return "Thứ ba";
                case DayOfWeek.Wednesday:
                    return "Thứ tư";
                case DayOfWeek.Thursday:
                    return "Thứ năm";
                case DayOfWeek.Friday:
                    return "Thứ sáu";
                case DayOfWeek.Saturday:
                    return "Thứ bảy";
                case DayOfWeek.Sunday:
                    return "Chủ nhật";
                default:
                    return "";
            }
        }
    }
}
