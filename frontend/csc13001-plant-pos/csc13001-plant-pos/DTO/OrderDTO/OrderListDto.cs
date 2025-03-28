using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using csc13001_plant_pos.Model;
using csc13001_plant_pos.Converter.JsonConverter;

namespace csc13001_plant_pos.DTO.OrderDTO
{
    public class OrderListDto
    {
        [JsonPropertyName("orderId")]
        public int OrderId { get; set; }

        [JsonPropertyName("customer")]
        public Customer Customer { get; set; }
        [JsonPropertyName("staff")]
        public StaffUser Staff { get; set; }

        [JsonPropertyName("orderDate")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime OrderDate { get; set; }

        [JsonPropertyName("discountProgram")]
        public DiscountProgram DiscountProgram { get; set; }

        [JsonPropertyName("totalPrice")]
        public decimal TotalPrice { get; set; }

        [JsonPropertyName("finalPrice")]
        public decimal FinalPrice { get; set; }

        [JsonPropertyName("items")]
        public List<OrderItem> OrderItems { get; set; }
    }
}
