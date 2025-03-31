using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using csc13001_plant_pos.DTO.InventoryDTO;
using csc13001_plant_pos.Service;

namespace csc13001_plant_pos.ViewModel
{
    public partial class WarehouseManagementViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<InventoryListDto> inventoryOrders = new ObservableCollection<InventoryListDto>();

        [ObservableProperty]
        private string searchQuery;

        [ObservableProperty]
        private DateTimeOffset? selectedDate;

        [ObservableProperty]
        private ObservableCollection<InventoryListDto> filteredInventoryOrders = new ObservableCollection<InventoryListDto>();

        private readonly IInventoryService _inventoryService;
        private ObservableCollection<InventoryListDto> allInventoryOrders = new ObservableCollection<InventoryListDto>();

        public WarehouseManagementViewModel(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
            LoadInventoryOrdersAsync();
        }

        public async void LoadInventoryOrdersAsync()
        {
            var response = await _inventoryService.GetAllInventoriesAsync();
            System.Diagnostics.Debug.WriteLine($"Status: {response?.Status}, Message: {response?.Message}");
            if (response?.Status == "success" && response.Data != null)
            {
                allInventoryOrders.Clear();
                InventoryOrders.Clear();
                FilteredInventoryOrders.Clear();

                foreach (var order in response.Data)
                {
                    allInventoryOrders.Add(order);
                    InventoryOrders.Add(order);
                    FilteredInventoryOrders.Add(order);
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load inventory orders: {response?.Message}");
            }

            UpdateFilteredOrders();
        }

        private void UpdateFilteredOrders()
        {
            FilteredInventoryOrders.Clear();
            var filtered = allInventoryOrders.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchQuery))
            {
                var query = SearchQuery.ToLower();
                filtered = filtered.Where(order =>
                    order.InventoryId.ToString().Contains(query) ||
                    (order.Supplier?.ToLower().Contains(query) ?? false));
            }

            if (SelectedDate.HasValue)
            {
                var selectedDateOnly = SelectedDate.Value.Date;
                filtered = filtered.Where(order => order.PurchaseDate.Date == selectedDateOnly);
            }

            foreach (var order in filtered)
            {
                FilteredInventoryOrders.Add(order);
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