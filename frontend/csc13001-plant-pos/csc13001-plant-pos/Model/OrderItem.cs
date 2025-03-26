using System.Text.Json.Serialization;

namespace csc13001_plant_pos.Model
{
    public class OrderItem
    {
        [JsonPropertyName("orderItemId")]
        public int OrderItemId { get; set; }

        [JsonPropertyName("product")]
        public Product Product { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("salePrice")]
        public decimal SalePrice { get; set; }

        [JsonPropertyName("purchasePrice")]
        public decimal PurchasePrice { get; set; }

        [JsonPropertyName("discountProgram")]
        public DiscountProgram? DiscountProgram { get; set; }
        public decimal TotalItemPrice => Quantity * SalePrice;
    }
}
