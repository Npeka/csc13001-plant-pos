using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel;
using csc13001_plant_pos.ViewModel;

namespace csc13001_plant_pos.View
{
    public sealed partial class TopSellingProductsPage : Page
    {
        public TopSellingProductViewModel ViewModel { get; }

        public TopSellingProductsPage()
        {
            this.DataContext = ViewModel = App.GetService<TopSellingProductViewModel>();
            this.InitializeComponent();
        }
    }
}
