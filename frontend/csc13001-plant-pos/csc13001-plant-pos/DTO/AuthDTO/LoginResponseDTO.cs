using System;
using System.Text.Json.Serialization;

namespace csc13001_plant_pos.DTO.AuthDTO;

public class LoginResponseDTO
{
    [JsonPropertyName("user")]
    public required UserDTO User
    {
        get; set;
    }

    [JsonPropertyName("accessToken")]
    public required string AccessToken
    {
        get; set;
    }
}

public class UserDTO
{
    [JsonPropertyName("userId")]
    public int UserId
    {
        get; set;
    }

    [JsonPropertyName("fullname")]
    public string Fullname
    {
        get; set;
    }

    [JsonPropertyName("email")]
    public string Email
    {
        get; set;
    }

    [JsonPropertyName("phone")]
    public string Phone
    {
        get; set;
    }

    [JsonPropertyName("isAdmin")]
    public bool IsAdmin
    {
        get; set;
    }
}
