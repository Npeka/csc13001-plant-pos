using System.Text.Json.Serialization;

namespace csc13001_plant_pos.Model
{
    public class Customer
    {
        [JsonPropertyName("customerId")]
        public int CustomerId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("gender")]
        public string Gender { get; set; }

        [JsonPropertyName("loyaltyPoints")]
        public int LoyaltyPoints { get; set; }

        [JsonPropertyName("loyaltyCardType")]
        public string LoyaltyCardType { get; set; }
    }
}
