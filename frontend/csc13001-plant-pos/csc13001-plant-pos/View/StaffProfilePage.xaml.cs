using Microsoft.UI.Xaml.Controls;
using csc13001_plant_pos.ViewModel;
using csc13001_plant_pos.DTO.OrderDTO;
using Microsoft.UI.Xaml;


namespace csc13001_plant_pos.View
{
    public sealed partial class StaffProfilePage : Page
    {
        public StaffProfileViewModel ViewModel { get; }
        public StaffProfilePage()
        {
            this.InitializeComponent();
            ViewModel = App.GetService<StaffProfileViewModel>();
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
