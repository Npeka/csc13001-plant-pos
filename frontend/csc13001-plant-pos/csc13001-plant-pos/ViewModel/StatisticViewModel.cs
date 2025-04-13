using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using csc13001_plant_pos.DTO.StatisticDTO;
using csc13001_plant_pos.Model;
using csc13001_plant_pos.Service;
using LiveChartsCore;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;


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

            var response = await _statisticService.GetStatisticAsync(StatisticQuery);
            StatisticDto = response?.Data;

            if (StatisticDto != null)
            {
                if (StatisticDto.TimeSeriesRevenues != null)
                {
                    UpdateAxes();
                }
            }

            var response2 = await _statisticService.GetListReviewAsync();
            StatisticReview = response2?.Data;
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