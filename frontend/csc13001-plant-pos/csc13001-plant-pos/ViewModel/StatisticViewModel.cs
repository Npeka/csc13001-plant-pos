using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using csc13001_plant_pos.DTO.StatisticDTO;
using csc13001_plant_pos.Model;
using csc13001_plant_pos.Service;
using LiveChartsCore;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using Microsoft.UI.Xaml;
using OfficeOpenXml;
using Windows.Storage.Pickers;
using WinRT.Interop;
using System.IO;
using csc13001_plant_pos.DTO.ProductDTO;

namespace csc13001_plant_pos.ViewModel
{
    public partial class StatisticViewModel : ObservableObject
    {
        [ObservableProperty]
        private StatisticReviewDto statisticReview;

        [ObservableProperty]
        private StatisticDto statisticDto;

        [ObservableProperty]
        private ObservableCollection<Product> products;

        [ObservableProperty]
        private List<ProductDto> topSellingProducts;

        [ObservableProperty]
        private List<Product> lowStockProducts;

        [ObservableProperty]
        public string timeType;

        [ObservableProperty]
        public DateTimeOffset startDate;

        [ObservableProperty]
        public DateTimeOffset endDate;

        public IEnumerable<ICartesianAxis> XAxes { get; set; }
        public IEnumerable<ICartesianAxis> YAxes { get; set; }
        public ISeries[] MySeries { get; set; }

        [ObservableProperty]
        public StatisticQueryDto statisticQuery;

        private readonly IStatisticService _statisticService;

        public StatisticViewModel(IStatisticService statisticService)
        {
            _statisticService = statisticService;
            LoadDataAsync();
        }

        public void LoadDataAsync()
        {
            TimeType = "DAILY";
            StartDate = DateTime.Now.AddDays(-1);
            EndDate = DateTime.Now;
            reloadData();
        }

        public async void reloadData()
        {
            StatisticQuery = new StatisticQueryDto
            {
                TimeType = TimeType,
                StartDate = StartDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                EndDate = EndDate.ToString("yyyy-MM-ddTHH:mm:ss")
            };

            // Gọi cả hai service song song
            var task1 = _statisticService.GetStatisticAsync(StatisticQuery);
            var task2 = _statisticService.GetListReviewAsync();

            // Chờ cả hai task hoàn thành
            await Task.WhenAll(task1, task2);

            // Đảm bảo không có lỗi xảy ra trong cả hai response
            var response = await task1;
            var response2 = await task2;

            // Xử lý response1
            StatisticDto = response?.Data;
            if (StatisticDto != null && StatisticDto.TimeSeriesRevenues != null)
            {
                UpdateAxes();
            }

            // Xử lý response2
            StatisticReview = response2?.Data;
            if (StatisticReview != null)
            {
                TopSellingProducts = StatisticReview.TopSellingProducts?.Take(6).ToList() ?? new List<ProductDto>();
                LowStockProducts = StatisticReview.LowStockProducts?.Take(6).ToList() ?? new List<Product>();
            }

            OnPropertyChanged(nameof(StatisticReview));
            OnPropertyChanged(nameof(TopSellingProducts));
            OnPropertyChanged(nameof(LowStockProducts));
        }


        public void UpdateAxes()
        {
            List<string> labels = new();
            List<long> revenueValues = new();
            var start = startDate.DateTime;
            var end = endDate.DateTime;

            switch (TimeType)
            {
                case "DAILY":
                    for (int hour = 0; hour < 24; hour++)
                    {
                        labels.Add($"{hour:00}h");
                        revenueValues.Add(GetRevenueForHour(hour));
                    }
                    break;

                case "MONTHLY":
                    for (var dt = start.Date; dt <= end.Date; dt = dt.AddDays(1))
                    {
                        labels.Add(dt.ToString("dd/MM"));
                        revenueValues.Add(GetRevenueForDate(dt));
                    }
                    break;

                case "YEARLY":
                    DateTime currentMonth = new DateTime(start.Year, start.Month, 1);
                    DateTime endMonth = new DateTime(end.Year, end.Month, 1);
                    while (currentMonth <= endMonth)
                    {
                        labels.Add(currentMonth.ToString("MM/yy"));
                        revenueValues.Add(GetRevenueForMonth(currentMonth));
                        currentMonth = currentMonth.AddMonths(1);
                    }
                    break;
            }

            XAxes = new ICartesianAxis[]
            {
        new Axis
        {
            Labels = labels.ToArray(),
            LabelsRotation = 15,
            Name = "Thời gian",
            MinStep = 1,
            ForceStepToMin = true,
        },
            };

            YAxes = new ICartesianAxis[]
            {
        new Axis
        {
            Name = "Giá trị",
            LabelsRotation = 0,
            MinLimit = 0
        }
            };

            // Cập nhật MySeries với dữ liệu doanh thu
            MySeries = new ISeries[]
            {
        new LineSeries<long>
        {
            Values = revenueValues.ToArray(),
            Name = "Doanh thu"
        }
            };

            OnPropertyChanged(nameof(MySeries));
            OnPropertyChanged(nameof(XAxes));
            OnPropertyChanged(nameof(YAxes));
        }

