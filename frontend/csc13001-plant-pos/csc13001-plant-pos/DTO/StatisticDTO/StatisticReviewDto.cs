using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using csc13001_plant_pos.Model;

namespace csc13001_plant_pos.DTO.StatisticDTO
{
    public class StatisticReviewDto
    {
        [JsonPropertyName("topSellingProducts")]
        public List<Product> topSellingProducts { get; set; }

        [JsonPropertyName("lowStockProducts")]
        public List<Product> lowStockProducts { get; set; }
    }
}