using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using csc13001_plant_pos.Model;

namespace csc13001_plant_pos.DTO.InventoryDTO
{
    public class InventoryListDto
    {
        [JsonPropertyName("inventoryId")]
        public int InventoryId { get; set; }
        [JsonPropertyName("supplier")]
        public string Supplier { get; set; }
        [JsonPropertyName("totalPrice")]
        public decimal TotalPrice { get; set; }
        [JsonPropertyName("purchaseDate")]
        public DateTime PurchaseDate { get; set; }
        [JsonPropertyName("items")]
        public List<InventoryItem> InventoryItems { get; set; }
    }
}
