using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

        [ObservableProperty]
        private int currentPage = 1;

        [ObservableProperty]
        private int pageSize = 10;

        [ObservableProperty]
        private int totalPages;

        [ObservableProperty]
        private bool isAscending = true;

        private readonly int[] pageSizeOptions = { 5, 10, 20, 50 };
        private readonly IStaffService _staffService;
        private readonly UserSessionService _userSessionService;
        private ObservableCollection<OrderListDto> allStaffOrders = new ObservableCollection<OrderListDto>();

        public int[] PageSizeOptions => pageSizeOptions;

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

            filtered = isAscending
                ? filtered.OrderBy(o => o.OrderId)
                : filtered.OrderByDescending(o => o.OrderId);

            var totalItems = filtered.Count();
            TotalPages = (int)Math.Ceiling((double)totalItems / PageSize);

            if (CurrentPage > TotalPages) CurrentPage = TotalPages > 0 ? TotalPages : 1;

            filtered = filtered
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize);

            foreach (var order in filtered)
            {
                FilteredStaffOrders.Add(order);
            }
        }

        [RelayCommand]
        private void NextPage()
        {
            if (CurrentPage < TotalPages)
            {
                CurrentPage++;
                UpdateFilteredOrders();
            }
        }

        [RelayCommand]
        private void PreviousPage()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                UpdateFilteredOrders();
            }
        }

        [RelayCommand]
        private void ChangeSortOrder()
        {
            IsAscending = !IsAscending;
            UpdateFilteredOrders();
        }

        partial void OnSearchQueryChanged(string value)
        {
            CurrentPage = 1;
            UpdateFilteredOrders();
        }

        partial void OnSelectedDateChanged(DateTimeOffset? value)
        {
            CurrentPage = 1;
            UpdateFilteredOrders();
        }

        partial void OnPageSizeChanged(int value)
        {
            CurrentPage = 1;
            UpdateFilteredOrders();
        }
    }
}