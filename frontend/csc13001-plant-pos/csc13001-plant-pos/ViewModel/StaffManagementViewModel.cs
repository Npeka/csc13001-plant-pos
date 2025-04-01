using System;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using csc13001_plant_pos.Model;
using csc13001_plant_pos.Service;
using System.Collections.ObjectModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        private DateTime startDateQuery;

        [ObservableProperty]
        private string statusQuery;

        public string FilteredStaffCount => $"{filteredStaffList?.Count ?? 0} nhân viên";

        private readonly IStaffService _staffService;

        public StaffManagementViewModel(IStaffService staffService)
        {
            _staffService = staffService;
            LoadStaffsDataAsync();
        }
        public void PrintFilteredStaffList()
        {
            // Print each user's details in the filteredStaffList to debug
            System.Diagnostics.Debug.WriteLine("Filtered Staff List:");
            foreach (var user in filteredStaffList)
            {
                System.Diagnostics.Debug.WriteLine($"ID: {user.UserId}, FullName: {user.Fullname}, StartDate: {user.StartDate}, Status: {user.Status}");
            }
        }
        public async void LoadStaffsDataAsync()
        {

            var response = await _staffService.GetListStaffAsync();
            System.Diagnostics.Debug.WriteLine($"Status: {response?.Status}, Message: {response?.Message}");
            if (response?.Status == "success" && response.Data != null)
            {
                staffList = new ObservableCollection<User>(response.Data);
                filteredStaffList = new ObservableCollection<User>(response.Data);
                PrintFilteredStaffList();
            }
        }

        public void ApplyFilters()
        {
            filteredStaffList.Clear();
            var filtered = staffList.AsEnumerable();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                filtered = filtered.Where(emp =>
                emp.Fullname.ToLower().Contains(searchQuery) ||
                emp.UserId.ToString().ToLower().Contains(searchQuery));
            }

            if (StartDateQuery != default(DateTime))
            {
                filtered = filtered.Where(emp => emp.StartDate.Date == StartDateQuery.Date);
            }

            if (!string.IsNullOrEmpty(statusQuery) && statusQuery != "All")
            {
                filtered = filtered.Where(emp => emp.Status == statusQuery);
            }

            foreach (var user in filtered)
            {
                filteredStaffList.Add(user);
            }
        }

        public void ResetFilters()
        {
            SearchQuery = "";
            StartDateQuery = default(DateTime);
            StatusQuery = "All";
            filteredStaffList = new ObservableCollection<User>(staffList);
        }

        public void EditEmployee_Click(User user, XamlRoot xamlroot)
        {   
            ContentDialog dialog = new ContentDialog()
            {
                Title = "Thông báo",
                Content = $"Đã chỉnh sửa nhân viên: {user.Fullname}",
                CloseButtonText = "Đóng",
                XamlRoot = xamlroot
            };

            _ = dialog.ShowAsync();
            // Ham sua nhan vien
        }

        public async void ShowDeleteConfirmationDialog(User user, XamlRoot xamlroot)
        {
            ContentDialog deleteDialog = new ContentDialog()
            {
                Title = "Xác nhận xóa",
                Content = $"Bạn có chắc chắn muốn xóa nhân viên '{user.Fullname}' không?",
                PrimaryButtonText = "Xóa",
                CloseButtonText = "Hủy",
                DefaultButton = ContentDialogButton.Close,
                XamlRoot = xamlroot
            };

            ContentDialogResult result = await deleteDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                // Xóa nhân viên
                staffList.Remove(user);
                if (filteredStaffList.Contains(user))
                {
                    filteredStaffList.Remove(user);
                }
                // Ham xoa nhan vien
            }
        }

        public void SearchBox_TextChanged()
        {
            ApplyFilters();
        }

        public void DateFilter_DateChanged()
        {
            ApplyFilters();
        }

        public void StatusFilter_SelectionChanged()
        {
            ApplyFilters();
        }
    }
}
