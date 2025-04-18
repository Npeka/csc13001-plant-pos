﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

        [ObservableProperty]
        private int currentPage = 1;

        [ObservableProperty]
        private int pageSize = 10;

        [ObservableProperty]
        private int totalPages;

        [ObservableProperty]
        private bool isAscending = true;

        private readonly int[] pageSizeOptions = { 5, 10, 20, 50 };
        private readonly IInventoryService _inventoryService;
        private ObservableCollection<InventoryListDto> allInventoryOrders = new ObservableCollection<InventoryListDto>();

        public int[] PageSizeOptions => pageSizeOptions;

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

        public void UpdateFilteredOrders()
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

            filtered = isAscending
                ? filtered.OrderBy(o => o.InventoryId)
                : filtered.OrderByDescending(o => o.InventoryId);

            var totalItems = filtered.Count();
            TotalPages = (int)Math.Ceiling((double)totalItems / PageSize);

            if (CurrentPage > TotalPages) CurrentPage = TotalPages > 0 ? TotalPages : 1;

            filtered = filtered
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize);

            foreach (var order in filtered)
            {
                FilteredInventoryOrders.Add(order);
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