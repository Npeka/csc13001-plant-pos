using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using csc13001_plant_pos.Model;

namespace csc13001_plant_pos.DTO.StatisticDTO
{
    public class StatisticDto
    {
        [JsonPropertyName("revenue")]
        public double Revenue { get; set; }
        [JsonPropertyName("revenueGrowthRate")]
        public double RevenueGrowthRate { get; set; }
        [JsonPropertyName("profit")]
        public double Profit { get; set; }
        [JsonPropertyName("profitGrowthRate")]
        public double ProfitGrowthRate { get; set; }
        [JsonPropertyName("orderCount")]
        public double OrderCount { get; set; }
        [JsonPropertyName("orderCountGrowthRate")]
        public double OrderCountGrowthRate { get; set; }
        [JsonPropertyName("growthRate")]
        public double? GrowthRate { get; set; }
        [JsonPropertyName("timeSeriesRevenues")]
        public List<TimeSeries> TimeSeriesRevenues { get; set; }
    }
}