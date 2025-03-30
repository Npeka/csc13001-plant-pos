using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace csc13001_plant_pos.View.Authentication
{
    public sealed partial class FormResetPassword : UserControl
    {
        public FormResetPassword()
        {
            this.InitializeComponent();
        }
        public void SetUsername(string username)
        {
        }

        private async void ResetPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = (App.Current as App)?.GetMainWindow();
            if (mainWindow?.Content is Frame frame && frame.Content is AuthenticationPage authenticationPage)
            {
                authenticationPage.NavigateToLogin();
            }
        }
    }
}
