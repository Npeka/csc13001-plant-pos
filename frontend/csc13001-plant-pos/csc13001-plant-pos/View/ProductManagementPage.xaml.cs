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
                Width = 300
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
                CloseButtonText = "Hủy",
                DefaultButton = ContentDialogButton.Primary,
                XamlRoot = this.XamlRoot
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

    }

}
