using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using csc13001_plant_pos.DTO.AuthDTO;
using csc13001_plant_pos.Service;

namespace csc13001_plant_pos.ViewModel
{
    public partial class AuthenticationViewModel : ObservableRecipient
    {
        private readonly IAuthenticationService _authService;

        [ObservableProperty]
        private string username = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

        [ObservableProperty]
        private string error = string.Empty;

        public AuthenticationViewModel(IAuthenticationService authService)
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

            var response = await _authService.RegisterAsync(Username, Password);

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
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                Error = "Username and password are required!";
                return null;
            }

            var response = await _authService.LoginAsync(Username, Password);

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
                return response.Data;
            }
        }
    }
}
