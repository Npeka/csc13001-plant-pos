using csc13001_plant_pos.ViewModel;
using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace csc13001_plant_pos.View
{

    public sealed partial class CustomerManagementPage : Page
    {
        public CustomerManagementViewModel ViewModel { get; }

        public CustomerManagementPage()
        {
            this.DataContext = ViewModel = App.GetService<CustomerManagementViewModel>();
            this.InitializeComponent();
        }

        private SolidColorBrush GetRankColor(string rank) => rank switch
        {
            "Đồng" => new SolidColorBrush(Colors.Brown),
            "Bạc" => new SolidColorBrush(Colors.Silver),
            "Vàng" => new SolidColorBrush(Colors.Gold),
            "Bạch Kim" => new SolidColorBrush(Colors.LightGray),
            "Kim Cương" => new SolidColorBrush(Colors.DeepSkyBlue),
            _ => new SolidColorBrush(Colors.Gray),
        };
    }
}
