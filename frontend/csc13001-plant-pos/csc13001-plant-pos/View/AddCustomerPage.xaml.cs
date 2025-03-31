using System;
using csc13001_plant_pos.ViewModel;
using Microsoft.UI.Xaml.Controls;

namespace csc13001_plant_pos.View
{
    public sealed partial class AddCustomerPage : Page
    {
        public AddCustomerViewModel ViewModel { get; }
        public AddCustomerPage()
        {
            this.InitializeComponent();
            ViewModel = App.GetService<AddCustomerViewModel>();
        }

        private async void AddCustomerButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            if (ViewModel != null)
            {
               
                var customerId = await ViewModel.AddCustomer();
                if (customerId != null)
                {
                    Frame.Navigate(typeof(CustomerProfilePage), customerId);
                }
            }
        }
    }
}
