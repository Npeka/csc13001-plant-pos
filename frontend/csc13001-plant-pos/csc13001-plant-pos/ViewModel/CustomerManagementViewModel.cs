using System;
using System.Linq;
using csc13001_plant_pos.Service;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using csc13001_plant_pos.DTO.CustomerDTO;
using System.Threading.Tasks;
using OfficeOpenXml;
using Microsoft.UI.Xaml;
using System.IO;
using WinRT.Interop;
using System.Collections.Generic;
using Windows.Storage.Pickers;
using System.ComponentModel;
using csc13001_plant_pos.Model;

namespace csc13001_plant_pos.ViewModel
{
    public partial class CustomerManagementViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<CustomerDto> customerList;

        [ObservableProperty]
        private ObservableCollection<CustomerDto> filteredCustomerList;

        [ObservableProperty]
        private int newCustomersThisMonth;

        [ObservableProperty]
        private int premiumCustomers;

        [ObservableProperty]
        private int totalCustomers;

        [ObservableProperty]
        private string searchQuery;

        [ObservableProperty]
        private DateTimeOffset? dateQuery;

        [ObservableProperty]
        private string rankQuery;

        private readonly ICustomerService _customerService;

        public CustomerManagementViewModel(ICustomerService customerService)
        {
            _customerService = customerService;
            LoadCustomersDataAsync();
        }

        public void UpdateStatistics()
        {
            TotalCustomers = FilteredCustomerList.Count;

            NewCustomersThisMonth = FilteredCustomerList.Count(c =>
                c.Customer.CreateAt.HasValue &&
                c.Customer.CreateAt.Value.Month == DateTime.Now.Month &&
                c.Customer.CreateAt.Value.Year == DateTime.Now.Year);


            PremiumCustomers = FilteredCustomerList.Count(c =>
                c.Customer.LoyaltyCardType == "Gold" || c.Customer.LoyaltyCardType == "Platinum" || c.Customer.LoyaltyCardType == "Diamond");
            OnPropertyChanged(nameof(NewCustomersThisMonth));
            OnPropertyChanged(nameof(TotalCustomers));
            OnPropertyChanged(nameof(PremiumCustomers));
        }
        public async void LoadCustomersDataAsync()
        {

            var response = await _customerService.GetListCustomersAsync();
            if (response?.Status == "success" && response.Data != null)
            {
                CustomerList = new ObservableCollection<CustomerDto>(response.Data);
                FilteredCustomerList = new ObservableCollection<CustomerDto>(response.Data);
                UpdateStatistics();
            }
        }

        public void ApplyFilters()
        {
            FilteredCustomerList.Clear();
            var filtered = CustomerList.AsEnumerable();
            if (!string.IsNullOrEmpty(SearchQuery))
            {
                filtered = filtered.Where(emp =>
                emp.Customer.Name.ToLower().Contains(SearchQuery) ||
                emp.Customer.CustomerId.ToString().ToLower().Contains(SearchQuery));
            }

            if (DateQuery != null)
            {
                filtered = filtered.Where(emp => emp.Customer.CreateAt.HasValue && emp.Customer.CreateAt.Value.Date == DateQuery.Value.Date);
            }

            if (!string.IsNullOrEmpty(RankQuery) && RankQuery != "All")
            {
                filtered = filtered.Where(emp => emp.Customer.LoyaltyCardType.ToLower() == RankQuery.ToLower());
            }
            foreach (var customer in filtered)
            {
                FilteredCustomerList.Add(customer);
            }
            UpdateStatistics();
        }

        public void ResetFilter_Click()
        {
            SearchQuery = "";
            DateQuery = null;
            RankQuery = "All";

            FilteredCustomerList.Clear();
            foreach (var customer in CustomerList)
            {
                FilteredCustomerList.Add(customer);
            }
            UpdateStatistics();
        }
        public async Task<bool> DeleteCustomerAsync(string id)
        {
            var response = await _customerService.DeleteCustomerAsync(id);
            if (response)
            {
                var customerToRemove = CustomerList.FirstOrDefault(c => c.Customer.CustomerId.ToString() == id);
                if (customerToRemove != null)
                {
                    CustomerList.Remove(customerToRemove);
                    FilteredCustomerList.Remove(customerToRemove);

                    UpdateStatistics();
                    return true;
                }
            }
            return false;
        }
        private bool IsCustomerDuplicate(string email, string phone)
        {
            return CustomerList.Any(c =>
                c.Customer.Email.Equals(email, StringComparison.OrdinalIgnoreCase) ||
                c.Customer.Phone.Equals(phone));
        }

