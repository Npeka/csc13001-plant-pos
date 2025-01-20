using System.Diagnostics;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using csc13001_plant_pos.ViewModels;
using csc13001_plant_pos.Data.Contexts;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using csc13001_plant_pos.Contracts.Services;
using Microsoft.UI.Xaml.Media.Animation;

namespace csc13001_plant_pos.Views;

public sealed partial class AuthenticationPage : Page
{
    public AuthenticationViewModel ViewModel
    {
        get;
    }

    public AuthenticationPage()
    {
        InitializeComponent();
        ViewModel = App.GetService<AuthenticationViewModel>();
        this.DataContext = ViewModel;
    }

    private void NavigateToForgotPassword_Click(object sender, RoutedEventArgs e)
    {
    }

    private void Password_PasswordChanged(object sender, RoutedEventArgs e)
    {
        ViewModel.Password = PasswordBox.Password;
    }

    private async void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        var user = await ViewModel.LoginAsync();
        if (user != null && App.MainWindow.Content is Frame frame)
        {
            frame.Navigate(typeof(ShellPage), user, new EntranceNavigationTransitionInfo());
        }
    }
}
