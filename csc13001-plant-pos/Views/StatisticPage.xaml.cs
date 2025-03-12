using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using ChartAxis = LiveChartsCore.SkiaSharpView.Axis;
using LiveChartsCore.SkiaSharpView.Drawing;
using LiveChartsCore.SkiaSharpView.VisualElements;
using LiveChartsCore.SkiaSharpView.SKCharts;
using LiveChartsCore.Kernel.Sketches;



namespace csc13001_plant_pos.Views
{
    public sealed partial class StatisticPage : Page, INotifyPropertyChanged
    {
        private ObservableCollection<SalesData> _salesDataList;
        public ObservableCollection<SalesData> SalesDataList
        {
            get => _salesDataList;
            set
            {
                _salesDataList = value;
                OnPropertyChanged(nameof(SalesDataList));
            }
        }

        // Thuộc tính biểu đồ doanh số
        public ISeries[] SalesChartSeries { get; set; }
        public ChartAxis[] SalesChartXAxes { get; set; }
        public ChartAxis[] SalesChartYAxes { get; set; }
        public LiveChartsCore.Kernel.Sketches.IChartTooltip<SkiaSharpDrawingContext> SalesChartTooltip { get; set; }

        // Thuộc tính biểu đồ phân bố sản phẩm
        public ISeries[] ProductsChartSeries { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public StatisticPage()
        {
            this.InitializeComponent();
            SetupCharts();
            LoadData();
            this.DataContext = this;
        }

        private void SetupCharts()
        {
            // Thiết lập biểu đồ doanh số
            SalesChartSeries = new ISeries[]
            {
                new LineSeries<double>
                {
                    Values = new double[] { 2300, 1950, 2450, 2800, 2650, 3200, 2900 },
                    Stroke = new SolidColorPaint(SKColors.SeaGreen, 3),
                    GeometryStroke = new SolidColorPaint(SKColors.SeaGreen),
                    GeometryFill = new SolidColorPaint(SKColors.White),
                    GeometrySize = 8,
                    Fill = new SolidColorPaint(SKColor.Parse("#204CAF50"))
                },
                new LineSeries<double>
                {
                    Values = new double[] { 1800, 1250, 1600, 1950, 1700, 2300, 2100 },
                    Stroke = new SolidColorPaint(SKColors.DodgerBlue, 3),
                    GeometryStroke = new SolidColorPaint(SKColors.DodgerBlue),
                    GeometryFill = new SolidColorPaint(SKColors.White),
                    GeometrySize = 8,
                    Fill = new SolidColorPaint(SKColor.Parse("#202196F3"))
                }
            };

            SalesChartXAxes = new ChartAxis[]
            {
                new ChartAxis
                {
                    Labels = new string[] { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" },
                    TextSize = 12
                }
            };

            SalesChartYAxes = new ChartAxis[]
            {
                new ChartAxis
                {
                    TextSize = 12,
                    Labeler = value => "$" + value.ToString("N0")
                }
            };


            // Thiết lập biểu đồ phân bố sản phẩm
            ProductsChartSeries = new ISeries[]
            {
                new PieSeries<double>
                {
                    Values = new double[] { 30 },
                    Name = "Monstera",
                    Fill = new SolidColorPaint(SKColor.Parse("#4CAF50"))
                },
                new PieSeries<double>
                {
                    Values = new double[] { 25 },
                    Name = "Ficus",
                    Fill = new SolidColorPaint(SKColor.Parse("#2196F3"))
                },
                new PieSeries<double>
                {
                    Values = new double[] { 20 },
                    Name = "Pothos",
                    Fill = new SolidColorPaint(SKColor.Parse("#FFC107"))
                },
                new PieSeries<double>
                {
                    Values = new double[] { 15 },
                    Name = "Snake Plant",
                    Fill = new SolidColorPaint(SKColor.Parse("#9C27B0"))
                },
                new PieSeries<double>
                {
                    Values = new double[] { 10 },
                    Name = "Others",
                    Fill = new SolidColorPaint(SKColor.Parse("#FF5722"))
                }
            };
        }

        private void LoadData()
        {
            var random = new Random();

            SalesDataList = new ObservableCollection<SalesData>
            {
                new SalesData
                {
                    Name = "Monstera Deliciosa",
                    Category = "Indoor Plants",
                    Sales = "$2,450.00",
                    Refunds = "$120.00",
                    Net = "$2,330.00",
                    ImagePath = "ms-appx:///Assets/plants/monstera.png",
                    TrendSeries = CreateTrendSeries(random, true)
                },
                new SalesData
                {
                    Name = "Fiddle Leaf Fig",
                    Category = "Indoor Plants",
                    Sales = "$1,890.00",
                    Refunds = "$95.00",
                    Net = "$1,795.00",
                    ImagePath = "ms-appx:///Assets/plants/fiddleleaf.png",
                    TrendSeries = CreateTrendSeries(random, true)
                },
                new SalesData
                {
                    Name = "Snake Plant",
                    Category = "Indoor Plants",
                    Sales = "$1,540.00",
                    Refunds = "$80.00",
                    Net = "$1,460.00",
                    ImagePath = "ms-appx:///Assets/plants/snakeplant.png",
                    TrendSeries = CreateTrendSeries(random, false)
                },
                new SalesData
                {
                    Name = "Pothos",
                    Category = "Indoor Plants",
                    Sales = "$1,320.00",
                    Refunds = "$65.00",
                    Net = "$1,255.00",
                    ImagePath = "ms-appx:///Assets/plants/pothos.png",
                    TrendSeries = CreateTrendSeries(random, true)
                },
                new SalesData
                {
                    Name = "Ceramic Planter",
                    Category = "Accessories",
                    Sales = "$950.00",
                    Refunds = "$120.00",
                    Net = "$830.00",
                    ImagePath = "ms-appx:///Assets/plants/planter.png",
                    TrendSeries = CreateTrendSeries(random, false)
                }
            };
        }

        private ISeries[] CreateTrendSeries(Random random, bool isPositive)
        {
            var values = new double[7];
            double value = random.Next(50, 100);

            for (int i = 0; i < 7; i++)
            {
                if (isPositive)
                {
                    value += random.Next(-10, 20);
                }
                else
                {
                    value += random.Next(-20, 10);
                }
                value = Math.Max(value, 10);
                value = Math.Min(value, 150);
                values[i] = value;
            }

            return new ISeries[]
            {
                new LineSeries<double>
                {
                    Values = values,
                    Stroke = new SolidColorPaint(isPositive ? SKColors.SeaGreen : SKColors.Crimson, 2),
                    Fill = null,
                    GeometrySize = 0,
                    LineSmoothness = 0.5,
                }
            };
        }
    }

    public class SalesData : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string Sales { get; set; }
        public string Refunds { get; set; }
        public string Net { get; set; }
        public string ImagePath { get; set; }
        public ISeries[] TrendSeries { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}