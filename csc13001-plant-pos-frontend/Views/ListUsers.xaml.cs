using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace csc13001_plant_pos_frontend.Views;

public sealed partial class ListUsers : Page
{
    public ObservableCollection<Customer> Customers
    {
        get; set;
    }
    public ObservableCollection<Customer> FilteredUsers
    {
        get; set;
    }

    public ListUsers()
    {
        this.InitializeComponent();
        Customers = new ObservableCollection<Customer>
        {
            new Customer("Nguyễn Văn A", "01", 1000, "Vàng", new DateTime(2023, 1, 10)),
            new Customer("Trần Thị B", "02", 2000, "Bạc", new DateTime(2023, 2, 15)),
            new Customer("Lê Văn C", "03", 1500, "Đồng", new DateTime(2023, 3, 20)),
            new Customer("Phạm Thị D", "04", 2500, "Kim Cương", new DateTime(2023, 4, 25)),
            new Customer("Hoàng Văn E", "05", 3000, "Vàng", new DateTime(2023, 5, 5)),
            new Customer("Đặng Thị F", "06", 1200, "Bạc", new DateTime(2023, 6, 12)),
            new Customer("Ngô Văn G", "07", 1800, "Đồng", new DateTime(2023, 7, 18)),
            new Customer("Bùi Thị H", "08", 2700, "Kim Cương", new DateTime(2023, 8, 22)),
            new Customer("Vũ Văn I", "09", 3500, "Vàng", new DateTime(2023, 9, 30)),
            new Customer("Tô Thị J", "10", 4000, "Bạch Kim", new DateTime(2023, 10, 10))
        };
        FilteredUsers = new ObservableCollection<Customer>(Customers);
        this.DataContext = this;
    }

    private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        string query = SearchBox.Text.ToLower();
        FilteredUsers.Clear();
        foreach (var customer in Customers.Where(c => c.Name.ToLower().Contains(query)))
        {
            FilteredUsers.Add(customer);
        }
    }

    private void DateFilter_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
    {
        if (sender.Date.HasValue)
        {
            DateTime selectedDate = sender.Date.Value.DateTime;
            FilteredUsers.Clear();

            foreach (var customer in Customers)
            {
                if (DateTime.TryParseExact(customer.CreatedAt, "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out DateTime createdAtDate))
                {
                    if (createdAtDate.Date == selectedDate.Date)
                    {
                        FilteredUsers.Add(customer);
                    }
                }
            }
        }
    }
    private void RankFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (RankFilter.SelectedItem is ComboBoxItem selectedItem)
        {
            string selectedRank = selectedItem.Content.ToString();
            ApplyFilters(selectedRank);
        }
    }

    private void ResetFilter_Click(object sender, RoutedEventArgs e)
    {
        SearchBox.Text = "";
        DateFilter.Date = null;
        RankFilter.SelectedIndex = 0; // Chọn lại "Tất cả"

        FilteredUsers.Clear();
        foreach (var customer in Customers)
        {
            FilteredUsers.Add(customer);
        }
    }

    private void ApplyFilters(string selectedRank)
    {
        FilteredUsers.Clear();
        var filtered = Customers.AsEnumerable();

        // Lọc theo ngày
        if (DateFilter.Date.HasValue)
        {
            DateTime selectedDate = DateFilter.Date.Value.DateTime;
            filtered = filtered.Where(c => DateTime.TryParseExact(c.CreatedAt, "dd/MM/yyyy",
                System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime createdAtDate) &&
                createdAtDate.Date == selectedDate.Date);
        }

        // Lọc theo hạng
        if (!string.IsNullOrEmpty(selectedRank) && selectedRank != "Tất cả")
        {
            filtered = filtered.Where(c => c.Rank == selectedRank);
        }

        foreach (var customer in filtered)
        {
            FilteredUsers.Add(customer);
        }
    }


}

public class Customer
{
    public string ID
    {
        get; set;
    }
    public string Name
    {
        get; set;
    }
    public int Points
    {
        get; set;
    }
    public string Rank
    {
        get; set;
    }
    public string CreatedAt
    {
        get; set;
    }
    public SolidColorBrush RankColor => GetRankColor(Rank);

    public Customer(string name, string id, int points, string rank, DateTime createdAt)
    {
        Name = name;
        Points = points;
        Rank = rank;
        CreatedAt = createdAt.ToString("dd/MM/yyyy");
        ID = id;
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