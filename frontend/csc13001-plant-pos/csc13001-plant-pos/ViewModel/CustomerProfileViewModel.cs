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
    public partial class CustomerProfileViewModel : ObservableObject
    {
        [ObservableProperty]
        private Customer customer;

        [ObservableProperty]
        private int totalOrders;

        [ObservableProperty]
        private decimal totalSpent;

        [ObservableProperty]
        private ObservableCollection<OrderListDto> customerOrders = new ObservableCollection<OrderListDto>();

        [ObservableProperty]
        private string searchQuery;

        [ObservableProperty]
        private DateTimeOffset? selectedDate;

        [ObservableProperty]
        private ObservableCollection<OrderListDto> filteredCustomerOrders = new ObservableCollection<OrderListDto>();

        [ObservableProperty]
        private int currentPage = 1;

        [ObservableProperty]
        private int pageSize = 10;

        [ObservableProperty]
        private int totalPages;

        [ObservableProperty]
        private bool isAscending = true;

        private readonly int[] pageSizeOptions = { 5, 10, 20, 50 };
        private readonly ICustomerService _customerService;
        private ObservableCollection<OrderListDto> allCustomerOrders = new ObservableCollection<OrderListDto>();

        public int[] PageSizeOptions => pageSizeOptions;

        public CustomerProfileViewModel(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task LoadCustomerDataAsync(string customerId)
        {
            var response = await _customerService.GetCustomerByIdAsync(customerId);
            System.Diagnostics.Debug.WriteLine($"Status: {response?.Status}, Message: {response?.Message}");
            if (response?.Status == "success" && response.Data != null)
            {
                Customer = response.Data.Customer;
                TotalOrders = response.Data.TotalOrders;
                TotalSpent = response.Data.TotalSpent;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load customer {customerId}: {response?.Message}");
            }
        }

        public async Task LoadCustomerOrdersAsync(string customerId)
        {
            var response = await _customerService.GetCustomerOrdersAsync(customerId);
            System.Diagnostics.Debug.WriteLine($"Status: {response?.Status}, Message: {response?.Message}");
            if (response?.Status == "success" && response.Data != null)
            {
                allCustomerOrders.Clear();
                CustomerOrders.Clear();
                FilteredCustomerOrders.Clear();

                foreach (var order in response.Data)
                {
                    allCustomerOrders.Add(order);
                    CustomerOrders.Add(order);
                    FilteredCustomerOrders.Add(order);
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load customer orders {customerId}: {response?.Message}");
            }

            UpdateFilteredOrders();
        }

        private void UpdateFilteredOrders()
        {
            FilteredCustomerOrders.Clear();
            var filtered = allCustomerOrders.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchQuery))
            {
                var query = SearchQuery.ToLower();
                filtered = filtered.Where(order =>
                    order.OrderId.ToString().Contains(query) ||
                    (order.Staff?.UserId.ToString().Contains(query) ?? false) ||
                    (order.Staff?.Fullname?.ToLower().Contains(query) ?? false));
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
                FilteredCustomerOrders.Add(order);
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