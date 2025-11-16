using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TechFood.Payment.Application.Common.Data.Product;
using TechFood.Payment.Application.Common.Services.Interfaces;
using TechFood.Payment.Infra.Order;

namespace TechFood.Payment.Infra.Product
{
    public class ProductService(IHttpClientFactory httpClientFactory,
        IHttpContextAccessor httpContextAccessor
        ) : IProductService
    {
        private readonly HttpClient _client = httpClientFactory.CreateClient(ProductOptions.ClientName);
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower)
            },
        };

        public async Task<IEnumerable<ProductResult>> GetAllAsync()
        {
            var response = await _client.GetAsync($"/v1/products");

            response.EnsureSuccessStatusCode();

            var deserializeObject = await response.Content.ReadAsStringAsync();

            return Enumerable.Empty<ProductResult>();
        }
    }
}