        private long GetRevenueForHour(int hour)
        {
            return StatisticDto?.TimeSeriesRevenues
                .Where(x =>
                {
                    if (DateTime.TryParseExact(x.Time, "yyyy-MM-dd HH:mm", null, System.Globalization.DateTimeStyles.None, out var dt))
                    {
                        return dt.Hour == hour;
                    }
                    return false;
                })
                .Sum(x => (long)x.Revenue) ?? 0;
        }


        private long GetRevenueForDate(DateTime date)
        {
            return StatisticDto?.TimeSeriesRevenues
                .Where(x => DateTime.ParseExact(x.Time, "yyyy-MM-dd", null) == date.Date)
                .Sum(x => (long)x.Revenue) ?? 0;
        }

        private long GetRevenueForMonth(DateTime month)
        {
            return StatisticDto?.TimeSeriesRevenues
                .Where(x => DateTime.ParseExact(x.Time, "yyyy-MM", null).Month == month.Month
                         && DateTime.ParseExact(x.Time, "yyyy-MM", null).Year == month.Year)
                .Sum(x => (long)x.Revenue) ?? 0;
        }

        public async Task ExportToExcelAsync(Window window)
        {
            ExcelPackage.License.SetNonCommercialOrganization("My Noncommercial organization");

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Report");

            // Thêm thông tin về timeType, startDate, và endDate vào báo cáo
            worksheet.Cells[1, 1].Value = "Time Type";
            worksheet.Cells[1, 2].Value = "Start Date";
            worksheet.Cells[1, 3].Value = "End Date";

            worksheet.Cells[2, 1].Value = timeType; // Giá trị timeType
            worksheet.Cells[2, 2].Value = startDate.ToString("yyyy-MM-dd HH:mm:ss"); // Format startDate
            worksheet.Cells[2, 3].Value = endDate.ToString("yyyy-MM-dd HH:mm:ss"); // Format endDate

            // Thêm headers cho StatisticDto
            worksheet.Cells[4, 1].Value = "Revenue";
            worksheet.Cells[4, 2].Value = "Revenue Growth Rate";
            worksheet.Cells[4, 3].Value = "Profit";
            worksheet.Cells[4, 4].Value = "Profit Growth Rate";
            worksheet.Cells[4, 5].Value = "Order Count";
            worksheet.Cells[4, 6].Value = "Order Count Growth Rate";
            worksheet.Cells[4, 7].Value = "Growth Rate";

            // Fill StatisticDto data
            worksheet.Cells[5, 1].Value = StatisticDto.Revenue;
            worksheet.Cells[5, 2].Value = StatisticDto.RevenueGrowthRate;
            worksheet.Cells[5, 3].Value = StatisticDto.Profit;
            worksheet.Cells[5, 4].Value = StatisticDto.ProfitGrowthRate;
            worksheet.Cells[5, 5].Value = StatisticDto.OrderCount;
            worksheet.Cells[5, 6].Value = StatisticDto.OrderCountGrowthRate;
            worksheet.Cells[5, 7].Value = StatisticDto.GrowthRate ?? 0;

            int row = 7;

            // Thêm headers cho Product data
            worksheet.Cells[row, 1].Value = "Product ID";
            worksheet.Cells[row, 2].Value = "Name";
            worksheet.Cells[row, 3].Value = "Description";
            worksheet.Cells[row, 4].Value = "Image URL";
            worksheet.Cells[row, 5].Value = "Sale Price";
            worksheet.Cells[row, 6].Value = "Purchase Price";
            worksheet.Cells[row, 7].Value = "Stock";
            worksheet.Cells[row, 8].Value = "Size";
            worksheet.Cells[row, 9].Value = "Care Level";
            worksheet.Cells[row, 10].Value = "Light Requirement";
            worksheet.Cells[row, 11].Value = "Watering Schedule";
            worksheet.Cells[row, 12].Value = "Environment Type";
            worksheet.Cells[row, 13].Value = "Category";

            row++; // Chuyển xuống dòng sau khi thêm tiêu đề cho sản phẩm

            // Fill Product data for TopSellingProducts
            if (StatisticReview?.TopSellingProducts != null)
            {
                worksheet.Cells[row, 1].Value = "Top Selling Products";
                worksheet.Cells[row, 1, row, 13].Merge = true; // Gộp dòng tiêu đề cho nhóm
                row++; // Chuyển xuống dòng tiếp theo

                foreach (var product in StatisticReview.TopSellingProducts)
                {
                    worksheet.Cells[row, 1].Value = product.Product.ProductId;
                    worksheet.Cells[row, 2].Value = product.Product.Name;
                    worksheet.Cells[row, 3].Value = product.Product.Description;
                    worksheet.Cells[row, 4].Value = product.Product.ImageUrl;
                    worksheet.Cells[row, 5].Value = product.Product.SalePrice;
                    worksheet.Cells[row, 6].Value = product.Product.PurchasePrice;
                    worksheet.Cells[row, 7].Value = product.Product.Stock;
                    worksheet.Cells[row, 8].Value = product.Product.Size;
                    worksheet.Cells[row, 9].Value = product.Product.CareLevel;
                    worksheet.Cells[row, 10].Value = product.Product.LightRequirement;
                    worksheet.Cells[row, 11].Value = product.Product.WateringSchedule;
                    worksheet.Cells[row, 12].Value = product.Product.EnvironmentType;
                    worksheet.Cells[row, 13].Value = product.Product.Category?.Name;
                    row++;
                }
            }

            // Tạo một khoảng cách giữa các nhóm
            row++;

            // Fill Product data for LowStockProducts
            if (StatisticReview?.LowStockProducts != null)
            {
                worksheet.Cells[row, 1].Value = "Low Stock Products";
                worksheet.Cells[row, 1, row, 13].Merge = true; // Gộp dòng tiêu đề cho nhóm
                row++; // Chuyển xuống dòng tiếp theo

                foreach (var product in StatisticReview.LowStockProducts)
                {
                    worksheet.Cells[row, 1].Value = product.ProductId;
                    worksheet.Cells[row, 2].Value = product.Name;
                    worksheet.Cells[row, 3].Value = product.Description;
                    worksheet.Cells[row, 4].Value = product.ImageUrl;
                    worksheet.Cells[row, 5].Value = product.SalePrice;
                    worksheet.Cells[row, 6].Value = product.PurchasePrice;
                    worksheet.Cells[row, 7].Value = product.Stock;
                    worksheet.Cells[row, 8].Value = product.Size;
                    worksheet.Cells[row, 9].Value = product.CareLevel;
                    worksheet.Cells[row, 10].Value = product.LightRequirement;
                    worksheet.Cells[row, 11].Value = product.WateringSchedule;
                    worksheet.Cells[row, 12].Value = product.EnvironmentType;
                    worksheet.Cells[row, 13].Value = product.Category?.Name;
                    row++;
                }
            }

            // Format auto width
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

            // Open file save picker
            var picker = new FileSavePicker();
            InitializeWithWindow.Initialize(picker, WindowNative.GetWindowHandle(window));
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            picker.SuggestedFileName = "exportReport";
            picker.FileTypeChoices.Add("Excel File", new List<string> { ".xlsx" });

            var file = await picker.PickSaveFileAsync();
            if (file != null)
            {
                using var stream = await file.OpenStreamForWriteAsync();
                await package.SaveAsAsync(stream);
            }
        }

        partial void OnTimeTypeChanged(string value)
        {
            switch (value)
            {
                case "DAILY":
                    EndDate = StartDate.AddDays(1);
                    break;
                case "WEEKLY":
                    EndDate = StartDate.AddDays(7);
                    break;
                case "MONTHLY":
                    EndDate = StartDate.AddMonths(1);
                    break;
                case "YEARLY":
                    EndDate = StartDate.AddYears(1);
                    break;
            }
            reloadData();
        }

        partial void OnStartDateChanged(DateTimeOffset value)
        {
            switch (TimeType)
            {
                case "DAILY":
                    EndDate = StartDate.AddDays(1);
                    break;
                case "WEEKLY":
                    EndDate = StartDate.AddDays(7);
                    break;
                case "MONTHLY":
                    EndDate = StartDate.AddMonths(1);
                    break;
                case "YEARLY":
                    EndDate = StartDate.AddYears(1);
                    break;
            }
            reloadData();
        }
    }
}