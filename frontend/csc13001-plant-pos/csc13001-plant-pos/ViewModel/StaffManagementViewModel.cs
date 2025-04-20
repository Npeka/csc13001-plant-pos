using System;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using csc13001_plant_pos.Model;
using csc13001_plant_pos.Service;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.Collections.Generic;
using Windows.Storage.Pickers;
using WinRT.Interop;
using OfficeOpenXml;
using Microsoft.UI.Xaml;
using System.IO;
using Windows.Storage;

namespace csc13001_plant_pos.ViewModel
{
    public partial class StaffManagementViewModel: ObservableObject
    {
        [ObservableProperty]
        public ObservableCollection<User> staffList;

        [ObservableProperty]
        public ObservableCollection<User> filteredStaffList;

        [ObservableProperty]
        private string searchQuery;

        [ObservableProperty]
        private DateTimeOffset? selectedDate;

        [ObservableProperty]
        private string statusQuery;

        public string FilteredStaffCount => $"{FilteredStaffList?.Count ?? 0} nhân viên";

        private readonly IStaffService _staffService;

        public StaffManagementViewModel(IStaffService staffService)
        {
            _staffService = staffService;
            LoadStaffsDataAsync();
        }

        public async void LoadStaffsDataAsync()
        {

            var response = await _staffService.GetListStaffAsync();
            System.Diagnostics.Debug.WriteLine($"Status: {response?.Status}, Message: {response?.Message}");
            if (response?.Status == "success" && response.Data != null)
            {
                StaffList = new ObservableCollection<User>(response.Data);
                FilteredStaffList = new ObservableCollection<User>(response.Data);
            }
        }

        public void ApplyFilters()
        {
            FilteredStaffList.Clear();
            var filtered = StaffList.AsEnumerable();

            if (!string.IsNullOrEmpty(SearchQuery))
            {
                filtered = filtered.Where(emp =>
                emp.Fullname.ToLower().Contains(SearchQuery) ||
                emp.UserId.ToString().ToLower().Contains(SearchQuery));
            }

            if (SelectedDate.HasValue)
            {
                filtered = filtered.Where(emp => emp.StartDate?.Date == SelectedDate.Value.Date);
            }

            if (!string.IsNullOrEmpty(StatusQuery) && StatusQuery != "All")
            {
                filtered = filtered.Where(emp => emp.Status == StatusQuery);
            }

            foreach (var user in filtered)
            {
                FilteredStaffList.Add(user);
            }
            OnPropertyChanged(nameof(FilteredStaffCount));
        }


        public void ResetFilters()
        {
            SearchQuery = "";
            SelectedDate = null;
            StatusQuery = "All";
            FilteredStaffList.Clear();
            foreach (var user in StaffList)
            {
                FilteredStaffList.Add(user);
            }
        }
        private bool IsStaffDuplicate(string email, string phone)
        {
            return StaffList.Any(c =>
                c.Email.Equals(email, StringComparison.OrdinalIgnoreCase) ||
                c.Phone.Equals(phone));
        }

        public async Task<bool> UpdateStaffAsync(User user, StorageFile file)
        {
            if (IsStaffDuplicate(user.Email, user.Phone))
            {
                return false;
            }
            var response = await _staffService.UpdateStaffAsync(user, file);
            if (response)
            {
                var existingUser = StaffList.FirstOrDefault(u => u.UserId == user.UserId);
                if (existingUser != null)
                {
                    existingUser.Fullname = user.Fullname;
                    existingUser.Email = user.Email;
                    existingUser.Phone = user.Phone;
                    existingUser.Status = user.Status;
                    existingUser.Gender = user.Gender;
                    existingUser.IsAdmin = user.IsAdmin;
                }
                else
                {
                    StaffList.Add(user);
                    FilteredStaffList.Add(user);
                }

                ResetFilters();
                return true;
            }
            return false;
        }

        public async Task<bool> AddStaffAsync(User user, StorageFile file)
        {
            if (IsStaffDuplicate(user.Email, user.Phone))
            {
                return false;
            }
            var response = await _staffService.AddStaffAsync(user, file);
            if (response)
            {
                StaffList.Add(user);
                FilteredStaffList.Add(user);
                ResetFilters();
                return true;
            }
            return false;
        }

        public async Task ExportToExcelAsync(Window window)
        {
            ExcelPackage.License.SetNonCommercialOrganization("My Noncommercial organization");

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Staffs");

            worksheet.Cells[1, 1].Value = "User ID";
            worksheet.Cells[1, 2].Value = "Full Name";
            worksheet.Cells[1, 3].Value = "Start Date";
            worksheet.Cells[1, 4].Value = "Status";
            worksheet.Cells[1, 5].Value = "Gender";
            worksheet.Cells[1, 6].Value = "Is Admin";
            worksheet.Cells[1, 7].Value = "Email";
            worksheet.Cells[1, 8].Value = "Phone";
            worksheet.Cells[1, 9].Value = "Image URL";
            worksheet.Cells[1, 10].Value = "Manage Inventory";
            worksheet.Cells[1, 11].Value = "Manage Discount";
            // Data
            int row = 2;
            foreach (var u in StaffList)
            {
                worksheet.Cells[row, 1].Value = u.UserId;
                worksheet.Cells[row, 2].Value = u.Fullname;
                worksheet.Cells[row, 3].Value = u.StartDate?.ToString("yyyy-MM-dd");
                worksheet.Cells[row, 4].Value = u.Status;
                worksheet.Cells[row, 5].Value = u.Gender;
                worksheet.Cells[row, 6].Value = u.IsAdmin ? "Yes" : "No";
                worksheet.Cells[row, 7].Value = u.Email;
                worksheet.Cells[row, 8].Value = u.Phone;
                worksheet.Cells[row, 9].Value = u.ImageUrl;
                worksheet.Cells[row, 10].Value = u.CanManageInventory ? "Yes" : "No";
                worksheet.Cells[row, 11].Value = u.CanManageDiscounts ? "Yes" : "No";
                row++;
            }

            // Format auto width
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

            // Mở file save picker
            var picker = new FileSavePicker();
            InitializeWithWindow.Initialize(picker, WindowNative.GetWindowHandle(window));
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            picker.SuggestedFileName = "exportStaff";
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
            Debug.WriteLine($"SearchQuery to '{value}'");
            ApplyFilters();
        }

        partial void OnSelectedDateChanged(DateTimeOffset? value)
        {
            ApplyFilters();
        }

        partial void OnStatusQueryChanged(string value)
        {
            Debug.WriteLine($"SearchQuery to '{value}'");
            ApplyFilters();
        }
    }
}
