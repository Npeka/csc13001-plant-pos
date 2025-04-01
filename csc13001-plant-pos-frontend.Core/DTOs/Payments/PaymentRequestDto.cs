using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace csc13001_plant_pos_frontend.Core.DTOs.Payments;
public class PaymentRequestDto
{
    [JsonPropertyName("orderCode")]
    public int OrderCode
    {
        get; set;
    }

    [JsonPropertyName("amount")]
    public int Amount
    {
        get; set;
    }

    [JsonPropertyName("description")]
    public string Description
    {
        get; set;
    }

    [JsonPropertyName("buyerName")]
    public string BuyerName
    {
        get; set;
    }

    [JsonPropertyName("buyerEmail")]
    public string BuyerEmail
    {
        get; set;
    }

    [JsonPropertyName("buyerPhone")]
    public string BuyerPhone
    {
        get; set;
    }

    [JsonPropertyName("buyerAddress")]
    public string BuyerAddress
    {
        get; set;
    }

    [JsonPropertyName("items")]
    public List<PaymentItemDto> Items { get; set; } = new();

    [JsonPropertyName("cancelUrl")]
    public string CancelUrl
    {
        get; set;
    }

    [JsonPropertyName("returnUrl")]
    public string ReturnUrl
    {
        get; set;
    }

    [JsonPropertyName("expiredAt")]
    public long ExpiredAt
    {
        get; set;
    }

    [JsonPropertyName("signature")]
    public string Signature
    {
        get; private set;
    }

    public void GenerateSignature(string checksumKey)
    {
        var data = $"amount={Amount}&cancelUrl={CancelUrl}&description={Description}&orderCode={OrderCode}&returnUrl={ReturnUrl}";
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(checksumKey));
        Signature = BitConverter.ToString(hmac.ComputeHash(Encoding.UTF8.GetBytes(data))).Replace("-", "").ToLower();
    }
}
