using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using csc13001_plant_pos.Model;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using Windows.Storage;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage.Streams;
using csc13001_plant_pos.ViewModel;
using Microsoft.UI.Xaml.Markup;

namespace csc13001_plant_pos.View
{
    public sealed partial class DetailProductPage : Page
    {
        public Product CurrentProduct { get; private set; }
        private bool _isEditMode = false;
        public DetailProductViewModel ViewModel { get; }
        public DetailProductPage()
        {
            this.DataContext = ViewModel = App.GetService<DetailProductViewModel>();
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is Product product)
            {
                CurrentProduct = product;
                this.DataContext = this;
            }
            ViewModel.LoadCategoryAsync();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
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
        private async void ShowProductDialogAsync(object sender, RoutedEventArgs e)
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
            <Style x:Key='FieldGroupHeaderStyle' TargetType='TextBlock'>
                <Setter Property='FontWeight' Value='SemiBold'/>
                <Setter Property='FontSize' Value='16'/>
                <Setter Property='Margin' Value='0,0,0,10'/>
            </Style>
            <Style TargetType='TextBox'>
                <Setter Property='Margin' Value='0,0,0,12'/>
                <Setter Property='HorizontalAlignment' Value='Stretch'/>
            </Style>
            <Style TargetType='ComboBox'>
                <Setter Property='Margin' Value='0,0,0,12'/>
                <Setter Property='HorizontalAlignment' Value='Stretch'/>
            </Style>
            <Style TargetType='RatingControl'>
                <Setter Property='Margin' Value='0,0,0,15'/>
                <Setter Property='MaxRating' Value='5'/>  
                <Setter Property='IsReadOnly' Value='False'/>
            </Style>
        </Grid.Resources>

        <ScrollViewer MaxHeight='600' VerticalScrollBarVisibility='Auto'>
            <Grid>
                <Grid.ColumnDefinitions>
                    <ColumnDefinition Width='*'/>
                    <ColumnDefinition Width='300'/>
                </Grid.ColumnDefinitions>

                <!-- Cột thông tin chính -->
                <StackPanel Grid.Column='0' Margin='0,0,20,0'>
                    <!-- Thông tin cơ bản -->
                    <TextBlock Text='Thông tin cơ bản' Style='{{StaticResource FieldGroupHeaderStyle}}'/>
                    
                    <TextBox x:Name='NameTextBox' Header='Tên sản phẩm' PlaceholderText='Nhập tên sản phẩm'/>
                    
                    <TextBox x:Name='DescriptionTextBox' 
                           Header='Mô tả sản phẩm' 
                           PlaceholderText='Nhập mô tả chi tiết về sản phẩm'
                           AcceptsReturn='True'
                           TextWrapping='Wrap'
                           Height='100'/>

                    <Grid Margin='0,0,0,12'>
                        <Grid.ColumnDefinitions>
                            <ColumnDefinition Width='*'/>
                            <ColumnDefinition Width='*'/>
                        </Grid.ColumnDefinitions>
                        
                        <TextBox x:Name='SalePriceTextBox' 
                               Grid.Column='0' 
                               Header='Giá bán (VND)' 
                               PlaceholderText='Nhập giá bán'
                               Margin='0,0,6,0'/>
                               
                        <TextBox x:Name='PurchasePriceTextBox' 
                               Grid.Column='1' 
                               Header='Giá nhập (VND)' 
                               PlaceholderText='Giá nhập'
                               IsEnabled='False'
                               Margin='6,0,0,0'/>
                    </Grid>
                    
                    <Grid Margin='0,0,0,12'>
                        <Grid.ColumnDefinitions>
                            <ColumnDefinition Width='*'/>
                            <ColumnDefinition Width='*'/>
                        </Grid.ColumnDefinitions>
                        
                        <TextBox x:Name='EnvironmentTypeTextBox' 
                               Grid.Column='0' 
                               Header='Loại môi trường' 
                               PlaceholderText='VD: Trong nhà/Ngoài trời'
                               Margin='0,0,6,0'/>
                               
                        <TextBox x:Name='StockTextBox' 
                               Grid.Column='1' 
                               Header='Số lượng tồn kho' 
                               IsEnabled='False'
                               Margin='6,0,0,0'/>
                    </Grid>
                    
