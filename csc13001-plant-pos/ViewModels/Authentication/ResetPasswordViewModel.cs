using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using csc13001_plant_pos.Core.Contracts.Services;
using Windows.Foundation.Metadata;

namespace csc13001_plant_pos.ViewModels.Authentication;
public partial class ResetPasswordViewModel : ObservableRecipient
{
    private readonly IAuthenticationService _authService;

    [ObservableProperty]
    private string username = string.Empty;

    [ObservableProperty]
    private string newPassword = string.Empty;

    [ObservableProperty]
    private string confirmPassword = string.Empty;

    [ObservableProperty]
    private string error = string.Empty;

    public ResetPasswordViewModel(IAuthenticationService authService)
    {
        _authService = authService;
    }

    public async Task<bool> ResetPassword()
    {
        if (string.IsNullOrWhiteSpace(NewPassword) ||
            string.IsNullOrWhiteSpace(ConfirmPassword))
        {
            Error = "All fields are required!";
            return false;
        }

        if (NewPassword != ConfirmPassword)
        {
            Error = "Passwords do not match!";
            return false;
        }

        return await _authService.ResetPassword(Username, NewPassword, ConfirmPassword);
    }
}
