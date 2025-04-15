using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using csc13001_plant_pos.Model;
using csc13001_plant_pos.Service;

namespace csc13001_plant_pos.ViewModel
{
    public partial class DiscountManagementViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<DiscountProgram> discounts = new ObservableCollection<DiscountProgram>();

        [ObservableProperty]
        private string searchQuery;

        [ObservableProperty]
        private DateTimeOffset? selectedDate;

        [ObservableProperty]
        private ObservableCollection<DiscountProgram> filteredDiscounts = new ObservableCollection<DiscountProgram>();

        [ObservableProperty]
        private int currentPage = 1;

        [ObservableProperty]
        private int pageSize = 10;

        [ObservableProperty]
        private int totalPages;

        [ObservableProperty]
        private bool isAscending = true;

        private readonly int[] pageSizeOptions = { 5, 10, 20, 50 };
        private readonly IDiscountProgramService _discountProgramService;
        private ObservableCollection<DiscountProgram> allDiscounts = new ObservableCollection<DiscountProgram>();

        public int[] PageSizeOptions => pageSizeOptions;

        public DiscountManagementViewModel(IDiscountProgramService discountService)
        {
            _discountProgramService = discountService;
            LoadDiscountsAsync();
        }

        public async void LoadDiscountsAsync()
        {
            var response = await _discountProgramService.GetAllDiscountsAsync();
            System.Diagnostics.Debug.WriteLine($"Status: {response?.Status}, Message: {response?.Message}");
            if (response?.Status == "success" && response.Data != null)
            {
                allDiscounts.Clear();
                Discounts.Clear();
                FilteredDiscounts.Clear();

                foreach (var discount in response.Data)
                {
                    allDiscounts.Add(discount);
                    Discounts.Add(discount);
                    FilteredDiscounts.Add(discount);
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load discounts: {response?.Message}");
            }

            UpdateFilteredDiscounts();
        }

        private void UpdateFilteredDiscounts()
        {
            FilteredDiscounts.Clear();
            var filtered = allDiscounts.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchQuery))
            {
                var query = SearchQuery.ToLower();
                filtered = filtered.Where(discount =>
                    discount.DiscountId.ToString().Contains(query) ||
                    (discount.Name?.ToLower().Contains(query) ?? false));
            }

            if (SelectedDate.HasValue)
            {
                var selectedDateOnly = SelectedDate.Value.Date;
                filtered = filtered.Where(discount =>
                    discount.StartDate.Date <= selectedDateOnly && discount.EndDate.Date >= selectedDateOnly);
            }

            filtered = isAscending
                ? filtered.OrderBy(d => d.DiscountId)
                : filtered.OrderByDescending(d => d.DiscountId);

            var totalItems = filtered.Count();
            TotalPages = (int)Math.Ceiling((double)totalItems / PageSize);

            if (CurrentPage > TotalPages) CurrentPage = TotalPages > 0 ? TotalPages : 1;

            filtered = filtered
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize);

            foreach (var discount in filtered)
            {
                FilteredDiscounts.Add(discount);
            }
        }

        public async Task<bool> CreateDiscountAsync(DiscountProgram discount)
        {
            bool success = await _discountProgramService.CreateDiscountAsync(discount);
            if (success)
            {
                LoadDiscountsAsync();
            }
            return success;
        }

        public async Task<bool> UpdateDiscountAsync(DiscountProgram discount)
        {
            bool success = await _discountProgramService.UpdateDiscountAsync(discount.DiscountId, discount);
            if (success)
            {
                LoadDiscountsAsync();
            }
            return success;
        }

        [RelayCommand]
        private void NextPage()
        {
            if (CurrentPage < TotalPages)
            {
                CurrentPage++;
                UpdateFilteredDiscounts();
            }
        }

        [RelayCommand]
        private void PreviousPage()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                UpdateFilteredDiscounts();
            }
        }

        [RelayCommand]
        private void ChangeSortOrder()
        {
            IsAscending = !IsAscending;
            UpdateFilteredDiscounts();
        }

        partial void OnSearchQueryChanged(string value)
        {
            CurrentPage = 1;
            UpdateFilteredDiscounts();
        }

        partial void OnSelectedDateChanged(DateTimeOffset? value)
        {
            CurrentPage = 1;
            UpdateFilteredDiscounts();
        }

        partial void OnPageSizeChanged(int value)
        {
            CurrentPage = 1;
            UpdateFilteredDiscounts();
        }
    }
}