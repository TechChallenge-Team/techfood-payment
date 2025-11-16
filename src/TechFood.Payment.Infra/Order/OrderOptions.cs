namespace TechFood.Payment.Infra.Order;

public class OrderOptions
{
    public const string SectionName = "TechFood:Order";

    public const string ClientName = "MercadoPagoClient";

    public const string BaseAddress = "https://api.mercadopago.com/";

    public string UserId { get; set; } = null!;

    public string AccessToken { get; set; } = null!;
}
