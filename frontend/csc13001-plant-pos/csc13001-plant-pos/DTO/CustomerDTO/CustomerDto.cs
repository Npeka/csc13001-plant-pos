using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using csc13001_plant_pos.Model;

namespace csc13001_plant_pos.DTO.CustomerDTO
{
    class CustomerDto
    {
        [JsonPropertyName("customer")]
        public Customer Customer { get; set; }

        [JsonPropertyName("totalOrders")]
        public int TotalOrders { get; set; }

        [JsonPropertyName("totalSpent")]
        public decimal TotalSpent { get; set; }
    }
}
