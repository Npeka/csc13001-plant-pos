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
                filtered = filtered.Where(emp => emp.StartDate.Date == SelectedDate.Value.Date);
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

        public async Task<bool> UpdateStaffAsync(User user)
        {
            var response = await _staffService.UpdateStaffAsync(user);
            if (response?.Status == "success")
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
                Debug.WriteLine($"User updated: {user.Fullname}, {user.Status}, {user.Gender}, {user.IsAdmin}");
                return true;
            }
            return false;
        }

        public async Task<bool> AddStaffAsync(User user)
        {
            var response = await _staffService.AddStaffAsync(user);
            if (response?.Status == "success")
            {
                StaffList.Add(user);
                FilteredStaffList.Add(user);
                ResetFilters();
                return false;
            }
            return false;
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
