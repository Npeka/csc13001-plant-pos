using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SkiaSharp;
using LiveChartsCore.Kernel;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SkiaSharp;
using Windows.Foundation;
using Windows.Foundation.Collections;
using System.ComponentModel;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace csc13001_plant_pos.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>

    public class DashboardTile
    {
        public string Title { get; set; }
        public string Value { get; set; }
        public int Percentage { get; set; }
    }
    public class Product
    {
        public string ProductID { get; set; }
        public string Name { get; set; }
        public int QuantitySold { get; set; }
        public int RemainingStock { get; set; }
    }

    public class DashboardViewModel
    {
        public ObservableCollection<DashboardTile> Tiles { get; set; }

        public DashboardViewModel()
        {
            Tiles = new ObservableCollection<DashboardTile>
            {
                new DashboardTile { Title = "Revenue", Value = "$14,329", Percentage = -10 },
                new DashboardTile { Title = "Orders", Value = "2,506", Percentage = 20 },
                new DashboardTile { Title = "Average", Value = "$2,047", Percentage = -10 }
            };
        }
    }

    public class SalesViewModel
    {


        public SalesViewModel()
        {

        }
    }

    public sealed partial class StatisticPage : Page, INotifyPropertyChanged
    {
        public ObservableCollection<ISeries> Series { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ObservableCollection<Axis> XAxes { get; set; }
        public ObservableCollection<Axis> YAxes { get; set; }
        public ObservableCollection<Product> BestSellingProducts { get; } = new()
    {
        new Product { ProductID = "SP001", Name = "Laptop", QuantitySold = 50 },
        new Product { ProductID = "SP002", Name = "Phone", QuantitySold = 30 },
        new Product { ProductID = "SP003", Name = "Tablet", QuantitySold = 20 }
    };

        public ObservableCollection<Product> LowStockProducts { get; } = new()
    {
        new Product { ProductID = "SP004", Name = "Mouse", RemainingStock = 5 },
        new Product { ProductID = "SP005", Name = "Keyboard", RemainingStock = 3 },
        new Product { ProductID = "SP006", Name = "Monitor", RemainingStock = 2 }
    };
        public DashboardViewModel DashboardVM { get; set; }
        private string _selectedDateRange = "Last 7 days";
        public string SelectedDateRange
        {
            get => _selectedDateRange;
            set
            {
                if (_selectedDateRange != value)
                {
                    _selectedDateRange = value;
                    OnPropertyChanged(nameof(SelectedDateRange));
                }
            }
        }

        public StatisticPage()
        {

            this.InitializeComponent();
            DashboardVM = new DashboardViewModel();
            // Initialize chart data
            Series = new ObservableCollection<ISeries>
        {
            new LineSeries<ObservablePoint>
            {
                Values = GenerateSalesData(),
                GeometrySize = 10,
                Stroke = new SolidColorPaint(SKColors.Gray) { StrokeThickness = 3 },
                Fill = null,
            }
        };

            XAxes = new ObservableCollection<Axis>
        {
            new Axis
            {
                Labels = GenerateTimeLabels(),
                LabelsRotation = 0,
                Name = "Time",
                NamePaint = new SolidColorPaint(SKColors.Black),
                LabelsPaint = new SolidColorPaint(SKColors.Black),
                TextSize = 14
            }
        };

            YAxes = new ObservableCollection<Axis>
        {
            new Axis
            {
                Labeler = value => "$" + value.ToString("N0"),
                Name = "Sales ($)",
                NamePaint = new SolidColorPaint(SKColors.Black),
                LabelsPaint = new SolidColorPaint(SKColors.Black),
                TextSize = 14
            }
        };

            this.DataContext = this;
        }

        private List<ObservablePoint> GenerateSalesData()
        {
            return new List<ObservablePoint>
        {
            new(0, 500), new(1, 750), new(2, 1200), new(3, 1800), new(4, 2300),
            new(5, 2600), new(6, 3200), new(7, 4000), new(8, 4200), new(9, 3800),
            new(10, 3500), new(11, 3100), new(12, 2800), new(13, 6600)
        };
        }
        private List<string> GenerateTimeLabels()
        {
            return new List<string>
        {
            "6AM", "7AM", "8AM", "9AM", "10AM", "11AM", "12PM",
            "1PM", "2PM", "3PM", "4PM", "5PM", "6PM", "7PM"
        };
        }

        private void OnDateRangeSelected(object sender, RoutedEventArgs e)
        {
            if (sender is MenuFlyoutItem item)
            {
                SelectedDateRange = item.Text;
                // Gọi SetBinding để cập nhật UI
                Bindings.Update();
            }
        }

        private async void OnCustomDatePicked(object sender, RoutedEventArgs e)
        {
            var months = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthNames
                         .Where(m => !string.IsNullOrEmpty(m))
                         .ToList();
            var years = Enumerable.Range(1900, DateTime.Now.Year - 1899).Reverse().ToList();

            var monthComboBox = new ComboBox { ItemsSource = months, SelectedIndex = DateTime.Now.Month - 1 };
            var yearComboBox = new ComboBox { ItemsSource = years, SelectedItem = DateTime.Now.Year };

            var stackPanel = new StackPanel();
            stackPanel.Children.Add(new TextBlock { Text = "Select Month:" });
            stackPanel.Children.Add(monthComboBox);
            stackPanel.Children.Add(new TextBlock { Text = "Select Year:" });
            stackPanel.Children.Add(yearComboBox);

            ContentDialog dialog = new ContentDialog
            {
                Title = "Select Month & Year",
                Content = stackPanel,
                PrimaryButtonText = "OK",
                CloseButtonText = "Cancel",
                XamlRoot = this.XamlRoot
            };

            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
            {
                string selectedMonth = monthComboBox.SelectedItem.ToString();
                string selectedYear = yearComboBox.SelectedItem.ToString();
                SelectedDateRange = $"{selectedMonth}, {selectedYear}";
            }
        }


    }

}