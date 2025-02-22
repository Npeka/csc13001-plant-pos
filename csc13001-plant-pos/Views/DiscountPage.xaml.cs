using csc13001_plant_pos.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace csc13001_plant_pos.Views;

public sealed partial class DiscountPage : Page
{
    public DiscountViewModel ViewModel
    {
        get;
    }

    public DiscountPage()
    {
        ViewModel = App.GetService<DiscountViewModel>();
        InitializeComponent();
    }
}
