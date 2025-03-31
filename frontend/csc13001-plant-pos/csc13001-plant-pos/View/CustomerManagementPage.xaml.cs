using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace csc13001_plant_pos.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CustomerManagementPage : Page, INotifyPropertyChanged
    {
        public ObservableCollection<Customer> Customers { get; set; }
        public ObservableCollection<Customer> FilteredUsers { get; set; }

        private int _totalCustomers;
        public int TotalCustomers
        {
            get => _totalCustomers;
            set
            {
                _totalCustomers = value;
                OnPropertyChanged();
            }
        }

        private int _newCustomersThisMonth;
        public int NewCustomersThisMonth
        {
            get => _newCustomersThisMonth;
            set
            {
                _newCustomersThisMonth = value;
                OnPropertyChanged();
            }
        }

        private int _premiumCustomers;
        public int PremiumCustomers
        {
            get => _premiumCustomers;
            set
            {
                _premiumCustomers = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CustomerManagementPage()
        {
            this.InitializeComponent();

            Customers = new ObservableCollection<Customer>
            {
                new Customer("Nguyễn Văn A", "01", 1000, "Vàng", new DateTime(2023, 1, 10), "0908123456" ),
                new Customer("Trần Thị B", "02", 2000, "Bạc", new DateTime(2023, 2, 15), "0909234567"),
                new Customer("Lê Văn C", "03", 1500, "Đồng", new DateTime(2023, 3, 20), "0907345678"),
                new Customer("Phạm Thị D", "04", 2500, "Kim Cương", new DateTime(2023, 4, 25), "0906456789" ),
                new Customer("Hoàng Văn E", "05", 3000, "Vàng", new DateTime(2023, 5, 5), "0905567890" ),
                new Customer("Đặng Thị F", "06", 1200, "Bạc", new DateTime(2023, 6, 12), "0904678901" ),
                new Customer("Ngô Văn G", "07", 1800, "Đồng", new DateTime(2023, 7, 18), "0903789012"),
                new Customer("Bùi Thị H", "08", 2700, "Kim Cương", new DateTime(2023, 8, 22), "0902890123"),
                new Customer("Vũ Văn I", "09", 3500, "Vàng", new DateTime(2023, 9, 30), "0901901234" ),
                new Customer("Tô Thị J", "10", 4000, "Bạch Kim", new DateTime(2023, 10, 10), "0900012345" ),
                new Customer("Đỗ Văn K", "11", 2200, "Bạc", DateTime.Now.AddDays(-5), "0911111111"),
                new Customer("Lý Thị L", "12", 3100, "Vàng", DateTime.Now.AddDays(-10), "0922222222" ),
                new Customer("Trương Văn M", "13", 5000, "Kim Cương", DateTime.Now.AddDays(-15), "0933333333" ),
            };

            FilteredUsers = new ObservableCollection<Customer>(Customers);
            this.DataContext = this;

            // Cập nhật thông tin thống kê
            UpdateStatistics();
        }

        private void UpdateStatistics()
        {
            // Cập nhật tổng số khách hàng
            TotalCustomers = Customers.Count;

            // Cập nhật số khách hàng mới trong tháng hiện tại
            int currentMonth = DateTime.Now.Month;
            int currentYear = DateTime.Now.Year;
            NewCustomersThisMonth = Customers.Count(c =>
                DateTime.TryParseExact(c.CreatedAt, "dd/MM/yyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out DateTime createdAtDate) &&
                createdAtDate.Month == currentMonth &&
                createdAtDate.Year == currentYear);

            // Cập nhật số khách hàng cao cấp (Vàng, Bạch Kim, Kim Cương)
            PremiumCustomers = Customers.Count(c =>
                c.Rank == "Vàng" || c.Rank == "Bạch Kim" || c.Rank == "Kim Cương");
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyAllFilters();
        }

        private void DateFilter_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            ApplyAllFilters();
        }

        private void RankFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyAllFilters();
        }

        private void StatusFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyAllFilters();
        }

        private void ResetFilter_Click(object sender, RoutedEventArgs e)
        {
            SearchBox.Text = "";
            DateFilter.Date = null;
            RankFilter.SelectedIndex = 0; // Chọn lại "Tất cả"
            StatusFilter.SelectedIndex = 0; // Chọn lại "Tất cả"

            FilteredUsers.Clear();
            foreach (var customer in Customers)
            {
                FilteredUsers.Add(customer);
            }
        }

        private void ApplyAllFilters()
        {
            string searchQuery = SearchBox.Text.ToLower();
            string selectedRank = RankFilter.SelectedItem is ComboBoxItem rankItem ?
                rankItem.Content.ToString() : "Tất cả";
            string selectedStatus = StatusFilter.SelectedItem is ComboBoxItem statusItem ?
                statusItem.Content.ToString() : "Tất cả";

            // Bắt đầu từ toàn bộ danh sách
            var filtered = Customers.AsEnumerable();

            // Lọc theo tìm kiếm
            if (!string.IsNullOrEmpty(searchQuery))
            {
                filtered = filtered.Where(c =>
                    c.Name.ToLower().Contains(searchQuery) ||
                    c.ID.ToLower().Contains(searchQuery) ||
                    c.Phone.ToLower().Contains(searchQuery));
            }

            // Lọc theo ngày
            if (DateFilter.Date.HasValue)
            {
                DateTime selectedDate = DateFilter.Date.Value.DateTime;
                filtered = filtered.Where(c =>
                    DateTime.TryParseExact(c.CreatedAt, "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out DateTime createdAtDate) &&
                    createdAtDate.Date == selectedDate.Date);
            }

            // Lọc theo hạng
            if (selectedRank != "Tất cả")
            {
                filtered = filtered.Where(c => c.Rank == selectedRank);
            }

            // Lọc theo trạng thái
            if (selectedStatus != "Tất cả")
            {
                filtered = filtered.Where(c => c.Status == selectedStatus);
            }

            FilteredUsers.Clear();
            foreach (var customer in filtered)
            {
                FilteredUsers.Add(customer);
            }
        }

        private void UpdateCustomer_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var customer = button.DataContext as Customer;

            // Hiển thị dialog cập nhật thông tin khách hàng
            ShowUpdateCustomerDialog(customer);
        }

        private async void DeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var customer = button.DataContext as Customer;

            // Confirm deletion with the user
            ContentDialog confirmDialog = new ContentDialog
            {
                Title = "Xác nhận xóa khách hàng",
                Content = $"Bạn có chắc chắn muốn xóa khách hàng {customer.Name} (ID: {customer.ID})?",
                PrimaryButtonText = "Xóa",
                CloseButtonText = "Hủy",
                DefaultButton = ContentDialogButton.Close,
                XamlRoot = this.XamlRoot
            };

            var result = await confirmDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                // Remove from both collections
                Customers.Remove(customer);
                if (FilteredUsers.Contains(customer))
                {
                    FilteredUsers.Remove(customer);
                }

                // Update statistics after deletion
                UpdateStatistics();
            }
        }
        private async void ShowUpdateCustomerDialog(Customer customer)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Cập nhật thông tin khách hàng",
                PrimaryButtonText = "Lưu",
                CloseButtonText = "Hủy",
                DefaultButton = ContentDialogButton.Primary,
                XamlRoot = this.XamlRoot
            };

            // Tạo form cập nhật
            var panel = new StackPanel { Spacing = 10 };

            panel.Children.Add(new TextBlock { Text = "Họ và tên:" });
            var nameBox = new TextBox { Text = customer.Name, PlaceholderText = "Nhập họ tên" };
            panel.Children.Add(nameBox);

            panel.Children.Add(new TextBlock { Text = "Số điện thoại:" });
            var phoneBox = new TextBox { Text = customer.Phone, PlaceholderText = "Nhập số điện thoại" };
            panel.Children.Add(phoneBox);

            panel.Children.Add(new TextBlock { Text = "Điểm số:" });
            var pointsBox = new NumberBox { Value = customer.Points, SpinButtonPlacementMode = NumberBoxSpinButtonPlacementMode.Compact };
            panel.Children.Add(pointsBox);

            panel.Children.Add(new TextBlock { Text = "Hạng thẻ:" });
            var rankCombo = new ComboBox();
            rankCombo.Items.Add("Đồng");
            rankCombo.Items.Add("Bạc");
            rankCombo.Items.Add("Vàng");
            rankCombo.Items.Add("Bạch Kim");
            rankCombo.Items.Add("Kim Cương");
            rankCombo.SelectedItem = customer.Rank;
            panel.Children.Add(rankCombo);

            dialog.Content = panel;

            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                // Cập nhật thông tin khách hàng
                customer.Name = nameBox.Text;
                customer.Phone = phoneBox.Text;
                customer.Points = (int)pointsBox.Value;
                customer.Rank = rankCombo.SelectedItem.ToString();

                // Cập nhật lại các thống kê
                UpdateStatistics();

                // Refresh giao diện
                int index = Customers.IndexOf(customer);
                Customers.RemoveAt(index);
                Customers.Insert(index, customer);

                // Áp dụng lại bộ lọc
                ApplyAllFilters();
            }
        }
    }

    public class Customer : INotifyPropertyChanged
    {
        public string ID { get; set; }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private int _points;
        public int Points
        {
            get => _points;
            set
            {
                _points = value;
                OnPropertyChanged();
            }
        }

        private string _rank;
        public string Rank
        {
            get => _rank;
            set
            {
                _rank = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(RankColor));
            }
        }

        public string CreatedAt { get; set; }

        private string _phone;
        public string Phone
        {
            get => _phone;
            set
            {
                _phone = value;
                OnPropertyChanged();
            }
        }

        private string _status;
        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        public SolidColorBrush RankColor => GetRankColor(Rank);

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Customer(string name, string id, int points, string rank, DateTime createdAt, string phone = "")
        {
            Name = name;
            Points = points;
            Rank = rank;
            CreatedAt = createdAt.ToString("dd/MM/yyyy");
            ID = id;
            Phone = phone;
        }

        private SolidColorBrush GetRankColor(string rank) => rank switch
        {
            "Đồng" => new SolidColorBrush(Colors.Brown),
            "Bạc" => new SolidColorBrush(Colors.Silver),
            "Vàng" => new SolidColorBrush(Colors.Gold),
            "Bạch Kim" => new SolidColorBrush(Colors.LightGray),
            "Kim Cương" => new SolidColorBrush(Colors.DeepSkyBlue),
            _ => new SolidColorBrush(Colors.Gray),
        };
    }
}
