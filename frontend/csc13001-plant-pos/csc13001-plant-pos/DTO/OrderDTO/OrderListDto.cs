using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using csc13001_plant_pos.Model;

namespace csc13001_plant_pos.DTO.OrderDTO
{
    public class OrderListDto
    {
        [JsonPropertyName("orderId")]
        public int OrderId { get; set; }

        [JsonPropertyName("customer")]
        public Customer Customer { get; set; }
        //[JsonPropertyName("staff")]
        //public StaffUser Staff { get; set; }

        [JsonPropertyName("orderDate")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime OrderDate { get; set; }

        [JsonPropertyName("discountProgram")]
        public DiscountProgram DiscountProgram { get; set; }

        [JsonPropertyName("totalPrice")]
        public decimal TotalPrice { get; set; }

        [JsonPropertyName("finalPrice")]
        public decimal FinalPrice { get; set; }

        [JsonPropertyName("orderItems")]
        public List<OrderItem> OrderItems { get; set; }

        [JsonPropertyName("customerId")]
        public int CustomerId { get; set; }
    }
    public class CustomDateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.Parse(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}
