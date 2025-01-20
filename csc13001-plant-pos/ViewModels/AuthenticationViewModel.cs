﻿using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using csc13001_plant_pos.Core.Models;
using csc13001_plant_pos.Services;
using csc13001_plant_pos.Views;
using Microsoft.UI.Xaml.Controls;

namespace csc13001_plant_pos.ViewModels;

public partial class AuthenticationViewModel : ObservableRecipient
{
    private readonly AuthenticationService _service;
    public string Username
    {
        get; set;
    }

    public string Password
    {
        get; set;
    }

    public string Error
    {
        get;
        private set;
    }

    public AuthenticationViewModel(AuthenticationService authenticationService)
    {
        _service = authenticationService;
        Username = string.Empty;
        Password = string.Empty;
        Error = string.Empty;
    }

    public async Task RegisterAsync()
    {
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            Error = "Username and password are required!";
            return;
        }

        var success = await _service.RegisterAsync(Username, Password);
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
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            Error = "Username and password are required!";
            return null;
        }

        var user = await _service.LoginAsync(Username, Password);
        if (user == null)
        {
            Error = "Invalid username or password!";
        }

        return user;
    }
}
