using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using csc13001_plant_pos.Core.Contracts.Services;
using csc13001_plant_pos.Core.Models;


namespace csc13001_plant_pos.ViewModels.Authentication;
public partial class LoginViewModel : ObservableRecipient
{
    private readonly IAuthenticationService _authService;
    public string Username
    {
        get; set;
    }

    public string Password
    {
        get; set;
    }

    public bool IsLoading
    {
        get; set;
    }

    public string Error
    {
        get;
        private set;
    }

    public LoginViewModel(IAuthenticationService authService)
    {
        _authService = authService;
        Username = string.Empty;
        Password = string.Empty;
        IsLoading = false;
        Error = string.Empty;
    }

    public async Task RegisterAsync()
    {
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            Error = "Username and password are required!";
            return;
        }

        var success = await _authService.RegisterAsync(Username, Password);
        if (!success)
        {
            Error = "Username already exists!";
        }
        else
        {
            Error = "Registration successful!";
        }
    }

    [RelayCommand]
    public async Task<User?> LoginAsync()
    {
        IsLoading = true;
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            Error = "Username and password are required!";
            IsLoading = false;
            return null;
        }

        var user = await _authService.LoginAsync(Username, Password);
        if (user == null)
        {
            Error = "Invalid username or password!";
        }

        IsLoading = false;
        return user;
    }
}
