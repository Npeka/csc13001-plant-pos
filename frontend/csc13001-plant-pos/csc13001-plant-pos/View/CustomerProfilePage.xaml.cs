using Microsoft.UI.Xaml.Controls;
using csc13001_plant_pos.ViewModel;
using csc13001_plant_pos.DTO.OrderDTO;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Navigation;

namespace csc13001_plant_pos.View { 
    public sealed partial class CustomerProfilePage : Page
    {
        public CustomerProfileViewModel ViewModel { get; }
        public CustomerProfilePage()
        {
            this.InitializeComponent();
            ViewModel = App.GetService<CustomerProfileViewModel>();
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is string customerId)
            {
                await ViewModel.LoadCustomerDataAsync(customerId);
                await ViewModel.LoadCustomerOrdersAsync(customerId);
            }
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
    }
}
