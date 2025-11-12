namespace TechFood.Infra.Payments.MercadoPago
{
    public class MercadoPagoOptions
    {
        public const string SectionName = "Payments:MercadoPago";

        public const string ClientName = "MercadoPagoClient";

        public const string BaseAddress = "https://api.mercadopago.com/";

        public string UserId { get; set; } = null!;

        public string AccessToken { get; set; } = null!;
    }
}
