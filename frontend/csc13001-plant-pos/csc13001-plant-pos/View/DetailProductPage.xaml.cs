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

            TextBox nameTextBox = new TextBox
            {
                Header = "Tên sản phẩm",
                Text = CurrentProduct.Name,
                Width = 300
            };

            TextBox descriptionTextBox = new TextBox
            {
                Header = "Mô tả",
                Text = CurrentProduct.Description,
                Width = 300,
                AcceptsReturn = true,
                TextWrapping = TextWrapping.Wrap,
                Height = 100
            };

            // Disabled fields
            TextBox purchasePriceTextBox = new TextBox
            {
                Header = "Giá nhập (VND)",
                Text = CurrentProduct.PurchasePrice.ToString(),
                Width = 300,
                IsEnabled = false
            };

            TextBox stockTextBox = new TextBox
            {
                Header = "Số lượng tồn kho",
                Text = CurrentProduct.Stock.ToString(),
                Width = 300,
                IsEnabled = false
            };

            // Regular fields
            TextBox salePriceTextBox = new TextBox
            {
                Header = "Giá bán (VND)",
                Text = CurrentProduct.SalePrice.ToString(),
                Width = 300
            };

            TextBox environmentTypeTextBox = new TextBox
            {
                Header = "Loại môi trường",
                Text = CurrentProduct.EnvironmentType,
                Width = 300
            };
            ComboBox categoryComboBox = new ComboBox
            {
                Header = "Phân loại",
                ItemsSource = ViewModel.Categories,
                DisplayMemberPath = "Name",
                SelectedItem = ViewModel.Categories.FirstOrDefault(c => c.CategoryId == CurrentProduct.Category.CategoryId),
                Width = 300
            };

            // Dropdown fields with values 1-5
            ComboBox sizeComboBox = new ComboBox
            {
                Header = "Kích thước chậu",
                ItemsSource = Enumerable.Range(1, 5).ToList(),
                SelectedItem = CurrentProduct.Size,
                Width = 300
            };

            ComboBox careLevelComboBox = new ComboBox
            {
                Header = "Độ khó chăm sóc",
                ItemsSource = Enumerable.Range(1, 5).ToList(),
                SelectedItem = CurrentProduct.CareLevel,
                Width = 300
            };

            ComboBox lightRequirementComboBox = new ComboBox
            {
                Header = "Yêu cầu ánh sáng",
                ItemsSource = Enumerable.Range(1, 5).ToList(),
                SelectedItem = CurrentProduct.LightRequirement,
                Width = 300
            };

            ComboBox wateringScheduleComboBox = new ComboBox
            {
                Header = "Nhu cầu nước",
                ItemsSource = Enumerable.Range(1, 5).ToList(),
                SelectedItem = CurrentProduct.WateringSchedule,
                Width = 300
            };

            // Image selection
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
                Source = string.IsNullOrEmpty(CurrentProduct.ImageUrl) ? null : new BitmapImage(new Uri(CurrentProduct.ImageUrl))
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

            // Create dialog layout
            StackPanel dialogContent = new StackPanel
            {
                Spacing = 10,
                Children =
        {
            nameTextBox,
            descriptionTextBox,
            salePriceTextBox,
            purchasePriceTextBox,
            stockTextBox,
            environmentTypeTextBox,
            categoryComboBox,
            sizeComboBox,
            careLevelComboBox,
            lightRequirementComboBox,
            wateringScheduleComboBox,
            selectImageButton,
            selectedImagePreview
        }
            };

            var scrollViewer = new ScrollViewer
            {
                Content = dialogContent,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                MaxHeight = 700
            };

            ContentDialog dialog = new ContentDialog
            {
                Title = "Chỉnh sửa sản phẩm",
                Content = scrollViewer,
                PrimaryButtonText = "Lưu",
                CloseButtonText = "Hủy",
                DefaultButton = ContentDialogButton.Primary,
                XamlRoot = this.XamlRoot
            };

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(nameTextBox.Text) ||
                    string.IsNullOrWhiteSpace(descriptionTextBox.Text) ||
                    !decimal.TryParse(salePriceTextBox.Text, out decimal salePrice) ||
                    sizeComboBox.SelectedItem == null ||
                    careLevelComboBox.SelectedItem == null ||
                    lightRequirementComboBox.SelectedItem == null ||
                    wateringScheduleComboBox.SelectedItem == null)
                {
                    await ShowErrorDialogAsync("Vui lòng nhập đầy đủ thông tin.");
                    return;
                }

                // Update product
                CurrentProduct.Name = nameTextBox.Text;
                CurrentProduct.Description = descriptionTextBox.Text;
                CurrentProduct.SalePrice = salePrice;
                CurrentProduct.EnvironmentType = environmentTypeTextBox.Text;
                CurrentProduct.Size = (int)sizeComboBox.SelectedItem;
                CurrentProduct.CareLevel = (int)careLevelComboBox.SelectedItem;
                CurrentProduct.LightRequirement = (int)lightRequirementComboBox.SelectedItem;
                CurrentProduct.WateringSchedule = (int)wateringScheduleComboBox.SelectedItem;

                // Save changes
                await ViewModel.UpdateProductAsync(CurrentProduct, selectedFile);

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