using csc13001_plant_pos.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace csc13001_plant_pos.Views;

public sealed partial class SalePage : Page
{
    public SaleViewModel ViewModel
    {
        get;
    }

    public SalePage()
    {
        ViewModel = App.GetService<SaleViewModel>();
        InitializeComponent();
    }
}
