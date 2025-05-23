﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;
using csc13001_plant_pos.Converter.JsonConverter;
using csc13001_plant_pos.Model;

namespace csc13001_plant_pos.DTO.NotificationDTO;

public class NotificationDto : ObservableObject
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

    [JsonPropertyName("users")]
    public List<User> Users { get; set; }
}