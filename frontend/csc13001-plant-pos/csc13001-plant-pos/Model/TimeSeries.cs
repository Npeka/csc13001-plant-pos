using System.Text.Json.Serialization;

namespace csc13001_plant_pos.Model
{
    public class TimeSeries
    {
        [JsonPropertyName("time")]
        public string Time { get; set; }
        [JsonPropertyName("value")]
        public int revenue { get; set; }
    }
}
