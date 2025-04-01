using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using csc13001_plant_pos.Model;

namespace csc13001_plant_pos.DTO.ProductDTO
{
    public class ProductDto
    {
        [JsonPropertyName("product")]
        public Product Product { get; set; }

        [JsonPropertyName("salesQuantity")]
        public int SalesQuantity { get; set; }

        [JsonPropertyName("revenue")]
        public int Revenue { get; set; }
    }
}