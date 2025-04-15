using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using csc13001_plant_pos.DTO.OrderDTO;
using csc13001_plant_pos.Service;

namespace csc13001_plant_pos.ViewModel
{
    public partial class OrderViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<OrderListDto> orders = new ObservableCollection<OrderListDto>();

        [ObservableProperty]
        private string searchQuery;

        [ObservableProperty]
        private DateTimeOffset? selectedDate;

        [ObservableProperty]
        private ObservableCollection<OrderListDto> filteredOrders = new ObservableCollection<OrderListDto>();

        [ObservableProperty]
        private int currentPage = 1;

        [ObservableProperty]
        private int pageSize = 10;

        [ObservableProperty]
        private int totalPages;

        [ObservableProperty]
        private bool isAscending = true;

        private readonly int[] pageSizeOptions = { 5, 10, 20, 50 };
        private readonly IOrderService _orderService;
        private ObservableCollection<OrderListDto> allOrders = new ObservableCollection<OrderListDto>();

        public int[] PageSizeOptions => pageSizeOptions;

        public OrderViewModel(IOrderService orderService)
        {
            _orderService = orderService;
            LoadOrdersAsync();
        }

        public async void LoadOrdersAsync()
        {
            var response = await _orderService.GetAllOrdersAsync();
            System.Diagnostics.Debug.WriteLine($"Status: {response?.Status}, Message: {response?.Message}");
            if (response?.Status == "success" && response.Data != null)
            {
                allOrders.Clear();
                Orders.Clear();
                FilteredOrders.Clear();

                foreach (var order in response.Data)
                {
                    allOrders.Add(order);
                    Orders.Add(order);
                    FilteredOrders.Add(order);
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load orders: {response?.Message}");
            }

            UpdateFilteredOrders();
        }

        public void UpdateFilteredOrders()
        {
            FilteredOrders.Clear();
            var filtered = allOrders.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchQuery))
            {
                var query = SearchQuery.ToLower();
                filtered = filtered.Where(order =>
                    order.OrderId.ToString().Contains(query) ||
                    (order.Staff?.UserId.ToString().Contains(query) ?? false) ||
                    (order.Staff?.Fullname?.ToLower().Contains(query) ?? false) ||
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
                FilteredOrders.Add(order);
            }
        }

        public async Task<bool> DeleteOrderAsync(OrderListDto order)
        {
            if (order == null) return false;
            bool success = await _orderService.DeleteOrderAsync(order.OrderId.ToString());
            if (success)
            {
                allOrders.Remove(order);
                Orders.Remove(order);
                FilteredOrders.Remove(order);
                System.Diagnostics.Debug.WriteLine($"Order {order.OrderId} deleted successfully.");
                UpdateFilteredOrders();
            }
            return success;
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