using System;
using System.Threading.Tasks;
using csc13001_plant_pos.ViewModels.Authentication;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace csc13001_plant_pos.Views.Authentication
{
    public sealed partial class FormForgotPassword : UserControl
    {
        private int cooldownTime = 30; // Cooldown 30s
        private DispatcherTimer timer;

        public ForgotPasswordViewModel ViewModel
        {
            get;
        }

        public FormForgotPassword()
        {
            this.InitializeComponent();
            this.DataContext = ViewModel = App.GetService<ForgotPasswordViewModel>();
        }

        private async void SendOTPButton_Click(object sender, RoutedEventArgs e)
        {
            var res = await ViewModel.SendOTP();
            if (res == true)
            {
                StartCooldown();
            }
        }

        private async void VerifyOTPButton_Click(object sender, RoutedEventArgs e)
        {
            var res = await ViewModel.VerifyOTP();
            if (res == true)
            {
                if (App.MainWindow.Content is Frame frame && frame.Content is AuthenticationPage authPage)
                {
                    authPage.NavigateToResetPassword(ViewModel.Username);
                }
            }
        }

        private void StartCooldown()
        {
            SendOTPButton.IsEnabled = false;
            ViewModel.IsSendOTPEnabled = false;
            ViewModel.SendOTPButtonText = $"Wait {cooldownTime}s";

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, e) =>
            {
                cooldownTime--;
                if (cooldownTime > 0)
                {
                    ViewModel.SendOTPButtonText = $"Wait {cooldownTime}s";
                }
                else
                {
                    timer.Stop();
                    ViewModel.SendOTPButtonText = "Send OTP";
                    ViewModel.IsSendOTPEnabled = true;
                    SendOTPButton.IsEnabled = true;
                    cooldownTime = 10;
                }
            };
            timer.Start();
        }

        private void NavigateToLogin_Click(object sender, RoutedEventArgs e)
        {
            if (App.MainWindow.Content is Frame frame && frame.Content is AuthenticationPage authPage)
            {
                authPage.NavigateToLogin();
            }
        }
    }
}
