using System;
using System.Text.Json.Serialization;
using csc13001_plant_pos.Model;

namespace csc13001_plant_pos.DTO.AuthDTO;

public class LoginResponseDTO
{
    [JsonPropertyName("user")]
    public required User User { get; set; }

    [JsonPropertyName("accessToken")]
    public required string AccessToken { get; set; }
}