using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using csc13001_plant_pos.DTO.AuthDTO;
using csc13001_plant_pos.Service;

namespace csc13001_plant_pos.ViewModel.Authentication;

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

    [ObservableProperty]
    private LoginResponseDTO loginResponseDTO = null;

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

        var loginResponseDTO = await _authService.LoginAsync(Username, Password);
        IsLoading = false;

        if (loginResponseDTO == null)
        {
            Error = "Login failed!";
            return null;
        }

        if (loginResponseDTO.Status == "success" && loginResponseDTO.Data != null)
        {
            return loginResponseDTO.Data;
        }
        else
        {
            Error = loginResponseDTO.Message;
            return null;
        }
    }
}