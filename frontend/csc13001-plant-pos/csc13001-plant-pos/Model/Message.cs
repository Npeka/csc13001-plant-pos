using System;
using System.Text.Json.Serialization;
using csc13001_plant_pos.Converter.JsonConverter;

namespace csc13001_plant_pos.Model
{
    public class Message
    {
        [JsonPropertyName("messageId")]
        public int MessageId { get; set; }

        [JsonPropertyName("user")]
        public User User { get; set; }

        [JsonPropertyName("message")]
        public string Content { get; set; }

        [JsonPropertyName("fromBot")]
        public bool FromBot { get; set; }

        [JsonPropertyName("sentAt")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime SentAt { get; set; }
        [JsonIgnore]
        public long Sequence { get; set; }
    }
}