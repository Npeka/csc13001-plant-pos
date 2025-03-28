using csc13001_plant_pos.ViewModel;
using Microsoft.UI.Xaml.Controls;

namespace csc13001_plant_pos.View
{
    public sealed partial class WarehouseManagementPage : Page
    {
        public WarehouseManagementViewModel ViewModel { get; } = new WarehouseManagementViewModel();
        public WarehouseManagementPage()
        {
            this.InitializeComponent();
        }
    }
}
