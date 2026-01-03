using System.Net;
using System.Text;
using System.Text.Json;
using Moq.Protected;
using TechFood.Payment.Application.Common.Dto.Order;
using TechFood.Payment.Infra.Services;

namespace TechFood.Payment.Infra.Tests.Services;

public class OrderServiceTests
{
    [Fact(DisplayName = "GetByIdAsync should return order when found")]
    [Trait("Infra", "OrderService")]
    public async Task GetByIdAsync_WithValidId_ShouldReturnOrder()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var expectedOrder = new OrderDto
        {
            Id = orderId,
            Number = 123,
            Amount = 100.00m,
            Items = new List<OrderItemResult>
            {
                new OrderItemResult { Id = Guid.NewGuid(), Quantity = 2, Price = 50.00m }
            }
        };

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri!.PathAndQuery.Contains($"/v1/Orders/{orderId}")),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(expectedOrder), Encoding.UTF8, "application/json")
            });

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("http://localhost")
        };

        var service = new OrderService(httpClient);

        // Act
        var result = await service.GetByIdAsync(orderId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(orderId);
        result.Number.Should().Be(123);
        result.Amount.Should().Be(100.00m);
        result.Items.Should().HaveCount(1);
    }

    [Fact(DisplayName = "GetByIdAsync should throw exception when not found")]
    [Trait("Infra", "OrderService")]
    public async Task GetByIdAsync_WithInvalidId_ShouldThrowException()
    {
        // Arrange
        var orderId = Guid.NewGuid();

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound
            });

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("http://localhost")
        };

        var service = new OrderService(httpClient);

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => service.GetByIdAsync(orderId));
    }
}
