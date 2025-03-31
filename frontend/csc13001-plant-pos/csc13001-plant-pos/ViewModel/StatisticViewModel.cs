using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using csc13001_plant_pos.DTO.OrderDTO;
using csc13001_plant_pos.Model;
using csc13001_plant_pos.Service;
using LiveChartsCore.Defaults;

namespace csc13001_plant_pos.ViewModel
{
    public partial class StatisticViewModel : ObservableObject
    {
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

        public class DashboardTile
        {
            public string Title { get; set; }
            public string Value { get; set; }
            public int Percentage { get; set; }
        }
    }
}