                    <ComboBox x:Name='CategoryComboBox' 
                            Header='Phân loại sản phẩm'
                            PlaceholderText='Chọn phân loại'/>
                    
                    <!-- Đặc điểm sản phẩm -->
                    <Rectangle Height='1' Fill='{{ThemeResource DividerStrokeColorDefaultBrush}}' Margin='0,10,0,15'/>
                    
                    <TextBlock Text='Đặc điểm sản phẩm' Style='{{StaticResource FieldGroupHeaderStyle}}'/>

                    <TextBlock Text='Kích thước chậu' Margin='0,0,0,5'/>
                    <RatingControl x:Name='SizeRating'/>
                    
                    <TextBlock Text='Độ khó chăm sóc' Margin='0,0,0,5'/>
                    <RatingControl x:Name='CareLevelRating'/>
                    
                    <TextBlock Text='Yêu cầu ánh sáng' Margin='0,0,0,5'/>
                    <RatingControl x:Name='LightRequirementRating'/>
                    
                    <TextBlock Text='Nhu cầu nước' Margin='0,0,0,5'/>
                    <RatingControl x:Name='WateringScheduleRating'/>
                </StackPanel>
                
                <!-- Cột hình ảnh -->
                <StackPanel Grid.Column='1'>
                    <Border x:Name='ImageBorder'
                            Width='250' Height='250'
                            BorderThickness='1'
                            BorderBrush='{{ThemeResource CardStrokeColorDefaultBrush}}'
                            CornerRadius='8'>
                        <Grid>
                            <FontIcon x:Name='DefaultImageIcon'
                                    Glyph='&#xEB9F;'
                                    FontSize='50'
                                    Foreground='#99000000'
                                    HorizontalAlignment='Center'
                                    VerticalAlignment='Center'/>
                            
                            <Image x:Name='ProductImage'
                                 Stretch='UniformToFill'/>
                        </Grid>
                    </Border>
                    
                    <Button x:Name='SelectImageButton'
                            Content='Chọn ảnh sản phẩm'
                            HorizontalAlignment='Center'
                            Margin='0,15,0,0'/>
                    
                    <InfoBar Title='Định dạng ảnh'
                             IsOpen='True'
                             Severity='Informational'
                             Message='Hỗ trợ định dạng JPG và PNG'
                             Margin='0,15,0,0'/>
                </StackPanel>
            </Grid>
        </ScrollViewer>
    </Grid>";

            // Parse XAML để tạo UI
            dialogContent = XamlReader.Load(xamlContent) as Grid;

            // Lấy các tham chiếu đến điều khiển
            var nameTextBox = dialogContent.FindName("NameTextBox") as TextBox;
            var descriptionTextBox = dialogContent.FindName("DescriptionTextBox") as TextBox;
            var salePriceTextBox = dialogContent.FindName("SalePriceTextBox") as TextBox;
            var purchasePriceTextBox = dialogContent.FindName("PurchasePriceTextBox") as TextBox;
            var stockTextBox = dialogContent.FindName("StockTextBox") as TextBox;
            var environmentTypeTextBox = dialogContent.FindName("EnvironmentTypeTextBox") as TextBox;
            var categoryComboBox = dialogContent.FindName("CategoryComboBox") as ComboBox;

            var sizeRating = dialogContent.FindName("SizeRating") as RatingControl;
            var careLevelRating = dialogContent.FindName("CareLevelRating") as RatingControl;
            var lightRequirementRating = dialogContent.FindName("LightRequirementRating") as RatingControl;
            var wateringScheduleRating = dialogContent.FindName("WateringScheduleRating") as RatingControl;

            var selectImageButton = dialogContent.FindName("SelectImageButton") as Button;
            var productImage = dialogContent.FindName("ProductImage") as Image;
            var defaultImageIcon = dialogContent.FindName("DefaultImageIcon") as FontIcon;

            // Thiết lập dữ liệu
            nameTextBox.Text = CurrentProduct.Name;
            descriptionTextBox.Text = CurrentProduct.Description;
            salePriceTextBox.Text = CurrentProduct.SalePrice.ToString();
            purchasePriceTextBox.Text = CurrentProduct.PurchasePrice.ToString();
            stockTextBox.Text = CurrentProduct.Stock.ToString();
            environmentTypeTextBox.Text = CurrentProduct.EnvironmentType;

