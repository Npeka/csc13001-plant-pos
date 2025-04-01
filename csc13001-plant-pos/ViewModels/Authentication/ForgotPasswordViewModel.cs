using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using csc13001_plant_pos.Core.Contracts.Services;
using csc13001_plant_pos.Services;
using csc13001_plant_pos.Utils;
using csc13001_plant_pos.Views.Authentication;
using static System.Net.WebRequestMethods;

namespace csc13001_plant_pos.ViewModels.Authentication;
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
        var res = await _authService.ForgotPassword(Username);
        if (!res)
        {
            Error = "Username not found! Please enter a valid username!";
            IsSendOTPEnabled = true;
            return false;
        }

        Error = "OTP sent successfully!";
        ErrorColor = "Green";
        IsSendOTPEnabled = false;
        return true;
    }

    [RelayCommand]
    public async Task<bool> VerifyOTP()
    {
        if (string.IsNullOrWhiteSpace(Otp))
        {
            ErrorOtp = "Please enter the OTP!";
            return false;
        }

        var isValid = await _authService.VerifyOTP(Username, Otp);
        if (!isValid)
        {
            ErrorOtp = "Invalid OTP!";
            return false;
        }

        ErrorOtp = "";
        return true;
    }
}
