using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Moq.Protected;
using TechFood.Payment.Application.Common.Dto;
using TechFood.Payment.Application.Common.Dto.QrCode;
using TechFood.Payment.Infra.Payments.MercadoPago;

namespace TechFood.Payment.Infra.Tests.Services;

public class MercadoPagoPaymentServiceTests
{
    [Fact]
    public async Task GenerateQrCodePaymentAsync_ShouldReturnQrCodeResult_WhenSuccessful()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var posId = "POS123";
        var title = "Test Order";
        var amount = 100.50m;
        var items = new List<PaymentItem>
        {
            new("Item 1", 2, "unit", 50.25m, 100.50m)
        };

        var request = new QrCodePaymentRequest(
            posId,
            orderId,
            title,
            amount,
            items);

        var expectedResponse = new
        {
            id = "order-123",
            type_response = new { qr_data = "00020126580014br.gov.bcb.pix" }
        };

        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(expectedResponse)
            });

        var httpClient = new HttpClient(mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("https://api.mercadopago.com/")
        };

        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        mockHttpClientFactory
            .Setup(f => f.CreateClient(It.IsAny<string>()))
            .Returns(httpClient);

        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        var mockHttpContext = new Mock<HttpContext>();
        var mockHttpRequest = new Mock<HttpRequest>();
        
        mockHttpRequest.Setup(r => r.Scheme).Returns("https");
        mockHttpRequest.Setup(r => r.Host).Returns(new HostString("localhost"));
        mockHttpContext.Setup(c => c.Request).Returns(mockHttpRequest.Object);
        mockHttpContextAccessor.Setup(a => a.HttpContext).Returns(mockHttpContext.Object);

        var service = new MercadoPagoPaymentService(
            mockHttpClientFactory.Object,
            mockHttpContextAccessor.Object);

        // Act
        var result = await service.GenerateQrCodePaymentAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.QrCodeId.Should().Be("order-123");
        result.QrCodeData.Should().Be("00020126580014br.gov.bcb.pix");
    }

    [Fact]
    public async Task GenerateQrCodePaymentAsync_ShouldThrowException_WhenApiReturnsError()
    {
        // Arrange
        var request = new QrCodePaymentRequest(
            "POS123",
            Guid.NewGuid(),
            "Test Order",
            100m,
            new List<PaymentItem>
            {
                new("Item 1", 1, "unit", 100m, 100m)
            });

        var errorResponse = new
        {
            errors = new[]
            {
                new { code = "400", message = "Invalid request" }
            }
        };

        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = JsonContent.Create(errorResponse)
            });

        var httpClient = new HttpClient(mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("https://api.mercadopago.com/")
        };

        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        mockHttpClientFactory
            .Setup(f => f.CreateClient(It.IsAny<string>()))
            .Returns(httpClient);

        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        var mockHttpContext = new Mock<HttpContext>();
        var mockHttpRequest = new Mock<HttpRequest>();
        
        mockHttpRequest.Setup(r => r.Scheme).Returns("https");
        mockHttpRequest.Setup(r => r.Host).Returns(new HostString("localhost"));
        mockHttpContext.Setup(c => c.Request).Returns(mockHttpRequest.Object);
        mockHttpContextAccessor.Setup(a => a.HttpContext).Returns(mockHttpContext.Object);

        var service = new MercadoPagoPaymentService(
            mockHttpClientFactory.Object,
            mockHttpContextAccessor.Object);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => 
            service.GenerateQrCodePaymentAsync(request));
    }

}
