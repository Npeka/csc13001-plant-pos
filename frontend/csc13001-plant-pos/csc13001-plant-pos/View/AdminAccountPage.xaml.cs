using Microsoft.UI.Xaml.Controls;
using csc13001_plant_pos.ViewModel;

namespace csc13001_plant_pos.View
{
    public sealed partial class AdminAccountPage : Page
    {
        public AdminAccountViewModel ViewModel { get; }

        public AdminAccountPage()
        {
            this.InitializeComponent();
            ViewModel = App.GetService<AdminAccountViewModel>();
            DataContext = ViewModel;
        }
    }
}