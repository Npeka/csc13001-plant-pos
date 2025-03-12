using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace csc13001_plant_pos_frontend.Core.DTOs.Payments;
public class PaymentItemDto
{
    [JsonPropertyName("name")]
    public string Name
    {
        get; set;
    }

    [JsonPropertyName("quantity")]
    public int Quantity
    {
        get; set;
    }

    [JsonPropertyName("price")]
    public int Price
    {
        get; set;
    }
}
