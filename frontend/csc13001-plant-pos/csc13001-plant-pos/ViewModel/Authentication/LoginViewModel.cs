using System.ComponentModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using csc13001_plant_pos.DTO.AuthDTO;
using csc13001_plant_pos.Service;

namespace csc13001_plant_pos.ViewModel.Authentication;

public partial class LoginViewModel : ObservableRecipient
{
    private readonly IAuthenticationService _authService;
    private readonly UserSessionService _userSession;

    [ObservableProperty]
    private string username = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    [ObservableProperty]
    private bool isNotLoading = true;

    [ObservableProperty]
    private string error = string.Empty;

    [ObservableProperty]
    private LoginResponseDTO loginResponseDTO = null;

    public LoginViewModel(IAuthenticationService authService, UserSessionService userSession)
    {
        _authService = authService;
        _userSession = userSession;
    }

    public async Task RegisterAsync()
    {
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            Error = "Username and password are required!";
            return;
        }

        IsNotLoading = false;
        var response = await _authService.RegisterAsync(Username, Password);
        IsNotLoading = true;

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
        IsNotLoading = false;
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            Error = "Username and password are required!";
            IsNotLoading = true;
            return null;
        }

        var apiResponse = await _authService.LoginAsync(Username, Password);
        loginResponseDTO = apiResponse?.Data;
        IsNotLoading = true;

        if (loginResponseDTO == null)
        {
            Error = "Login failed!";
            return null;
        }
        if (apiResponse.Status == "success" && apiResponse.Data != null)
        {
            Error = string.Empty;
            _userSession.SetUser(loginResponseDTO.User);
            return apiResponse.Data;
        }
        else
        {
            Error = apiResponse.Message;
            return null;
        }
    }
}