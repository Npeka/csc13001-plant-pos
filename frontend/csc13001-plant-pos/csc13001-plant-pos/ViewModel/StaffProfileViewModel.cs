﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using csc13001_plant_pos.DTO.OrderDTO;
using csc13001_plant_pos.Model;
using csc13001_plant_pos.Service;

namespace csc13001_plant_pos.ViewModel
{
    public partial class StaffProfileViewModel : ObservableObject
    {
        [ObservableProperty]
        private User staffUser;

        [ObservableProperty]
        private int totalOrders;

        [ObservableProperty]
        private decimal totalRevenue;

        [ObservableProperty]
        private ObservableCollection<OrderListDto> staffOrders = new ObservableCollection<OrderListDto>();

        [ObservableProperty]
        private string searchQuery;

        [ObservableProperty]
        private DateTimeOffset? selectedDate;

        [ObservableProperty]
        private ObservableCollection<OrderListDto> filteredStaffOrders = new ObservableCollection<OrderListDto>();

        private readonly IStaffService _staffService;
        private readonly UserSessionService _userSessionService;
        private ObservableCollection<OrderListDto> allStaffOrders = new ObservableCollection<OrderListDto>();

        public StaffProfileViewModel(IStaffService staffService, UserSessionService userSessionService)
        {
            _staffService = staffService;
            _userSessionService = userSessionService;
            LoadStaffDataAsync();
            LoadStaffOrdersAsync();
        }

        public async void LoadStaffDataAsync()
        {
            var userId = _userSessionService.CurrentUser?.UserId ?? 0;
            if (userId == 0) return;

            var response = await _staffService.GetStaffByIdAsync(userId);
            System.Diagnostics.Debug.WriteLine($"Status: {response?.Status}, Message: {response?.Message}");
            if (response?.Status == "success" && response.Data != null)
            {
                StaffUser = response.Data.User;
                TotalOrders = response.Data.TotalOrders;
                TotalRevenue = response.Data.TotalRevenue;
            }
        }

        public async void LoadStaffOrdersAsync()
        {
            var userId = _userSessionService.CurrentUser?.UserId ?? 0;
            if (userId == 0) return;

            var response = await _staffService.GetStaffOrdersAsync(userId);
            System.Diagnostics.Debug.WriteLine($"Status: {response?.Status}, Message: {response?.Message}");
            if (response?.Status == "success" && response.Data != null)
            {
                allStaffOrders.Clear();
                StaffOrders.Clear();
                FilteredStaffOrders.Clear();

                foreach (var order in response.Data)
                {
                    allStaffOrders.Add(order);
                    StaffOrders.Add(order);
                    FilteredStaffOrders.Add(order);
                }
            }

            UpdateFilteredOrders();
        }
        private void UpdateFilteredOrders()
        {
            FilteredStaffOrders.Clear();
            var filtered = allStaffOrders.AsEnumerable();
            if (!string.IsNullOrWhiteSpace(SearchQuery))
            {
                var query = SearchQuery.ToLower();
                filtered = filtered.Where(order =>
                    order.OrderId.ToString().Contains(query) ||
                    (order.Customer?.CustomerId.ToString().Contains(query) ?? false) ||
                    (order.Customer?.Name?.ToLower().Contains(query) ?? false));
            }
            if (SelectedDate.HasValue)
            {
                var selectedDateOnly = SelectedDate.Value.Date;
                filtered = filtered.Where(order => order.OrderDate.Date == selectedDateOnly);
            }
            foreach (var order in filtered)
            {
                FilteredStaffOrders.Add(order);
            }
        }
        partial void OnSearchQueryChanged(string value)
        {
            UpdateFilteredOrders();
        }
        partial void OnSelectedDateChanged(DateTimeOffset? value)
        {
            UpdateFilteredOrders();
        }
    }
}