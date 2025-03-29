using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using csc13001_plant_pos.Service;

namespace csc13001_plant_pos.ViewModel.Authentication;

public partial class ForgotPasswordViewModel : ObservableRecipient
{
    private readonly IAuthenticationService _authService;

    [ObservableProperty]
    private string username = string.Empty;

    [ObservableProperty]
    private string otp = string.Empty;

    [ObservableProperty]
    private bool isSendOTPEnabled = true;

    [ObservableProperty]
    private string sendOTPButtonText = "Send OTP";

    [ObservableProperty]
    private string error = string.Empty;

    [ObservableProperty]
    private string errorColor = "Red";

    [ObservableProperty]
    private string errorOtp = string.Empty;

    [ObservableProperty]
    private bool isErrorVisible = false;

    public ForgotPasswordViewModel(IAuthenticationService authService)
    {
        _authService = authService;
    }

    [RelayCommand]
    public async Task<bool> SendOTP()
    {
        ErrorColor = "Red";
        if (string.IsNullOrWhiteSpace(Username))
        {
            Error = "Please enter your username!";
            return false;
        }

        IsSendOTPEnabled = false;
        var response = await _authService.ForgotPasswordAsync(Username);

        if (response == null)
        {
            Error = "Server error! Please try again.";
            IsSendOTPEnabled = true;
            return false;
        }

        if (response.Status == "success")
        {
            Error = "OTP sent successfully!";
            ErrorColor = "Green";
            return true;
        }
        else
        {
            Error = response.Message;
            IsSendOTPEnabled = true;
            return false;
        }
    }

    [RelayCommand]
    public async Task<bool> VerifyOTP()
    {
        if (string.IsNullOrWhiteSpace(Otp))
        {
            ErrorOtp = "Please enter the OTP!";
            return false;
        }

        var response = await _authService.VerifyOTPAsync(Username, Otp);

        if (response == null)
        {
            ErrorOtp = "Server error! Please try again.";
            return false;
        }

        if (response.Status == "success")
        {
            ErrorOtp = "";
            return true;
        }
        else
        {
            ErrorOtp = response.Message; // Hiển thị lỗi từ API
            return false;
        }
    }
}
