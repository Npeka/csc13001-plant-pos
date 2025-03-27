using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using csc13001_plant_pos.View.Authentication;

namespace csc13001_plant_pos.View
{
    public sealed partial class AuthenticationPage : Page
    {
        public FormLogin LoginForm { get; } = new FormLogin();
        public FormForgotPassword ForgotPasswordForm { get; } = new FormForgotPassword();
        public FormResetPassword ResetPasswordForm { get; } = new FormResetPassword();

        public AuthenticationPage()
        {
            this.InitializeComponent();
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
