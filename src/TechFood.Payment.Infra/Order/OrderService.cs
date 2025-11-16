using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TechFood.Payment.Application.Common.Services.Interfaces;

namespace TechFood.Payment.Infra.Order
{
    internal class OrderService(
        IHttpClientFactory httpClientFactory,
        IHttpContextAccessor httpContextAccessor
        ) : IOrderService
    {
        private readonly HttpClient _client = httpClientFactory.CreateClient(OrderOptions.ClientName);
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower)
            },
        };

        public async Task<Application.Common.Data.Order.OrderResult> GetByIdAsync(Guid orderId)
        {
            var response = await _client.GetAsync($"/v1/Orders/{orderId}");

            response.EnsureSuccessStatusCode();

            var deserializeObject = await response.Content.ReadAsStringAsync();

            return new();
        }

    }
}
