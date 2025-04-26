using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using csc13001_plant_pos.Model;
using csc13001_plant_pos.Service;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage;

namespace csc13001_plant_pos.ViewModel
{
    public partial class AdminAccountViewModel : ObservableObject
    {
        private readonly IAuthenticationService _authService;
        private readonly IStaffService _staffService;
        private readonly UserSessionService _userSessionService;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string phone;

        [ObservableProperty]
        private string currentPassword;

        [ObservableProperty]
        private string newPassword;

        [ObservableProperty]
        private string confirmPassword;

        [ObservableProperty]
        private string infoErrorMessage;

        [ObservableProperty]
        private string passwordErrorMessage;

        [ObservableProperty]
        private string infoSuccessMessage;

        [ObservableProperty]
        private string passwordSuccessMessage;

        public AdminAccountViewModel(IAuthenticationService authService, IStaffService staffService, UserSessionService userSessionService)
        {
            _authService = authService;
            _staffService = staffService;
            _userSessionService = userSessionService;

            var currentUser = _userSessionService.CurrentUser;
            if (currentUser != null)
            {
                Email = currentUser.Email;
                Phone = currentUser.Phone;
            }
        }

        [RelayCommand]
        public async Task SaveInfo()
        {
            InfoErrorMessage = string.Empty;
            InfoSuccessMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(Email))
            {
                InfoErrorMessage = "Email không được để trống.";
                return;
            }
            if (!Regex.IsMatch(Email?.Trim(), @"^[a-zA-Z0-9._%+-]+@gmail\.com$"))
            {
                InfoErrorMessage = "Email phải có định dạng hợp lệ và sử dụng đuôi @gmail.com.";
                return;
            }

            if (string.IsNullOrWhiteSpace(Phone))
            {
                InfoErrorMessage = "Số điện thoại không được để trống.";
                return;
            }
            if (!Regex.IsMatch(Phone, @"^0[0-9]{9,10}$"))
            {
                InfoErrorMessage = "Số điện thoại phải bắt đầu bằng 0 và có 10 hoặc 11 chữ số.";
                return;
            }

            var currentUser = _userSessionService.CurrentUser;
            if (currentUser == null)
            {
                InfoErrorMessage = "Không tìm thấy thông tin người dùng.";
                return;
            }

            currentUser.Email = Email;
            currentUser.Phone = Phone;

            var success = await _staffService.UpdateStaffAsync(currentUser, null);
            if (success.Contains("thành công"))
            {
                InfoErrorMessage = string.Empty;
                InfoSuccessMessage = success;
                _userSessionService.SetUser(currentUser);
            }
            else
            {
                InfoErrorMessage = success;
                InfoSuccessMessage = string.Empty;
            }
        }

        [RelayCommand]
        public void CancelInfo()
        {
            InfoErrorMessage = string.Empty;
            InfoSuccessMessage = string.Empty;
            var currentUser = _userSessionService.CurrentUser;
            if (currentUser != null)
            {
                Email = currentUser.Email;
                Phone = currentUser.Phone;
            }
        }

        [RelayCommand]
        public async Task SavePassword()
        {
            PasswordErrorMessage = string.Empty;
            PasswordSuccessMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(CurrentPassword))
            {
                PasswordErrorMessage = "Mật khẩu hiện tại không được để trống.";
                return;
            }

            if (string.IsNullOrWhiteSpace(NewPassword))
            {
                PasswordErrorMessage = "Mật khẩu mới không được để trống.";
                return;
            }
            if (NewPassword.Length < 5)
            {
                PasswordErrorMessage = "Mật khẩu mới phải có ít nhất 5 ký tự.";
                return;
            }

            if (NewPassword != ConfirmPassword)
            {
                PasswordErrorMessage = "Mật khẩu xác nhận không khớp.";
                return;
            }

            var currentUser = _userSessionService.CurrentUser;
            if (currentUser == null)
            {
                PasswordErrorMessage = "Không tìm thấy thông tin người dùng.";
                return;
            }

            var loginResponse = await _authService.LoginAsync("admin", CurrentPassword);
            if (loginResponse == null || loginResponse.Status != "success")
            {
                PasswordErrorMessage = "Mật khẩu hiện tại không đúng.";
                return;
            }

            var resetResponse = await _authService.ResetPasswordAsync("admin", NewPassword, ConfirmPassword);
            if (resetResponse != null && resetResponse.Status == "success")
            {
                PasswordErrorMessage = string.Empty;
                PasswordSuccessMessage = "Đổi mật khẩu thành công.";
                CurrentPassword = string.Empty;
                NewPassword = string.Empty;
                ConfirmPassword = string.Empty;
            }
            else
            {
                PasswordErrorMessage = resetResponse?.Message ?? "Đổi mật khẩu thất bại. Vui lòng thử lại.";
                PasswordSuccessMessage = string.Empty;
            }
        }

        [RelayCommand]
        public void CancelPassword()
        {
            PasswordErrorMessage = string.Empty;
            PasswordSuccessMessage = string.Empty;
            CurrentPassword = string.Empty;
            NewPassword = string.Empty;
            ConfirmPassword = string.Empty;
        }
    }
}