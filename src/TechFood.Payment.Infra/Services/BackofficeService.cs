using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using TechFood.Payment.Application.Common.Dto.Product;
using TechFood.Payment.Application.Common.Services.Interfaces;

namespace TechFood.Payment.Infra.Services
{
    public class BackofficeService : IBackofficeService
    {
        private readonly HttpClient _httpClient;

        public BackofficeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetAsync("/v1/products", cancellationToken);

            response.EnsureSuccessStatusCode();

            var products = await response.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>(cancellationToken);

            return products ?? [];
        }
    }
}
