using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using csc13001_plant_pos.DTO;
using csc13001_plant_pos.Model;
using csc13001_plant_pos.Service;

namespace csc13001_plant_pos.ViewModel.Authentication;

public partial class LoginViewModel : ObservableRecipient
{
    private readonly IAuthenticationService _authService;
    private readonly UserSessionService _userSession;
    private readonly ICredentialStorageService _credentialService;
    public event EventHandler<NavigateEventArgs> NavigateToDashboard;

    [ObservableProperty] private string _username = string.Empty;
    [ObservableProperty] private string _password = string.Empty;
    [ObservableProperty] private string _error = string.Empty;
    [ObservableProperty] private bool _isEnableLoginButton = true;
    [ObservableProperty] private bool _rememberMe = false;
    [ObservableProperty] private ObservableCollection<RememberedCredential> _savedCredentials = new();

    public LoginViewModel(IAuthenticationService authService, UserSessionService userSession, ICredentialStorageService credentialService)
    {
        _authService = authService;
        _userSession = userSession;
        _credentialService = credentialService;

        LoadSavedCredentialsAsync();
    }

    private async Task LoadSavedCredentialsAsync()
    {
        var credentials = await _credentialService.GetRememberedCredentialsAsync();
        SavedCredentials = new ObservableCollection<RememberedCredential>(credentials);
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

            if (RememberMe)
            {
                await _credentialService.SaveCredentialAsync(Username, Password);
                await LoadSavedCredentialsAsync();
            }

            NavigateToDashboard?.Invoke(this, new NavigateEventArgs(user));
        }
        else
        {
            Error = response.Message;
        }
    }

    [RelayCommand]
    public async Task DeleteSavedCredential(RememberedCredential credential)
    {
        if (credential != null)
        {
            await _credentialService.RemoveCredentialAsync(credential.Username);
            SavedCredentials.Remove(credential);
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