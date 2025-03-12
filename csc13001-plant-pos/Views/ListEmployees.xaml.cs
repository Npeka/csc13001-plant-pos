using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace csc13001_plant_pos.Views
{
    public sealed partial class ListEmployees : Page
    {
        public ObservableCollection<Employee> Employees { get; set; }
        public ObservableCollection<Employee> FilteredEmployees { get; set; }

        public ListEmployees()
        {
            this.InitializeComponent();
            Employees = new ObservableCollection<Employee>
            {
                new Employee("Nguyễn Văn A", "E001", "0987654321", new DateTime(2021, 1, 10), "Quản lý"),
                new Employee("Trần Thị B", "E002", "0978654321", new DateTime(2022, 2, 15), "Nhân viên bán hàng"),
                new Employee("Lê Văn C", "E003", "0967543210", new DateTime(2023, 3, 20), "Nhân viên kho"),
                new Employee("Phạm Thị D", "E004", "0956432109", new DateTime(2020, 4, 25), "Kế toán"),
                new Employee("Hoàng Văn E", "E005", "0945321098", new DateTime(2019, 5, 5), "Nhân viên bán hàng"),
                new Employee("Đặng Thị F", "E006", "0934210987", new DateTime(2018, 6, 12), "Quản lý"),
                new Employee("Ngô Văn G", "E007", "0923109876", new DateTime(2021, 7, 18), "Nhân viên kho"),
                new Employee("Bùi Thị H", "E008", "0912098765", new DateTime(2023, 8, 22), "Kế toán"),
                new Employee("Vũ Văn I", "E009", "0901098765", new DateTime(2022, 9, 30), "Nhân viên bán hàng"),
                new Employee("Tô Thị J", "E010", "0988198765", new DateTime(2017, 10, 10), "Quản lý")
            };
            FilteredEmployees = new ObservableCollection<Employee>(Employees);
            this.DataContext = this;

            // Cập nhật số lượng nhân viên hiển thị
            UpdateEmployeeCount();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string query = SearchBox.Text.ToLower();
            FilteredEmployees.Clear();
            foreach (var employee in Employees.Where(emp => emp.Name.ToLower().Contains(query) || emp.ID.ToLower().Contains(query)))
            {
                FilteredEmployees.Add(employee);
            }
            UpdateEmployeeCount();
        }

        private void DateFilter_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            ApplyFilters();
        }

        private void PositionFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void ResetFilter_Click(object sender, RoutedEventArgs e)
        {
            SearchBox.Text = "";
            DateFilter.Date = null;
            PositionFilter.SelectedIndex = 0; // Chọn lại "Tất cả"

            FilteredEmployees.Clear();
            foreach (var employee in Employees)
            {
                FilteredEmployees.Add(employee);
            }
            UpdateEmployeeCount();
        }

        private void ApplyFilters()
        {
            string searchQuery = SearchBox.Text.ToLower();
            string selectedPosition = (PositionFilter.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Tất cả";
            DateTime? selectedDate = DateFilter.Date?.DateTime;

            FilteredEmployees.Clear();
            var filtered = Employees.AsEnumerable();

            // Lọc theo tìm kiếm
            if (!string.IsNullOrEmpty(searchQuery))
            {
                filtered = filtered.Where(emp =>
                    emp.Name.ToLower().Contains(searchQuery) ||
                    emp.ID.ToLower().Contains(searchQuery));
            }

            // Lọc theo ngày bắt đầu làm việc
            if (selectedDate.HasValue)
            {
                filtered = filtered.Where(emp => emp.StartDate.Date == selectedDate.Value.Date);
            }

            // Lọc theo vị trí
            if (!string.IsNullOrEmpty(selectedPosition) && selectedPosition != "Tất cả")
            {
                filtered = filtered.Where(emp => emp.Position == selectedPosition);
            }

            foreach (var employee in filtered)
            {
                FilteredEmployees.Add(employee);
            }

            UpdateEmployeeCount();
        }

        private void UpdateEmployeeCount()
        {
            EmployeeCount.Text = $"{FilteredEmployees.Count} nhân viên";
        }

        // Xử lý nút chỉnh sửa nhân viên
        private void EditEmployee_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Employee employee = button.Tag as Employee;

            // Xử lý logic chỉnh sửa nhân viên
            // Frame.Navigate(typeof(EditEmployeePage), employee);

            // Hiển thị thông báo tạm thời để kiểm tra
            ContentDialog dialog = new ContentDialog()
            {
                Title = "Thông báo",
                Content = $"Đang chỉnh sửa nhân viên: {employee.Name}",
                CloseButtonText = "Đóng",
                XamlRoot = this.XamlRoot
            };

            _ = dialog.ShowAsync();
        }

        // Xử lý nút xóa nhân viên
        private void DeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Employee employee = button.Tag as Employee;

            // Hiển thị dialog xác nhận xóa
            ShowDeleteConfirmationDialog(employee);
        }

        private async void ShowDeleteConfirmationDialog(Employee employee)
        {
            ContentDialog deleteDialog = new ContentDialog()
            {
                Title = "Xác nhận xóa",
                Content = $"Bạn có chắc chắn muốn xóa nhân viên '{employee.Name}' không?",
                PrimaryButtonText = "Xóa",
                CloseButtonText = "Hủy",
                DefaultButton = ContentDialogButton.Close,
                XamlRoot = this.XamlRoot
            };

            ContentDialogResult result = await deleteDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                // Xóa nhân viên
                Employees.Remove(employee);
                if (FilteredEmployees.Contains(employee))
                {
                    FilteredEmployees.Remove(employee);
                }
                UpdateEmployeeCount();
            }
        }
    }

    public class Employee
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime StartDate { get; set; }
        public string Position { get; set; }

        public Employee(string name, string id, string phoneNumber, DateTime startDate, string position)
        {
            Name = name;
            ID = id;
            PhoneNumber = phoneNumber;
            StartDate = startDate;
            Position = position;
        }
    }
}