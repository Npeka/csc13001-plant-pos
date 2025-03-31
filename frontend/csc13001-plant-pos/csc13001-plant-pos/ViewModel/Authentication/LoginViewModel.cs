using System;
using System.ComponentModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using csc13001_plant_pos.DTO;
using csc13001_plant_pos.DTO.AuthDTO;
using csc13001_plant_pos.Model;
using csc13001_plant_pos.Service;

namespace csc13001_plant_pos.ViewModel.Authentication;

public partial class LoginViewModel : ObservableRecipient
{
    private readonly IAuthenticationService _authService;
    private readonly UserSessionService _userSession;
    public event EventHandler<NavigateEventArgs> NavigateToDashboard;

    [ObservableProperty] public partial string Username { get; set; } = string.Empty;
    [ObservableProperty] public partial string Password { get; set; } = string.Empty;
    [ObservableProperty] public partial string Error { get; set; } = string.Empty;
    [ObservableProperty] public partial bool IsEnableLoginButton { get; set; } = true;

    public LoginViewModel(IAuthenticationService authService, UserSessionService userSession)
    {
        _authService = authService;
        _userSession = userSession;
    }

    [RelayCommand]
    public async Task Login()
    {

        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            Error = "Vui lòng nhập tên người dùng và mật khẩu!";
            return;
        }

        IsEnableLoginButton = false;
        var response = await _authService.LoginAsync(Username, Password);
        IsEnableLoginButton = true;

        if (response == null)
        {
            Error = ApiResponseHelper.MessageServerError();
        }
        else if (response.IsSuccess() && response.Data != null)
        {
            Error = string.Empty;
            var user = response.Data.User;
            _userSession.SetUser(user);
            NavigateToDashboard?.Invoke(this, new NavigateEventArgs(user));
        }
        else
        {
            Error = response.Message;
        }
    }
}

public class NavigateEventArgs : EventArgs
{
    public User User { get; }

    public NavigateEventArgs(User user)
    {
        User = user;
    }
}