        public async Task<string?> AddCustomerAsync(CustomerDto data)
        {
            if (IsCustomerDuplicate(data.Customer.Email, data.Customer.Phone))
            {
                return "Email hoặc số điện thoại đã tồn tại trong hệ thống.";
            }
            var CustomerCreateDto = new CustomerCreateDto
            {
                Name = data.Customer.Name,
                Phone = data.Customer.Phone,
                Email = data.Customer.Email,
                Gender = data.Customer.Gender,
                Address = data.Customer.Address,
                BirthDate = data.Customer.BirthDate,
                LoyaltyCardType = data.Customer.LoyaltyCardType,
            };
            var response = await _customerService.AddCustomerAsync(CustomerCreateDto);
            if (response != null)
            {
                if (int.TryParse(response, out int parsedId))
                {
                    data.Customer.CustomerId = parsedId;
                    CustomerList.Add(data);
                    FilteredCustomerList.Add(data);
                    UpdateStatistics();
                    return "Tạo khách hàng mới thành công";
                }
                else
                {
                    return response;
                }
            }
            return response;
        }

        public async Task<string?> UpdateCustomerAsync(CustomerDto data)
        {
            if (IsCustomerDuplicate(data.Customer.Email, data.Customer.Phone))
            {
                return "Email hoặc số điện thoại đã tồn tại trong hệ thống.";
            }
            var response = await _customerService.UpdateCustomerAsync(data);
            var existingCustomer = CustomerList.FirstOrDefault(c => c.Customer.CustomerId == data.Customer.CustomerId);
            if (existingCustomer != null)
            {
                var index = CustomerList.IndexOf(existingCustomer);
                CustomerList[index] = data;
            }

            ResetFilter_Click();
            return response;
        }

        public async Task ExportToExcelAsync(Window window)
        {
            ExcelPackage.License.SetNonCommercialOrganization("My Noncommercial organization");

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Customers");

            // Header
            worksheet.Cells[1, 1].Value = "Customer ID";
            worksheet.Cells[1, 2].Value = "Name";
            worksheet.Cells[1, 3].Value = "Phone";
            worksheet.Cells[1, 4].Value = "Email";
            worksheet.Cells[1, 5].Value = "Gender";
            worksheet.Cells[1, 6].Value = "Address";
            worksheet.Cells[1, 7].Value = "Birth Date";
            worksheet.Cells[1, 8].Value = "Loyalty Points";
            worksheet.Cells[1, 9].Value = "Loyalty Card Type";
            worksheet.Cells[1, 10].Value = "Created At";
            worksheet.Cells[1, 11].Value = "Total Orders";
            worksheet.Cells[1, 12].Value = "Total Spent";

            // Data
            int row = 2;
            foreach (var item in CustomerList)
            {
                var c = item.Customer;
                worksheet.Cells[row, 1].Value = c.CustomerId;
                worksheet.Cells[row, 2].Value = c.Name;
                worksheet.Cells[row, 3].Value = c.Phone;
                worksheet.Cells[row, 4].Value = c.Email;
                worksheet.Cells[row, 5].Value = c.Gender;
                worksheet.Cells[row, 6].Value = c.Address;
                worksheet.Cells[row, 7].Value = c.BirthDate?.ToString("yyyy-MM-dd");
                worksheet.Cells[row, 8].Value = c.LoyaltyPoints;
                worksheet.Cells[row, 9].Value = c.LoyaltyCardType;
                worksheet.Cells[row, 10].Value = c.CreateAt?.ToString("yyyy-MM-dd HH:mm");
                worksheet.Cells[row, 11].Value = item.TotalOrders;
                worksheet.Cells[row, 12].Value = item.TotalSpent;
                row++;
            }

            // Format auto width
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

            // Mở file save picker
            var picker = new FileSavePicker();
            InitializeWithWindow.Initialize(picker, WindowNative.GetWindowHandle(window));
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            picker.SuggestedFileName = "exportCustomer";
            picker.FileTypeChoices.Add("Excel File", new List<string> { ".xlsx" });

            var file = await picker.PickSaveFileAsync();
            if (file != null)
            {
                using var stream = await file.OpenStreamForWriteAsync();
                await package.SaveAsAsync(stream);
            }
        }

        partial void OnSearchQueryChanged(string value)
        {
            ApplyFilters();
        }

        partial void OnRankQueryChanged(string value)
        {
            ApplyFilters();
        }

        partial void OnDateQueryChanged(DateTimeOffset? value)
        {
            ApplyFilters();
        }

    }
}
