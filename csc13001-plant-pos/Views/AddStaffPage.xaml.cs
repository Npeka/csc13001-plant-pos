using csc13001_plant_pos.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace csc13001_plant_pos.Views;

public sealed partial class AddStaffPage : Page
{
    public AddStaffViewModel ViewModel
    {
        get;
    }

    public AddStaffPage()
    {
        ViewModel = App.GetService<AddStaffViewModel>();
        InitializeComponent();
    }
}
