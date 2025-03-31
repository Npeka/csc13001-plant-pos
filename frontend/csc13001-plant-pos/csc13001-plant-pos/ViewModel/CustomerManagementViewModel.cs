using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using csc13001_plant_pos.Model;
using csc13001_plant_pos.Service;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace csc13001_plant_pos.ViewModel
{
    public partial class CustomerManagementViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<User> customerList;

        [ObservableProperty]
        private ObservableCollection<User> filteredCustomerList;

        [ObservableProperty]
        private int newCustomersThisMonth;

        [ObservableProperty]
        private int premiumCustomers;


        [ObservableProperty]
        private string searchQuery;
        [ObservableProperty]
        private DateTime startDateQuery;
        [ObservableProperty]
        private string statusQuery;

        private readonly ICustomerService _customerService;
        //public CustomerManagementViewModel(ICustomerService customerService)
        //{
        //    _customerService = customerService;
        //    LoadCustomersDataAsync();
        //}
        //public async void LoadCustomersDataAsync()
        //{
        //    var response = await _customerService.GetListCustomerAsync();
        //    System.Diagnostics.Debug.WriteLine($"Status: {response?.Status}, Message: {response?.Message}");
        //    if (response?.Status == "success" && response.Data != null)
        //    {
        //        customerList = new ObservableCollection<User>(response.Data);
        //        filteredCustomerList = new ObservableCollection<User>(response.Data);
        //    }
        //}
        public void ApplyFilters()
        {
            filteredCustomerList.Clear();
            var filtered = customerList.AsEnumerable();
            if (!string.IsNullOrEmpty(searchQuery))
            {
                filtered = filtered.Where(emp =>
                emp.Fullname.ToLower().Contains(searchQuery) ||
                emp.UserId.ToString().ToLower().Contains(searchQuery));
            }
        }
    }
}
