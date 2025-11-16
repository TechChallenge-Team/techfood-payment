
namespace TechFood.Payment.Infra.Product
{
    internal class ProductOptions
    {
        public const string SectionName = "TechFood:Order";

        public const string ClientName = "MercadoPagoClient";

        public const string BaseAddress = "http://localhost:45001";

        public string UserId { get; set; } = null!;

        public string AccessToken { get; set; } = null!;
    }
}
