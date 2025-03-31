using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using csc13001_plant_pos.DTO.OrderDTO;
using csc13001_plant_pos.Service;
using Microsoft.UI.Xaml.Controls;

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

        private readonly IOrderService _orderService;
        private ObservableCollection<OrderListDto> allOrders = new ObservableCollection<OrderListDto>();

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

        private void UpdateFilteredOrders()
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

            foreach (var order in filtered)
            {
                FilteredOrders.Add(order);
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