using System;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;
using csc13001_plant_pos.Converter.JsonConverter;

namespace csc13001_plant_pos.Model;

public class Notification : ObservableObject
{
    [JsonPropertyName("notificationUserId")]
    public int NotificationUserId { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("content")]
    public string Content { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("typeName")]
    public string TypeName { get; set; }

    [JsonPropertyName("createdAt")]
    [JsonConverter(typeof(CustomDateTimeConverter))]
    public DateTime CreatedAt { get; set; }

    private bool _isRead;
    [JsonPropertyName("isRead")]
    public bool IsRead
    {
        get => _isRead;
        set => SetProperty(ref _isRead, value);
    }
}
