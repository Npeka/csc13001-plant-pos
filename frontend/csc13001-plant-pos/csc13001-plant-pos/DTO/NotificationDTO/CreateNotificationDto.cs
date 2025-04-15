using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace csc13001_plant_pos.DTO.NotificationDTO
{
    public class CreateNotificationDto
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("to")]
        public List<int> To { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}