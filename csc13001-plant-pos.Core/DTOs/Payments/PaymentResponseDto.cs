using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;


namespace csc13001_plant_pos.Core.DTOs.Payments;
public class PaymentResponseDto
{
    [JsonPropertyName("code")]
    string Code
    {
        get; set;
    }

    [JsonPropertyName("message")]
    string Desc
    {
        get; set;
    }

    [JsonPropertyName("data")]
    PaymentResponseDataDto Data
    {
        get; set;
    }

    [JsonPropertyName("signature")]
    string Signature
    {
        get; set;
    }
}

