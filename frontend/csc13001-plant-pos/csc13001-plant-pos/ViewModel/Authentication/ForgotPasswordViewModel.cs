using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using csc13001_plant_pos.DTO;
using csc13001_plant_pos.Service;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;

namespace csc13001_plant_pos.ViewModel.Authentication;

public partial class ForgotPasswordViewModel : ObservableRecipient
{
    private readonly IAuthenticationService _authService;
    private DispatcherTimer _cooldownTimer;
    private int _cooldownSeconds = 60;
    public event EventHandler NavigateToResetPassword;

    [ObservableProperty] public partial string Username { get; set; } = string.Empty;
    [ObservableProperty] public partial string ErrorUsername { get; set; } = string.Empty;
    [ObservableProperty] public partial SolidColorBrush ErrorUsernameColor { get; set; } = new SolidColorBrush(Colors.Red);
    [ObservableProperty] public partial string Otp { get; set; } = string.Empty;
    [ObservableProperty] public partial bool IsSendOTPEnabled { get; set; } = true;
    [ObservableProperty] public partial string SendOTPButtonText { get; set; } = "Gửi OTP";
    [ObservableProperty] public partial string ErrorColor { get; set; } = "Red";
    [ObservableProperty] public partial string ErrorOtp { get; set; } = string.Empty;
    [ObservableProperty] public partial bool IsErrorVisible { get; set; } = false;

    public ForgotPasswordViewModel(IAuthenticationService authService)
    {
        _authService = authService;
        _cooldownTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        _cooldownTimer.Tick += CooldownTimer_Tick;
    }

    [RelayCommand]
    public async Task SendOTP()
    {
        if (string.IsNullOrWhiteSpace(Username))
        {
            ErrorUsername = "Vui lòng nhập tên người dùng!";
            ErrorUsernameColor = new SolidColorBrush(Colors.Red);
            return;
        }

        IsSendOTPEnabled = false;
        var response = await _authService.ForgotPasswordAsync(Username);
        Console.WriteLine(response);

        if (response == null)
        {
            ErrorUsername = ApiResponseHelper.MessageServerError();
            ErrorUsernameColor = new SolidColorBrush(Colors.Red);
            IsSendOTPEnabled = true;
        }
        else if (response.IsSuccess())
        {
            ErrorUsername = "OTP đã được gửi thành công!";
            ErrorUsernameColor = new SolidColorBrush(Colors.Green);
            _cooldownTimer.Start();
        }
        else
        {
            IsSendOTPEnabled = true;
            ErrorUsername = response.Message;
            ErrorUsernameColor = new SolidColorBrush(Colors.Red);
        }
    }

    private void CooldownTimer_Tick(object sender, object e)
    {
        _cooldownSeconds--;

        if (_cooldownSeconds <= 0)
        {
            _cooldownTimer.Stop();
            _cooldownSeconds = 60;
            IsSendOTPEnabled = true;
            SendOTPButtonText = "Gửi lại OTP";
        }
        else
        {
            SendOTPButtonText = $"Chờ {_cooldownSeconds}s";
        }
    }

    [RelayCommand]
    public async Task VerifyOTP()
    {
        if (string.IsNullOrWhiteSpace(Otp))
        {
            ErrorOtp = "Vui lòng nhập OTP!";
            return;
        }

        var response = await _authService.VerifyOTPAsync(Username, Otp);

        if (response == null)
        {
            ErrorOtp = ApiResponseHelper.MessageServerError();
        }
        else if (response.IsSuccess())
        {
            ErrorOtp = string.Empty;
            NavigateToResetPassword?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            ErrorOtp = response.Message;
        }
    }
}
