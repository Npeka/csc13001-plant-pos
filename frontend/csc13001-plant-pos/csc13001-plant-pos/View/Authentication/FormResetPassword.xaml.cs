using System;
using csc13001_plant_pos.ViewModel.Authentication;
using Microsoft.UI.Xaml.Controls;

namespace csc13001_plant_pos.View.Authentication
{
    public sealed partial class FormResetPassword : UserControl
    {
        ResetPasswordViewModel ViewModel { get; set; }

        public FormResetPassword()
        {
            this.InitializeComponent();
            this.DataContext = ViewModel = App.GetService<ResetPasswordViewModel>();
            ViewModel.NavigateToLogin += OnNavigateToLogin;
        }

        public void SetUsername(string username)
        {
            ViewModel.Username = username;
        }

        private void OnNavigateToLogin(object sender, EventArgs e)
        {
            var mainWindow = (App.Current as App)?.GetMainWindow();
            if (mainWindow?.Content is Frame frame && frame.Content is AuthenticationPage authenticationPage)
            {
                authenticationPage.NavigateToLogin();
            }
        }
    }
}
