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
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string query = SearchBox.Text.ToLower();
            FilteredEmployees.Clear();
            foreach (var employee in Employees.Where(emp => emp.Name.ToLower().Contains(query) || emp.ID.ToLower().Contains(query)))
            {
                FilteredEmployees.Add(employee);
            }
        }

        private void DateFilter_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            if (sender.Date.HasValue)
            {
                DateTime selectedDate = sender.Date.Value.DateTime;
                FilteredEmployees.Clear();

                foreach (var employee in Employees)
                {
                    if (employee.StartDate.Date == selectedDate.Date)
                    {
                        FilteredEmployees.Add(employee);
                    }
                }
            }
        }

        private void PositionFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PositionFilter.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedPosition = selectedItem.Content.ToString();
                ApplyFilters(selectedPosition);
            }
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
        }

        private void ApplyFilters(string selectedPosition)
        {
            FilteredEmployees.Clear();
            var filtered = Employees.AsEnumerable();

            // Lọc theo ngày bắt đầu làm việc
            if (DateFilter.Date.HasValue)
            {
                DateTime selectedDate = DateFilter.Date.Value.DateTime;
                filtered = filtered.Where(emp => emp.StartDate.Date == selectedDate.Date);
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
