using Microsoft.UI.Xaml.Controls;
using csc13001_plant_pos.View.Authentication;
using csc13001_plant_pos.ViewModel;

namespace csc13001_plant_pos.View
{
    public sealed partial class AuthenticationPage : Page
    {
        public FormLogin LoginForm { get; } = new FormLogin();
        public FormForgotPassword ForgotPasswordForm { get; } = new FormForgotPassword();
        public FormResetPassword ResetPasswordForm { get; } = new FormResetPassword();
        public AuthenticationViewModel ViewModel
        {
            get;
        }

        public AuthenticationPage()
        {
            this.InitializeComponent();
            this.DataContext = ViewModel = App.GetService<AuthenticationViewModel>();
        }
        public void NavigateToLogin()
        {
            FormLayout.Child = LoginForm;
        }

        public void NavigateToForgotPassword()
        {
            FormLayout.Child = ForgotPasswordForm;
        }

        public void NavigateToResetPassword(string username)
        {
            ResetPasswordForm.SetUsername(username);
            FormLayout.Child = ResetPasswordForm;
        }
    }
}
