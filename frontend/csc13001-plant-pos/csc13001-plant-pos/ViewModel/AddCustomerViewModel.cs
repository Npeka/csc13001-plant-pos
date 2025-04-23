using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using csc13001_plant_pos.DTO.CustomerDTO;
using csc13001_plant_pos.Service;

namespace csc13001_plant_pos.ViewModel
{
    public partial class AddCustomerViewModel : ObservableObject
    {
        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string phone;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string gender = "Male";

        [ObservableProperty]
        private bool isMaleChecked = true;

        [ObservableProperty]
        private bool isFemaleChecked = false;

        [ObservableProperty]
        private string address;

        [ObservableProperty]
        private DateTimeOffset? dateOfBirth;

        [ObservableProperty]
        private string errorMessage;

        [ObservableProperty]
        private bool isErrorVisible;

        private readonly ICustomerService _customerService;

        public AddCustomerViewModel(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [RelayCommand]
        public async Task<string?> AddCustomer()
        {
            ErrorMessage = string.Empty;
            IsErrorVisible = false;

            if (string.IsNullOrWhiteSpace(Name) ||
                string.IsNullOrWhiteSpace(Phone) ||
                string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Address))
            {
                ErrorMessage = "Vui lòng điền đầy đủ các thông tin bắt buộc (Tên, Số điện thoại, Email, Địa chỉ).";
                IsErrorVisible = true;
                return null;
            }

            if (!Regex.IsMatch(Email?.Trim(), @"^[a-zA-Z0-9._%+-]+@gmail\.com$"))
            {
                ErrorMessage = "Email phải có định dạng hợp lệ và sử dụng đuôi @gmail.com.";
                IsErrorVisible = true;
                return null;
            }

            if (!Regex.IsMatch(Phone, @"^0[0-9]{9,10}$"))
            {
                ErrorMessage = "Số điện thoại phải bắt đầu bằng số 0 và có 10 hoặc 11 chữ số.";
                IsErrorVisible = true;
                return null;
            }

            var customerDto = new CustomerCreateDto
            {
                Name = Name,
                Phone = Phone,
                Email = Email,
                Gender = Gender,
                Address = Address,
                LoyaltyCardType = "All"
            };

            var customerId = await _customerService.AddCustomerAsync(customerDto);
            if (int.TryParse(customerId, out _))
            {
                System.Diagnostics.Debug.WriteLine($"Customer added successfully with ID: {customerId}");
                ResetForm();
                IsErrorVisible = false;
            }
            else
            {
                if (customerId != null)
                    ErrorMessage = customerId;
                IsErrorVisible = true;
            }

            return customerId;
        }

        [RelayCommand]
        public void ResetForm()
        {
            Name = string.Empty;
            Phone = string.Empty;
            Email = string.Empty;
            Gender = "Male";
            IsMaleChecked = true;
            IsFemaleChecked = false;
            Address = string.Empty;
            DateOfBirth = null;
            ErrorMessage = string.Empty;
            IsErrorVisible = false;
        }

        partial void OnIsMaleCheckedChanged(bool value)
        {
            if (value)
            {
                Gender = "Male";
                IsFemaleChecked = false;
            }
        }

        partial void OnIsFemaleCheckedChanged(bool value)
        {
            if (value)
            {
                Gender = "Female";
                IsMaleChecked = false;
            }
        }
    }
}