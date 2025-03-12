using csc13001_plant_pos_frontend.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace csc13001_plant_pos_frontend.Views;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel
    {
        get;
    }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();
    }
}
