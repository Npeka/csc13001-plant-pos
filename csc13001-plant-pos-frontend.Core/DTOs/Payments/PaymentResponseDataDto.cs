using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace csc13001_plant_pos_frontend.Core.DTOs.Payments;
public class PaymentResponseDataDto
{
    [JsonPropertyName("bin")]
    string Bin
    {
        get; set;
    }

    [JsonPropertyName("accountNumber")]
    string AccountNumber
    {
        get; set;
    }

    [JsonPropertyName("accountName")]
    string AccountName
    {
        get; set;
    }

    [JsonPropertyName("currency")]
    string Currency
    {
        get; set;
    }

    [JsonPropertyName("paymentLinkId")]
    string PaymentLinkId
    {
        get; set;
    }

    [JsonPropertyName("amount")]
    int Amount
    {
        get; set;
    }

    [JsonPropertyName("description")]
    string Description
    {
        get; set;
    }

    [JsonPropertyName("orderCode")]
    int OrderCode
    {
        get; set;
    }

    [JsonPropertyName("status")]
    string Status
    {
        get; set;
    }

    [JsonPropertyName("checkoutUrl")]
    string CheckoutUrl
    {
        get; set;
    }

    [JsonPropertyName("qrCode")]
    string QrCode
    {
        get; set;
    }
}