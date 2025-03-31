using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using csc13001_plant_pos.ViewModel.Authentication;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace csc13001_plant_pos.View.Authentication;

public sealed partial class FormForgotPassword : UserControl
{

    public ForgotPasswordViewModel ViewModel { get; }

    public FormForgotPassword()
    {
        this.InitializeComponent();
        this.DataContext = ViewModel = App.GetService<ForgotPasswordViewModel>();
        ViewModel.NavigateToResetPassword += OnNavigateToResetPassword;
    }
    private void OnNavigateToResetPassword(object sender, EventArgs e)
    {
        var mainWindow = (App.Current as App)?.GetMainWindow();
        if (mainWindow?.Content is Frame frame && frame.Content is AuthenticationPage authPage)
        {
            authPage.NavigateToResetPassword(ViewModel.Username);
        }
    }

    private void NavigateToLogin_Click(object sender, RoutedEventArgs e)
    {
        var mainWindow = (App.Current as App)?.GetMainWindow();
        if (mainWindow?.Content is Frame frame && frame.Content is AuthenticationPage authPage)
        {
            authPage.NavigateToLogin();
        }
    }
}
