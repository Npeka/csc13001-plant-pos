using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using csc13001_plant_pos.DTO.StatisticDTO;
using csc13001_plant_pos.Model;
using csc13001_plant_pos.Service;

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
        public DateTime startDate;

        [ObservableProperty]
        public DateTime endDate;

        [ObservableProperty]
        public StatisticQueryDto statisticQuery;

        private readonly IStatisticService _statisticService;

        public StatisticViewModel(IStatisticService statisticService)
        {
            _statisticService = statisticService;
            LoadDataAsync();
        }

        public async void LoadDataAsync()
        {
            TimeType = "DAILY";
            StartDate = DateTime.Now.AddDays(-1);
            EndDate = DateTime.Now;
            StatisticQuery = new StatisticQueryDto
            {
                TimeType = TimeType,
                StartDate = StartDate,
                EndDate = EndDate
            };
            var response = await _statisticService.GetStatisticAsync(StatisticQuery);
            StatisticDto = response?.Data;
            var response2 = await _statisticService.GetListReviewAsync();
            StatisticReview = response2?.Data;
        }

        //partial void OnStatisticQueryChanged(StatisticQueryDto oldValue)
        //{
        //    // Xử lý khi statisticQuery bị thay đổi
        //}

    }
}