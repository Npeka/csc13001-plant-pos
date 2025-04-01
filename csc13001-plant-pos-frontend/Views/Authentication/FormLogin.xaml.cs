using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using csc13001_plant_pos_frontend.ViewModels.Authentication;
using System.Diagnostics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace csc13001_plant_pos_frontend.Views.Authentication;
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

    private void NavigateToForgotPassword_Click(object sender, RoutedEventArgs e)
    {
        if (App.MainWindow.Content is Frame frame && frame.Content is AuthenticationPage authenticationPage)
        {
            authenticationPage.NavigateToForgotPassword();
        }
    }
}
