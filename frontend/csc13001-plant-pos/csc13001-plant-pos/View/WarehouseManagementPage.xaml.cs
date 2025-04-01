using csc13001_plant_pos.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace csc13001_plant_pos.View
{
    public sealed partial class WarehouseManagementPage : Page
    {
        public WarehouseManagementViewModel ViewModel { get; }

        public WarehouseManagementPage()
        {
            this.InitializeComponent();
            ViewModel = App.GetService<WarehouseManagementViewModel>();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.LoadInventoryOrdersAsync();
        }
        private async void AddStockReceiptButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddStockReceiptPage));
        }
    }
}