using System;
using System.Collections.Generic;
using System.Linq;
using csc13001_plant_pos.Model;
using csc13001_plant_pos.Service;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using csc13001_plant_pos.DTO.CustomerDTO;
using Windows.System;
using System.Diagnostics;

namespace csc13001_plant_pos.ViewModel
{
    public partial class CustomerManagementViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<CustomerDto> customerList;

        [ObservableProperty]
        public ObservableCollection<CustomerDto> filteredCustomerList;

        [ObservableProperty]
        public int newCustomersThisMonth;

        [ObservableProperty]
        public int premiumCustomers;

        [ObservableProperty]
        public int totalCustomers;

        [ObservableProperty]
        public string searchQuery;

        [ObservableProperty]
        public DateTime startDateQuery;

        [ObservableProperty]
        public string rankQuery;

        private readonly ICustomerService _customerService;

        public CustomerManagementViewModel(ICustomerService customerService)
        {
            _customerService = customerService;
            LoadCustomersDataAsync();
        }

        public void UpdateStatistics()
        {
            // Cập nhật tổng số khách hàng
            TotalCustomers = customerList.Count;

            // Cập nhật số khách hàng mới trong tháng hiện tại
            int currentMonth = DateTime.Now.Month;
            int currentYear = DateTime.Now.Year;
            //NewCustomersThisMonth = customerList.Count(c =>
            //    DateTime.TryParseExact(c.CreatedAt, "dd/MM/yyyy",
            //    System.Globalization.CultureInfo.InvariantCulture,
            //    System.Globalization.DateTimeStyles.None, out DateTime createdAtDate) &&
            //    createdAtDate.Month == currentMonth &&
            //    createdAtDate.Year == currentYear);

            // Cập nhật số khách hàng cao cấp (Vàng, Bạch Kim, Kim Cương)
            PremiumCustomers = customerList.Count(c =>
                c.Customer.LoyaltyCardType == "Gold" || c.Customer.LoyaltyCardType == "Platinum" || c.Customer.LoyaltyCardType == "Diamond");
        }
        public async void LoadCustomersDataAsync()
        {

            var response = await _customerService.GetListCustomersAsync();
            System.Diagnostics.Debug.WriteLine($"Status: {response?.Status}, Message: {response?.Message}");
            if (response?.Status == "success" && response.Data != null)
            {
                customerList = new ObservableCollection<CustomerDto>(response.Data);
                filteredCustomerList = new ObservableCollection<CustomerDto>(response.Data);
                UpdateStatistics();
            }
        }

        public void ApplyFilters()
        {
            filteredCustomerList.Clear();
            var filtered = customerList.AsEnumerable();
            if (!string.IsNullOrEmpty(searchQuery))
            {
                filtered = filtered.Where(emp =>
                emp.Customer.Name.ToLower().Contains(searchQuery) ||
                emp.Customer.CustomerId.ToString().ToLower().Contains(searchQuery));
            }

            //if (startDateQuery != null)
            //{
            //    filtered = filtered.Where(emp => emp.CreatedAt.Date >= startDateQuery.Date);
            //}

            if (!string.IsNullOrEmpty(rankQuery) && rankQuery != "All")
            {
                filtered = filtered.Where(emp => emp.Customer.LoyaltyCardType.ToLower() == rankQuery.ToLower());
            }
            foreach (var customer in filtered)
            {
                filteredCustomerList.Add(customer);
            }
        }

        public void ResetFilter_Click()
        {
            SearchQuery = "";
            startDateQuery = default(DateTime);
            rankQuery = "";

            filteredCustomerList.Clear();
            foreach (var customer in customerList)
            {
                filteredCustomerList.Add(customer);
            }
        }

        public void DateFilter_DateChanged()
        {
            ApplyFilters();
        }

        partial void OnSearchQueryChanged(string value)
        {
            Debug.WriteLine($"SearchQuery to '{value}'");
            ApplyFilters();
        }

        partial void OnRankQueryChanged(string value)
        {
            Debug.WriteLine($"SearchQuery to '{value}'");
            ApplyFilters();
        }
        //public async void ShowUpdateCustomerDialog(Customer customer, XamlRoot xamlroot)
        //{
        //    ContentDialog dialog = new ContentDialog
        //    {
        //        Title = "Cập nhật thông tin khách hàng",
        //        PrimaryButtonText = "Lưu",
        //        CloseButtonText = "Hủy",
        //        DefaultButton = ContentDialogButton.Primary,
        //        XamlRoot = xamlroot
        //    };

        //    // Tạo form cập nhật
        //    var panel = new StackPanel { Spacing = 10 };

        //    panel.Children.Add(new TextBlock { Text = "Họ và tên:" });
        //    var nameBox = new TextBox { Text = customer.Name, PlaceholderText = "Nhập họ tên" };
        //    panel.Children.Add(nameBox);

        //    panel.Children.Add(new TextBlock { Text = "Số điện thoại:" });
        //    var phoneBox = new TextBox { Text = customer.Phone, PlaceholderText = "Nhập số điện thoại" };
        //    panel.Children.Add(phoneBox);

        //    panel.Children.Add(new TextBlock { Text = "Điểm số:" });
        //    var pointsBox = new NumberBox { Value = customer.Points, SpinButtonPlacementMode = NumberBoxSpinButtonPlacementMode.Compact };
        //    panel.Children.Add(pointsBox);

        //    panel.Children.Add(new TextBlock { Text = "Hạng thẻ:" });
        //    var rankCombo = new ComboBox();
        //    rankCombo.Items.Add("Đồng");
        //    rankCombo.Items.Add("Bạc");
        //    rankCombo.Items.Add("Vàng");
        //    rankCombo.Items.Add("Bạch Kim");
        //    rankCombo.Items.Add("Kim Cương");
        //    rankCombo.SelectedItem = customer.Rank;
        //    panel.Children.Add(rankCombo);

        //    dialog.Content = panel;

        //    var result = await dialog.ShowAsync();
        //    if (result == ContentDialogResult.Primary)
        //    {
        //        // Cập nhật thông tin khách hàng
        //        customer.Name = nameBox.Text;
        //        customer.Phone = phoneBox.Text;
        //        customer.Points = (int)pointsBox.Value;
        //        customer.Rank = rankCombo.SelectedItem.ToString();


        //        // Áp dụng lại bộ lọc
        //        ApplyFilters();
        //    }
        //}

        public void UpdateCustomer_Click(Customer customer)
        {

            // Hiển thị dialog cập nhật thông tin khách hàng
            //ShowUpdateCustomerDialog(customer);
        }

        public async void DeleteCustomer_Click(CustomerDto customer)
        {

            // Confirm deletion with the user
            ContentDialog confirmDialog = new ContentDialog
            {
                Title = "Xác nhận xóa khách hàng",
                Content = $"Bạn có chắc chắn muốn xóa khách hàng {customer.Customer.Name} (ID: {customer.Customer.CustomerId})?",
                PrimaryButtonText = "Xóa",
                CloseButtonText = "Hủy",
                DefaultButton = ContentDialogButton.Close,
                //XamlRoot = this.XamlRoot
            };

            var result = await confirmDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                // Remove from both collections
                customerList.Remove(customer);
                if (filteredCustomerList.Contains(customer))
                {
                    filteredCustomerList.Remove(customer);
                }

                // Update statistics after deletion
                //UpdateStatistics();
            }
        }
    }
}
