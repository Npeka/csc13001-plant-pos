using System;
using csc13001_plant_pos.Service;
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
    private readonly UserSessionService _userSessionService;

    public FormLogin()
    {
        this.InitializeComponent();
        ViewModel = App.GetService<LoginViewModel>();
    }

    private void Password_PasswordChanged(object sender, RoutedEventArgs e)
    {
        ViewModel.Password = PasswordBox.Password;
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
                var userSessionService = App.GetService<UserSessionService>();
                if (user.IsAdmin)
                {
                    frame.Navigate(typeof(AdminDashBoard), userSessionService);
                }
                else
                {
                    frame.Navigate(typeof(SaleDashBoard), userSessionService);
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