            // Thiết lập ComboBox Category
            categoryComboBox.ItemsSource = ViewModel.Categories;
            categoryComboBox.DisplayMemberPath = "Name";
            categoryComboBox.SelectedItem = ViewModel.Categories.FirstOrDefault(c => c.CategoryId == CurrentProduct.Category.CategoryId);

            // Thiết lập Rating Controls thay cho ComboBox
            sizeRating.Value = CurrentProduct.Size;
            careLevelRating.Value = CurrentProduct.CareLevel;
            lightRequirementRating.Value = CurrentProduct.LightRequirement;
            wateringScheduleRating.Value = CurrentProduct.WateringSchedule;

            // Cài đặt hiển thị ảnh
            if (!string.IsNullOrEmpty(CurrentProduct.ImageUrl))
            {
                productImage.Source = new BitmapImage(new Uri(CurrentProduct.ImageUrl));
                defaultImageIcon.Visibility = Visibility.Collapsed;
            }
            else
            {
                defaultImageIcon.Visibility = Visibility.Visible;
            }

            // Xử lý sự kiện chọn ảnh
            selectImageButton.Click += async (s, args) =>
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
                    productImage.Source = bitmap;
                    defaultImageIcon.Visibility = Visibility.Collapsed;
                }
            };

            // Tạo hộp thoại với giao diện đã cải thiện
            ContentDialog dialog = new ContentDialog
            {
                Title = "Chỉnh sửa sản phẩm",
                Content = dialogContent,
                PrimaryButtonText = "Lưu thay đổi",
                CloseButtonText = "Hủy bỏ",
                DefaultButton = ContentDialogButton.Primary,
                XamlRoot = this.XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                PrimaryButtonStyle = Application.Current.Resources["AccentButtonStyle"] as Style,
                MinWidth = 800
            };

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(nameTextBox.Text) ||
                    string.IsNullOrWhiteSpace(descriptionTextBox.Text) ||
                    !decimal.TryParse(salePriceTextBox.Text, out decimal salePrice) ||
                    categoryComboBox.SelectedItem == null)
                {
                    await ShowErrorDialogAsync("Vui lòng nhập đầy đủ thông tin cần thiết.");
                    return;
                }

                // Update product
                CurrentProduct.Name = nameTextBox.Text;
                CurrentProduct.Description = descriptionTextBox.Text;
                CurrentProduct.SalePrice = salePrice;
                CurrentProduct.EnvironmentType = environmentTypeTextBox.Text;
                CurrentProduct.Size = (int)sizeRating.Value;
                CurrentProduct.CareLevel = (int)careLevelRating.Value;
                CurrentProduct.LightRequirement = (int)lightRequirementRating.Value;
                CurrentProduct.WateringSchedule = (int)wateringScheduleRating.Value;
                CurrentProduct.Category = categoryComboBox.SelectedItem as Category;

                    // Lưu thay đổi
                    bool updateResult = await ViewModel.UpdateProductAsync(CurrentProduct, selectedFile);


                    if (updateResult)
                    {
                        var successDialog = new ContentDialog
                        {
                            Title = "Thành công",
                            Content = "Đã cập nhật thông tin sản phẩm thành công.",
                            CloseButtonText = "Đóng",
                            XamlRoot = this.XamlRoot
                        };
                        await successDialog.ShowAsync();
                    }
                    else
                    {
                        await ShowErrorDialogAsync("Không thể cập nhật sản phẩm. Vui lòng thử lại sau.");
                    }
            }
        }
        private void SaveChanges()
        {
            // Lưu các thay đổi vào đối tượng CurrentProduct
            // Các thuộc tính đã được binding với Mode=TwoWay nên đã tự động cập nhật

            // Chỉ cần cập nhật chiều cao vì nó không binding
            // và có thể thêm code lưu xuống database ở đây

            // Hiển thị thông báo thành công
            ShowSaveSuccessMessage();
        }

        private async void ShowSaveSuccessMessage()
        {
            ContentDialog dialog = new ContentDialog()
            {
                Title = "Thành công",
                Content = "Thông tin sản phẩm đã được cập nhật thành công.",
                CloseButtonText = "Đóng"
            };

            await dialog.ShowAsync();
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
    }
}