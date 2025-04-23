using System.Collections.Generic;
using System.Text.Json.Serialization;
using csc13001_plant_pos.DTO.ProductDTO;
using csc13001_plant_pos.Model;

namespace csc13001_plant_pos.DTO.StatisticDTO
{
    public class StatisticReviewDto
    {
        [JsonPropertyName("topSellingProducts")]
        public List<ProductDto> TopSellingProducts { get; set; }

        [JsonPropertyName("lowStockProducts")]
        public List<Product> LowStockProducts { get; set; }
    }
}