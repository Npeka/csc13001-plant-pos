using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using csc13001_plant_pos_frontend.Core.Contracts.Services;

namespace csc13001_plant_pos_frontend.ViewModels.Authentication;

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
        Error = string.Empty;

        if (string.IsNullOrWhiteSpace(NewPassword) || string.IsNullOrWhiteSpace(ConfirmPassword))
        {
            Error = "All fields are required!";
            return false;
        }

        if (NewPassword != ConfirmPassword)
        {
            Error = "Passwords do not match!";
            return false;
        }

        var response = await _authService.ResetPasswordAsync(Username, NewPassword, ConfirmPassword);

        if (response == null)
        {
            Error = "Server error! Please try again.";
            return false;
        }

        if (response.Status == "success")
        {
            return true;
        }
        else
        {
            Error = response.Message;
            return false;
        }
    }
}
