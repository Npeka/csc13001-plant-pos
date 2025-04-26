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

                string success = await ViewModel.UpdateStaffAsync(ViewModel.StaffUser, selectedFile);

               
                    await ShowErrorDialogAsync(success);

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

    }
}
