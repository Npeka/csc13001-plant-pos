using System;
using csc13001_plant_pos.DTO.AuthDTO;
using csc13001_plant_pos.ViewModel.Authentication;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;


namespace csc13001_plant_pos.View.Authentication;

public sealed partial class FormLogin : UserControl
{
    public LoginViewModel ViewModel
    {
        get;
    }

    public FormLogin()
    {
        this.InitializeComponent();
        this.DataContext = ViewModel = App.GetService<LoginViewModel>();
    }

    private void Password_PasswordChanged(object sender, RoutedEventArgs e)
    {
    }
    private async void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.LoginAsync();
        if (ViewModel.LoginResponseDTO != null)
        {
            var user = ViewModel.LoginResponseDTO.User;

            var mainWindow = (App.Current as App)?.GetMainWindow();
            if (mainWindow?.Content is Frame frame)
            {
                if (user.IsAdmin)
                {
                    frame.Navigate(typeof(AdminDashBoard));
                }
                else
                {
                    frame.Navigate(typeof(SaleDashBoard));
                }
            }
        }
    }


    private void NavigateToForgotPassword_Click(object sender, RoutedEventArgs e)
    {
        var mainWindow = (App.Current as App)?.GetMainWindow();
        if (mainWindow?.Content is Frame frame && frame.Content is AuthenticationPage authenticationPage)
        {
            authenticationPage.NavigateToForgotPassword();
        }
    }
}
