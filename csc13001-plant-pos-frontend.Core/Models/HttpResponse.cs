using System.Text.Json.Serialization;

namespace csc13001_plant_pos_frontend.Core.Models;

public class HttpResponse<T>
{
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("data")]
    public T? Data
    {
        get; set;
    }
}
