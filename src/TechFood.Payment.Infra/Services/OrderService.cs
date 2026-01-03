using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TechFood.Payment.Application.Common.Dto.Order;
using TechFood.Payment.Application.Common.Services.Interfaces;

namespace TechFood.Payment.Infra.Services;

internal class OrderService : IOrderService
{
    private readonly HttpClient _httpClient;

    public OrderService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<OrderDto> GetByIdAsync(Guid orderId)
    {
        var response = await _httpClient.GetAsync($"/v1/Orders/{orderId}");

        response.EnsureSuccessStatusCode();

        var deserializeObject = await response.Content.ReadFromJsonAsync<OrderDto>();

        return deserializeObject;
    }
}
