using System.Text.Json.Serialization;

namespace csc13001_plant_pos.DTO.Message
{
    public class CreateMessageDto
    {
        [JsonPropertyName("userId")]
        public int UserId { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
