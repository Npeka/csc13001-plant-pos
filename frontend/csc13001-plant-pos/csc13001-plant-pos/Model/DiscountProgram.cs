using System;
using System.Text.Json.Serialization;

namespace csc13001_plant_pos.Model
{
    public class DiscountProgram
    {
        [JsonPropertyName("discountId")]
        public int DiscountId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("discountRate")]
        public double DiscountRate { get; set; }

        [JsonPropertyName("startDate")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("endDate")]
        public DateTime EndDate { get; set; }

        [JsonPropertyName("applicableCustomerType")]
        public string ApplicableCustomerType { get; set; }
    }
}
