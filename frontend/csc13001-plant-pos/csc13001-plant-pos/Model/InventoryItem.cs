using System.Text.Json.Serialization;

namespace csc13001_plant_pos.Model
{
    public class InventoryItem
    {
        [JsonPropertyName("inventoryItemId")]
        public int InventoryItemId { get; set; }
        [JsonPropertyName("product")]
        public Product Product { get; set; }
        [JsonPropertyName("purchasePrice")]
        public decimal PurchasePrice { get; set; }
        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }
        [JsonPropertyName("remainingQuantity")]
        public int RemainingQuantity { get; set; }
        public decimal TotalItemPrice => Quantity * PurchasePrice;
    }
}
