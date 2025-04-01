using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using csc13001_plant_pos.DTO;
using csc13001_plant_pos.Service;

namespace csc13001_plant_pos.ViewModel.Authentication
{
    public partial class ResetPasswordViewModel : ObservableRecipient
    {
        private readonly IAuthenticationService _authService;
        public event EventHandler NavigateToLogin;

        [ObservableProperty] public partial string Username { get; set; } = string.Empty;

        [ObservableProperty] public partial string NewPassword { get; set; } = string.Empty;

        [ObservableProperty] public partial string ConfirmPassword { get; set; } = string.Empty;

        [ObservableProperty] public partial string Error { get; set; } = string.Empty;

        public ResetPasswordViewModel(IAuthenticationService authService)
        {
            _authService = authService;
        }

        [RelayCommand]
        public async Task ResetPassword()
        {
            if (string.IsNullOrWhiteSpace(NewPassword) || string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                Error = "Tất cả các trường là bắt buộc!";
            }

            else if (NewPassword != ConfirmPassword)
            {
                Error = "Mật khẩu không khớp!";
                return;
            }

            var response = await _authService.ResetPasswordAsync(Username, NewPassword, ConfirmPassword);

            if (response == null)
            {
                Error = ApiResponseHelper.MessageServerError();
            }
            else if (response.IsSuccess())
            {
                Error = string.Empty;
                NavigateToLogin?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                Error = response.Message;
            }
        }
    }
}
