using Microsoft.UI.Xaml.Controls;
using csc13001_plant_pos.ViewModel;

namespace csc13001_plant_pos.View { 
    public sealed partial class CustomerProfilePage : Page
    {
        public CustomerProfileViewModel ViewModel { get; } = new CustomerProfileViewModel();
        public CustomerProfilePage()
        {
            this.InitializeComponent();
        }
    }
}
