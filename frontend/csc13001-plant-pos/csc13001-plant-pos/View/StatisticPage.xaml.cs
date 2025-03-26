using System.Collections.ObjectModel;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.Painting.Effects;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SkiaSharp;
using LiveChartsCore.Kernel;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using LiveChartsCore.Measure;
using System.Collections.Generic;

namespace csc13001_plant_pos.View
{
    public class SalesData
    {
        public string Name { get; set; }
        public string Sales { get; set; }
        public string Refunds { get; set; }
        public string Net { get; set; }
    }

    public class DashboardTile
    {
        public string Title { get; set; }
        public string Value { get; set; }
        public int Percentage { get; set; }
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

    public sealed partial class StatisticPage : Page
    {
        public ObservableCollection<ISeries> Series { get; set; }
        public ObservableCollection<Axis> XAxes { get; set; }
        public ObservableCollection<Axis> YAxes { get; set; }
        public ObservableCollection<SalesData> SalesList { get; set; }

        public StatisticPage()
        {
            SalesList = new ObservableCollection<SalesData>
            {
                new SalesData { Name = "Gross Sales", Sales = "$10,904.00", Refunds = "$0.00", Net = "$10,904.00" },
                new SalesData { Name = "Net Sales", Sales = "$10,904.00", Refunds = "$0.00", Net = "$10,904.00" },
                new SalesData { Name = "Total Collected", Sales = "$10,904.00", Refunds = "$0.00", Net = "$10,904.00" }
            };
            this.InitializeComponent();
            // Initialize chart data
            Series = new ObservableCollection<ISeries>
        {
            new LineSeries<ObservablePoint>
            {
                Values = GenerateSalesData(),
                GeometrySize = 10,
                Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 3 },
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
                NamePaint = new SolidColorPaint(SKColors.White),
                LabelsPaint = new SolidColorPaint(SKColors.White),
                TextSize = 14
            }
        };

            YAxes = new ObservableCollection<Axis>
        {
            new Axis
            {
                Labeler = value => "$" + value.ToString("N0"),
                Name = "Sales ($)",
                NamePaint = new SolidColorPaint(SKColors.White),
                LabelsPaint = new SolidColorPaint(SKColors.White),
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
    }

}