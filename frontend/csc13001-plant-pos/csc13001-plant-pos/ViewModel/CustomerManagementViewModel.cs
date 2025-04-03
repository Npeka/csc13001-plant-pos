using System;
using System.Linq;
using csc13001_plant_pos.Service;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using csc13001_plant_pos.DTO.CustomerDTO;
using System.Threading.Tasks;

namespace csc13001_plant_pos.ViewModel
{
    public partial class CustomerManagementViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<CustomerDto> customerList;

        [ObservableProperty]
        private ObservableCollection<CustomerDto> filteredCustomerList;

        [ObservableProperty]
        private int newCustomersThisMonth;

        [ObservableProperty]
        private int premiumCustomers;

        [ObservableProperty]
        private int totalCustomers;

        [ObservableProperty]
        private string searchQuery;

        [ObservableProperty]
        private DateTimeOffset? dateQuery;

        [ObservableProperty]
        private string rankQuery;

        private readonly ICustomerService _customerService;

        public CustomerManagementViewModel(ICustomerService customerService)
        {
            _customerService = customerService;
            LoadCustomersDataAsync();
        }

        public void UpdateStatistics()
        {
            TotalCustomers = FilteredCustomerList.Count;

            NewCustomersThisMonth = FilteredCustomerList.Count(c =>
                c.Customer.CreateAt.HasValue &&
                c.Customer.CreateAt.Value.Month == DateTime.Now.Month &&
                c.Customer.CreateAt.Value.Year == DateTime.Now.Year);


            PremiumCustomers = FilteredCustomerList.Count(c =>
                c.Customer.LoyaltyCardType == "Gold" || c.Customer.LoyaltyCardType == "Platinum" || c.Customer.LoyaltyCardType == "Diamond");
            OnPropertyChanged(nameof(NewCustomersThisMonth));
            OnPropertyChanged(nameof(TotalCustomers));
            OnPropertyChanged(nameof(PremiumCustomers));
        }
        public async void LoadCustomersDataAsync()
        {

            var response = await _customerService.GetListCustomersAsync();
            if (response?.Status == "success" && response.Data != null)
            {
                CustomerList = new ObservableCollection<CustomerDto>(response.Data);
                FilteredCustomerList = new ObservableCollection<CustomerDto>(response.Data);
                UpdateStatistics();
            }
        }

        public void ApplyFilters()
        {
            FilteredCustomerList.Clear();
            var filtered = CustomerList.AsEnumerable();
            if (!string.IsNullOrEmpty(SearchQuery))
            {
                filtered = filtered.Where(emp =>
                emp.Customer.Name.ToLower().Contains(SearchQuery) ||
                emp.Customer.CustomerId.ToString().ToLower().Contains(SearchQuery));
            }

            if (DateQuery != null)
            {
                filtered = filtered.Where(emp => emp.Customer.CreateAt.HasValue && emp.Customer.CreateAt.Value.Date == DateQuery.Value.Date);
            }

            if (!string.IsNullOrEmpty(RankQuery) && RankQuery != "All")
            {
                filtered = filtered.Where(emp => emp.Customer.LoyaltyCardType.ToLower() == RankQuery.ToLower());
            }
            foreach (var customer in filtered)
            {
                FilteredCustomerList.Add(customer);
            }
            UpdateStatistics();
        }

        public void ResetFilter_Click()
        {
            SearchQuery = "";
            DateQuery = null;
            RankQuery = "All";

            FilteredCustomerList.Clear();
            foreach (var customer in CustomerList)
            {
                FilteredCustomerList.Add(customer);
            }
            UpdateStatistics();
        }
        public async Task<bool> DeleteCustomerAsync(string id)
        {
            var response = await _customerService.DeleteCustomerAsync(id);
            if (response)
            {
                var customerToRemove = CustomerList.FirstOrDefault(c => c.Customer.CustomerId.ToString() == id);
                if (customerToRemove != null)
                {
                    CustomerList.Remove(customerToRemove);
                    FilteredCustomerList.Remove(customerToRemove);

                    UpdateStatistics();
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> AddCustomerAsync(CustomerDto data)
        {
            var response = await _customerService.AddCustomerAsync(data);
            if (response != null)
            {
                var customerId = response;
                data.Customer.CustomerId = int.Parse(customerId);
                CustomerList.Add(data);
                FilteredCustomerList.Add(data);
                UpdateStatistics();
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateCustomerAsync(CustomerDto data)
        {
            var response = await _customerService.UpdateCustomerAsync(data);
            if (response)
            {
                
                
                UpdateStatistics();
                return true;
            }
            return false;
        }
        partial void OnSearchQueryChanged(string value)
        {
            ApplyFilters();
        }

        partial void OnRankQueryChanged(string value)
        {
            ApplyFilters();
        }

        partial void OnDateQueryChanged(DateTimeOffset? value)
        {
            ApplyFilters();
        }

    }
}
