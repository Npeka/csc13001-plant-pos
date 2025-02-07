using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using csc13001_plant_pos.ViewModels.Authentication;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace csc13001_plant_pos.Views.Authentication;
public sealed partial class FormResetPassword : UserControl
{
    private readonly ResetPasswordViewModel ViewModel;
    public FormResetPassword()
    {
        this.InitializeComponent();
        this.DataContext = ViewModel = App.GetService<ResetPasswordViewModel>();
    }

    public void SetUsername(string username)
    {
        ViewModel.Username = username;
    }

    private async void ResetPasswordButton_Click(object sender, RoutedEventArgs e)
    {
        var res = await ViewModel.ResetPassword();
        if (res == true)
        {
            if (App.MainWindow.Content is Frame frame && frame.Content is AuthenticationPage authenticationPage)
            {
                authenticationPage.NavigateToLogin();
            }
        }
    }
}
