using System.Text.Json.Serialization;

namespace TechFood.Payment.Contracts.Webhooks;

public record MercadoPagoWebhookRequest
{
    [JsonPropertyName("action")]
    public string Action { get; init; } = string.Empty;

    [JsonPropertyName("data")]
    public MercadoPagoWebhookData Data { get; init; } = new();

    [JsonPropertyName("date_created")]
    public string DateCreated { get; init; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; init; } = string.Empty;

    [JsonPropertyName("user_id")]
    public long UserId { get; init; }
}

public record MercadoPagoWebhookData
{
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;
}
