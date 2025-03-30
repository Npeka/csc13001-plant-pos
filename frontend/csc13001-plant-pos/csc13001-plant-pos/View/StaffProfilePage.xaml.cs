using Microsoft.UI.Xaml.Controls;
using csc13001_plant_pos.ViewModel;


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

    }
}
