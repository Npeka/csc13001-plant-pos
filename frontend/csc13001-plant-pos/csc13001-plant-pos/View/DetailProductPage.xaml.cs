using csc13001_plant_pos.ViewModel;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace csc13001_plant_pos.View
{
    public sealed partial class DetailProductPage : Page
    {
        public DetailProductViewModel ViewModel { get; }

        public DetailProductPage()
        {
            this.InitializeComponent();
            ViewModel = App.GetService<DetailProductViewModel>();
            this.DataContext = ViewModel;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is string productId)
            {
                await ViewModel.LoadProductAsync(productId);
            }
        }

        private void BackButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        private void AddToCart_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
        }
    }
}