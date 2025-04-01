using System.Text.Json.Serialization;

namespace csc13001_plant_pos.DTO;

public class ApiResponse<T>
{
    [JsonPropertyName("status")]
    public required string Status { get; set; }

    [JsonPropertyName("message")]
    public required string Message { get; set; }

    [JsonPropertyName("data")]
    public T? Data { get; set; }

    public bool IsSuccess()
    {
        return Status.Equals("success");
    }

    public bool IsError()
    {
        return Status.Equals("error");
    }
}