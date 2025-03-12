using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using csc13001_plant_pos_frontend.Core.Contracts.Services;
using csc13001_plant_pos_frontend.Core.DTOs.Auth;
using csc13001_plant_pos_frontend.Core.Models;

namespace csc13001_plant_pos_frontend.ViewModels.Authentication;

public partial class LoginViewModel : ObservableRecipient
{
    private readonly IAuthenticationService _authService;

    [ObservableProperty]
    private string username = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    [ObservableProperty]
    private bool isLoading = false;

    [ObservableProperty]
    private string error = string.Empty;

    public LoginViewModel(IAuthenticationService authService)
    {
        _authService = authService;
    }

    public async Task RegisterAsync()
    {
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            Error = "Username and password are required!";
            return;
        }

        IsLoading = true;
        var response = await _authService.RegisterAsync(Username, Password);
        IsLoading = false;

        if (response == null)
        {
            Error = "Registration failed!";
            return;
        }

        if (response.Status == "success")
        {
            Error = "Registration successful!";
        }
        else
        {
            Error = response.Message;
        }
    }

    [RelayCommand]
    public async Task<LoginResponseDTO?> LoginAsync()
    {
        IsLoading = true;
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            Error = "Username and password are required!";
            IsLoading = false;
            return null;
        }

        var response = await _authService.LoginAsync(Username, Password);
        IsLoading = false;

        if (response == null)
        {
            Error = "Login failed!";
            return null;
        }

        if (response.Status == "success" && response.Data != null)
        {
            return response.Data;
        }
        else
        {
            Error = response.Message;
            return null;
        }
    }
}
