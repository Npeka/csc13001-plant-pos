using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using csc13001_plant_pos.Model;

namespace csc13001_plant_pos.DTO.StatisticDTO
{
    public class StatisticDto
    {
        [JsonPropertyName("revenue")]
        public int Revenue { get; set; }
        [JsonPropertyName("revenueGrowthRate")]
        public int RevenueGrowthRate { get; set; }
        [JsonPropertyName("profit")]
        public int Profit { get; set; }
        [JsonPropertyName("profitGrowthRate")]
        public int ProfitGrowthRate { get; set; }
        [JsonPropertyName("orderCount")]
        public int OrderCount { get; set; }
        [JsonPropertyName("orderCountGrowthRate")]
        public int OrderCountGrowthRate { get; set; }
        [JsonPropertyName("growthRate")]
        public int? GrowthRate { get; set; }
        [JsonPropertyName("timeSeriesRevenues")]
        public List<TimeSeries> TimeSeriesRevenues { get; set; }
    }
}