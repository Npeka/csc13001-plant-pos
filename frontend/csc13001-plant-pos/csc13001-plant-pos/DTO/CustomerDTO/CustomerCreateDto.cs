using System.Text.Json.Serialization;

namespace csc13001_plant_pos.DTO.CustomerDTO
{
    public class CustomerCreateDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("phone")]
        public string Phone { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("address")]
        public string Address { get; set; }
        [JsonPropertyName("gender")]
        public string Gender { get; set; }
        [JsonPropertyName("loyaltyCardType")]
        public string LoyaltyCardType { get; set; }
    }
}