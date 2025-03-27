using Microsoft.UI.Xaml.Controls;
using csc13001_plant_pos.ViewModel;


namespace csc13001_plant_pos.View
{
    public sealed partial class StaffProfilePage : Page
    {
        public StaffProfilePageViewModel ViewModel { get; }
        public StaffProfilePage()
        {
            ViewModel = new StaffProfilePageViewModel();
            this.InitializeComponent();
        }

    }
}
