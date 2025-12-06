using System.Net;
using System.Text;
using System.Text.Json;
using Moq.Protected;
using TechFood.Payment.Application.Common.Dto.Product;
using TechFood.Payment.Infra.Backoffice;

namespace TechFood.Payment.Infra.Tests.Services;

public class BackofficeServiceTests
{
    [Fact(DisplayName = "GetAllAsync should return products list")]
    [Trait("Infra", "BackofficeService")]
    public async Task GetAllAsync_ShouldReturnProductsList()
    {
        // Arrange
        var expectedProducts = new List<ProductDto>
        {
            new ProductDto { Id = Guid.NewGuid() },
            new ProductDto { Id = Guid.NewGuid() }
        };

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri!.PathAndQuery.Contains("/v1/products")),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(expectedProducts), Encoding.UTF8, "application/json")
            });

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("http://localhost")
        };

        var service = new BackofficeService(httpClient);

        // Act
        var result = await service.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
    }

    [Fact(DisplayName = "GetAllAsync should return empty list when no products")]
    [Trait("Infra", "BackofficeService")]
    public async Task GetAllAsync_WhenNoProducts_ShouldReturnEmptyList()
    {
        // Arrange
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("null", Encoding.UTF8, "application/json")
            });

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("http://localhost")
        };

        var service = new BackofficeService(httpClient);

        // Act
        var result = await service.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact(DisplayName = "GetAllAsync should throw exception on error")]
    [Trait("Infra", "BackofficeService")]
    public async Task GetAllAsync_OnError_ShouldThrowException()
    {
        // Arrange
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError
            });

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("http://localhost")
        };

        var service = new BackofficeService(httpClient);

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => service.GetAllAsync());
    }
}
