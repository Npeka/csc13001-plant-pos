using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using csc13001_plant_pos.Model;
using csc13001_plant_pos.ViewModel;
using csc13001_plant_pos.DTO.CustomerDTO;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Linq;
using Windows.Storage.Pickers;
using Windows.Storage;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage.Streams;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace csc13001_plant_pos.View
{
    public sealed partial class ProductManagementPage : Page
    {
        public ProductManagementViewModel ViewModel { get; }

        public ProductManagementPage()
        {
            this.DataContext = ViewModel = App.GetService<ProductManagementViewModel>();
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.LoadProductsDataAsync();
        }
        private void ProductGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var selectedProduct = e.ClickedItem as Product;
            if (selectedProduct != null)
            {
                Frame.Navigate(typeof(DetailProductPage), selectedProduct);
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
        private async void ShowCategoryListDialogAsync(object sender, RoutedEventArgs e)
        {
            if (ViewModel?.Categories == null || ViewModel.Categories.Count == 0)
            {
                await ShowErrorDialogAsync("Không có danh mục nào để hiển thị.");
                return;
            }

            StackPanel dialogContent = new StackPanel { Spacing = 8 };
            TaskCompletionSource<Category> categorySelectionSource = new();

            foreach (var category in ViewModel.Categories)
            {
                var button = new Button
                {
                    Content = category.Name,
                    Tag = category,
                    HorizontalAlignment = HorizontalAlignment.Stretch
                };
                button.Click += (s, e) =>
                {
                    var selectedButton = s as Button;
                    var selectedCategory = selectedButton?.Tag as Category;
                    if (selectedCategory != null)
                    {
                        categorySelectionSource.TrySetResult(selectedCategory);
                    }
                };

                dialogContent.Children.Add(button);
            }

            ContentDialog dialog = new ContentDialog
            {
                Title = "Chọn danh mục cần chỉnh sửa",
                Content = new ScrollViewer
                {
                    Content = dialogContent,
                    VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                    Height = 400
                },
                CloseButtonText = "Đóng",
                XamlRoot = this.XamlRoot
            };

            var dialogTask = dialog.ShowAsync().AsTask();
            var completedTask = await Task.WhenAny(dialogTask, categorySelectionSource.Task);

            if (completedTask == categorySelectionSource.Task)
            {
                dialog.Hide();
                var selectedCategory = categorySelectionSource.Task.Result;
                await ShowCategoryDialogAsync(selectedCategory, true);
            }
        }

        public async void AddNewCategory(object sender, RoutedEventArgs e)
        {
            var newCategory = new Category
            {
                CategoryId = 0,
                Name = "",
                Description = "",
            };

            await ShowCategoryDialogAsync(newCategory, false);
        }

        public async void EditCategoryInformation(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var customer = button?.Tag as Category;

            if (customer == null)
            {
                return;
            }

            await ShowCategoryDialogAsync(customer, true);
        }
        private async Task ShowCategoryDialogAsync(Category category, bool isEdit)
        {
            TextBox nameTextBox = new TextBox
            {
                Header = "Tên danh mục",
                Text = category.Name,
                Width = 300
            };
            TextBox describeTextBox = new TextBox
            {
                Header = "Mô tả",
                Text = category.Description,
                Width = 300,
                Height = 100,
                TextWrapping = TextWrapping.Wrap,
                AcceptsReturn = true
            };
            


            StackPanel dialogContent = new StackPanel
            {
                Spacing = 10,
                Children = { nameTextBox, describeTextBox }
            };

            ContentDialog dialog = new ContentDialog
            {
                Title = isEdit ? "Chỉnh sửa danh mục" : "Thêm danh mục",
                Content = dialogContent,
                PrimaryButtonText = isEdit ? "Lưu" : "Thêm",
                SecondaryButtonText = isEdit ? "Xóa" : "",
                CloseButtonText = "Hủy",
                DefaultButton = ContentDialogButton.Primary,
                XamlRoot = this.XamlRoot
            };
            dialog.SecondaryButtonClick += async (s, e) =>
            {
                string? res = await ViewModel.DeleteCategoryAsync(category);
                await ShowErrorDialogAsync(res);
            };
            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                if (string.IsNullOrWhiteSpace(nameTextBox.Text) || string.IsNullOrWhiteSpace(describeTextBox.Text))
                {
                    await ShowErrorDialogAsync("Vui lòng nhập đầy đủ thông tin.");
                    return;
                }

                category.Name = nameTextBox.Text;
                category.Description = describeTextBox.Text;

                string? success = isEdit
                    ? await ViewModel.UpdateCategoryAsync(category)
                    : await ViewModel.AddCategoryAsync(category);


                await ShowErrorDialogAsync(success);
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
            Product CurrentProduct = new Product
            {
                ProductId = 0,
                Name = "",
                Description = "",
                SalePrice = 0,
                PurchasePrice = 0,
                Stock = 0,
                EnvironmentType = "",
                Size = 1,
                CareLevel = 1,
                LightRequirement = 1,
                WateringSchedule = 1,
                Category = new Category()
            };
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
                Title = "Thêm sản phẩm",
                Content = dialogContent,
                PrimaryButtonText = "Lưu",
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
                CurrentProduct.PurchasePrice = decimal.TryParse(purchasePriceTextBox.Text, out decimal purchasePrice) ? purchasePrice : 0;
                CurrentProduct.EnvironmentType = environmentTypeTextBox.Text;
                CurrentProduct.Stock = int.TryParse(stockTextBox.Text, out int stock) ? stock : 0;
                CurrentProduct.Size = (int)sizeRating.Value;
                CurrentProduct.CareLevel = (int)careLevelRating.Value;
                CurrentProduct.LightRequirement = (int)lightRequirementRating.Value;
                CurrentProduct.WateringSchedule = (int)wateringScheduleRating.Value;
                CurrentProduct.Category = categoryComboBox.SelectedItem as Category;

                // Lưu thay đổi
                string updateResult = await ViewModel.CreateProductAsync(CurrentProduct, selectedFile);

                    await ShowErrorDialogAsync(updateResult);
            }
        }
    }

}
