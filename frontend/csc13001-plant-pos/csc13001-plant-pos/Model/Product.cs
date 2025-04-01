using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace csc13001_plant_pos.Model
{
    public class Product : ObservableObject
    {
        [JsonPropertyName("productId")]
        public int ProductId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonPropertyName("salePrice")]
        public decimal SalePrice { get; set; }

        [JsonPropertyName("purchasePrice")]
        public decimal PurchasePrice { get; set; }

        private int _stock;
        [JsonPropertyName("stock")]
        public int Stock
        {
            get => _stock;
            set => SetProperty(ref _stock, value);
        }

        [JsonPropertyName("size")]
        public int Size { get; set; }

        [JsonPropertyName("careLevel")]
        public int CareLevel { get; set; }

        [JsonPropertyName("lightRequirement")]
        public int LightRequirement { get; set; }

        [JsonPropertyName("wateringSchedule")]
        public int WateringSchedule { get; set; }

        [JsonPropertyName("environmentType")]
        public string EnvironmentType { get; set; }

        [JsonPropertyName("category")]
        public Category Category { get; set; }
    }
}
