using csc13001_plant_pos.DTO.OrderDTO;
using csc13001_plant_pos.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace csc13001_plant_pos.View
{
    public sealed partial class OrderPage : Page
    {
        public OrderViewModel ViewModel { get; }

        public OrderPage()
        {
            this.InitializeComponent();
            ViewModel = App.GetService<OrderViewModel>();
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