using csc13001_plant_pos.ViewModel;
using Microsoft.UI.Xaml.Controls;


namespace csc13001_plant_pos.View
{
    public sealed partial class OrderPage : Page
    {
        public OrderViewModel ViewModel { get; } = new OrderViewModel();
        public OrderPage()
        {
            this.InitializeComponent();
        }
    }
}
