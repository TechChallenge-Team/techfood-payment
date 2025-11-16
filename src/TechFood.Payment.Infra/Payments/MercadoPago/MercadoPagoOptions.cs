namespace TechFood.Payment.Infra.Payments.MercadoPago
{
    public class MercadoPagoOptions
    {
        public const string SectionName = "Payments:MercadoPago";

        public const string ClientName = "OrderClient";

        public const string BaseAddress = "http://localhost:45001";

        public string UserId { get; set; } = null!;

        public string AccessToken { get; set; } = null!;
    }
}
