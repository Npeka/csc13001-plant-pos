using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace csc13001_plant_pos_frontend.Utils;

public static class JsonUtils
{
    private static readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static string Serialize(object data)
    {
        return JsonSerializer.Serialize(data, _options);
    }

    public static T? Deserialize<T>(string json)
    {
        return JsonSerializer.Deserialize<T>(json, _options);
    }

    public static StringContent ToJsonContent(object data)
    {
        return new StringContent(Serialize(data), Encoding.UTF8, "application/json");
    }
}
