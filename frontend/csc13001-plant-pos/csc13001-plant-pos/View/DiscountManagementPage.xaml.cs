using csc13001_plant_pos.ViewModel;
using Microsoft.UI.Xaml.Controls;

namespace csc13001_plant_pos.View { 
    public sealed partial class DiscountManagementPage : Page
    {
        public DiscountManagementPageViewModel ViewModel { get; } = new DiscountManagementPageViewModel();
        public DiscountManagementPage()
        {
            this.InitializeComponent();
        }
    }
}
