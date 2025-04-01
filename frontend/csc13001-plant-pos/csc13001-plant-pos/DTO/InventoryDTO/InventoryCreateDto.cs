using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace csc13001_plant_pos.DTO.InventoryDTO
{
    public class InventoryCreateDto
    {
        [JsonPropertyName("supplier")]
        public string Supplier { get; set; }

        [JsonPropertyName("totalPrice")]
        public decimal TotalPrice { get; set; }

        [JsonPropertyName("purchaseDate")]
        public DateTime PurchaseDate { get; set; }

        [JsonPropertyName("items")]
        public List<InventoryItemRequest> Items { get; set; }
    }

    public class InventoryItemRequest
    {
        [JsonPropertyName("productId")]
        public int ProductId { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("purchasePrice")]
        public decimal PurchasePrice { get; set; }
    }
}