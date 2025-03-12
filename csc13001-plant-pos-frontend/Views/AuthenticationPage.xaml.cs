using System.Diagnostics;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using csc13001_plant_pos_frontend.ViewModels;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using csc13001_plant_pos_frontend.Contracts.Services;
using Microsoft.UI.Xaml.Media.Animation;
using csc13001_plant_pos_frontend.Views.Authentication;
using csc13001_plant_pos_frontend.ViewModels.Authentication;
using Windows.Web.AtomPub;

namespace csc13001_plant_pos_frontend.Views;

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
        InitializeComponent();
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
