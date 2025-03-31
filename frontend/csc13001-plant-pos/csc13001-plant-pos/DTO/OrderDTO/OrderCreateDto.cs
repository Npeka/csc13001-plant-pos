using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace csc13001_plant_pos.DTO.OrderDTO
{
    public class OrderCreateDto
    {
        [JsonPropertyName("customerPhone")]
        public string CustomerPhone { get; set; }
        [JsonPropertyName("staffId")]
        public int StaffId { get; set; }
        [JsonPropertyName("totalPrice")]
        public decimal TotalPrice { get; set; }
        [JsonPropertyName("discountId")]
        public int? DiscountId { get; set; }
        [JsonPropertyName("items")]
        public List<OrderItemRequest> Items { get; set; }
    }
    public class OrderItemRequest
    {
        [JsonPropertyName("productId")]
        public int ProductId { get; set; }
        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }
    }
}
