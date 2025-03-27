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


namespace csc13001_plant_pos.View.Authentication
{
    public sealed partial class FormLogin : UserControl
    {
        public FormLogin()
        {
            this.InitializeComponent();
        }

        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
        }
        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = (App.Current as App)?.GetMainWindow();
            if (mainWindow?.Content is Frame frame)
            {
                frame.Navigate(typeof(RoleSelectionPage));
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
}
