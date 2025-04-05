using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using csc13001_plant_pos.Model;

namespace csc13001_plant_pos.View
{
    public sealed partial class DetailProductPage : Page
    {
        public Product CurrentProduct { get; private set; }
        public List<string> Tags { get; private set; }

        public DetailProductPage()
        {
            this.InitializeComponent();

            // Mock data
            CurrentProduct = new Product
            {
                ProductId = 13,
                Name = "Cây phát tài núi 2 tầng chậu đá mài",
                Description = "Cây phát tài núi là loài cây cảnh mang ý nghĩa phong thủy tốt, thích hợp làm quà tặng khai trương.",
                ImageUrl = "ms-appx:///Assets/plant_image.png",
                SalePrice = 1750000,
                PurchasePrice = 1900000,
                Stock = 15,
                Size = 36, // Đường kính chậu (cm)
                CareLevel = 1, // 1: Dễ chăm sóc, 2: Trung bình, 3: Khó
                LightRequirement = 2, // 1: Ít ánh sáng, 2: Nắng tán xa, 3: Nắng trực tiếp
                WateringSchedule = 2, // Số lần tưới nước mỗi tuần
                EnvironmentType = "Cây cảnh trong nhà",
                Category = new Category
                {
                    CategoryId = 1,
                    Name = "Cây cảnh",
                    Description = "Các loại cây cảnh trang trí"
                }
            };

            Tags = new List<string> { "Cây thủy sinh", "Cây phong thủy", "Dễ chăm sóc", "Trang trí" };
        }

        // Converter methods 
        public string SizeConverter(int size)
        {
            return $"{size}x{size}cm (DxC)";
        }

        public string CareLevelConverter(int level)
        {
            switch (level)
            {
                case 1: return "Dễ chăm sóc";
                case 2: return "Trung bình";
                case 3: return "Khó";
                default: return "Không xác định";
            }
        }

        public string LightRequirementConverter(int level)
        {
            switch (level)
            {
                case 1: return "Ít ánh sáng";
                case 2: return "Nắng tán xa, chịu được nắng trực tiếp";
                case 3: return "Cần nhiều nắng trực tiếp";
                default: return "Không xác định";
            }
        }

        public string WateringScheduleConverter(int frequency)
        {
            return $"Tưới nước {frequency} lần/tuần";
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            // Implementation for adding the product to the cart
            ContentDialog dialog = new ContentDialog()
            {
                Title = "Thông báo",
                Content = "Đã thêm sản phẩm vào giỏ hàng",
                CloseButtonText = "Đóng",
                XamlRoot = this.XamlRoot
            };

            _ = dialog.ShowAsync();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }
    }
}