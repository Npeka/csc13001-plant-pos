using csc13001_plant_pos.ViewModel;
using Microsoft.UI.Xaml.Controls;

namespace csc13001_plant_pos.View
{
    public sealed partial class StaffManagementPage : Page
    {
        public StaffManagementViewModel ViewModel { get; }

        public StaffManagementPage()
        {
            this.DataContext = ViewModel = App.GetService<StaffManagementViewModel>();
            this.InitializeComponent();
        }
    }
}