using csc13001_plant_pos.Model;
using csc13001_plant_pos.ViewModel;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

namespace csc13001_plant_pos.View
{
    public sealed partial class ProductPage : Page
    {
        public ProductsViewModel ViewModel { get; }

        public ProductPage()
        {
            this.InitializeComponent();
            this.DataContext = ViewModel = App.GetService<ProductsViewModel>();
        }

        private void ProductItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (sender is Grid grid && grid.Tag is Product product)
            {
                string productId = product.ProductId.ToString();
                Frame.Navigate(typeof(DetailProductPage), productId);
            }
        }
    }
}