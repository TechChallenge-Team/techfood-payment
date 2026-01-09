using System.Security.Cryptography;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TechFood.Payment.Api.Controllers;
using TechFood.Payment.Application.Webhooks.Commands.ProcessMercadoPagoWebhook;
using TechFood.Payment.Contracts.Webhooks;
using TechFood.Payment.Infra.Payments.MercadoPago;

namespace TechFood.Payment.Api.Tests.Controllers;

public class WebhooksControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<ILogger<WebhooksController>> _loggerMock;
    private readonly Mock<IOptions<MercadoPagoOptions>> _optionsMock;
    private readonly WebhooksController _controller;
    private readonly string _secretKey = "test-secret-key";

    public WebhooksControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _loggerMock = new Mock<ILogger<WebhooksController>>();
        _optionsMock = new Mock<IOptions<MercadoPagoOptions>>();

        _optionsMock.Setup(x => x.Value).Returns(new MercadoPagoOptions { WebhookSecret = _secretKey });

        _controller = new WebhooksController(
            _mediatorMock.Object,
            _loggerMock.Object,
            _optionsMock.Object);

        InitializeControllerContext();
    }

    private void InitializeControllerContext()
    {
        var httpContext = new DefaultHttpContext();
        var request = httpContext.Request;
        _controller.ControllerContext = new ControllerContext { HttpContext = httpContext };
    }

    [Fact(DisplayName = "InvokeAsync should return Ok with valid webhook")]
    [Trait("Api", "WebhooksController")]
    public async Task InvokeAsync_WithValidRequest_ShouldReturnOk()
    {
        // Arrange
        var dataId = Guid.NewGuid().ToString();
        var requestId = Guid.NewGuid().ToString();
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();

        var webhookRequest = new MercadoPagoWebhookRequest
        {
            Action = "payment.created",
            Type = "payment",
            Data = new MercadoPagoWebhookData { Id = dataId },
            UserId = 123456789,
            DateCreated = DateTime.UtcNow.ToString()
        };

        var serialized = JsonConvert.SerializeObject(webhookRequest);
        var xSignature = GenerateSignature(dataId, requestId, timestamp);

        SetupHttpContext(requestId, timestamp, xSignature, dataId);

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<ProcessMercadoPagoWebhookCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        // Act
        var result = await _controller.InvokeAsync(serialized);

        // Assert
        result.Should().BeOfType<OkResult>();

        _mediatorMock.Verify(
            x => x.Send(
                It.Is<ProcessMercadoPagoWebhookCommand>(c =>
                    c.Action == "payment.created" &&
                    c.Type == "payment" &&
                    c.DataId == dataId &&
                    c.UserId == 123456789),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact(DisplayName = "InvokeAsync should return NotFound with invalid signature")]
    [Trait("Api", "WebhooksController")]
    public async Task InvokeAsync_WithInvalidSignature_ShouldReturnNotFound()
    {
        // Arrange
        var dataId = Guid.NewGuid().ToString();
        var requestId = Guid.NewGuid().ToString();
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();

        var webhookRequest = new MercadoPagoWebhookRequest
        {
            Action = "payment.created",
            Type = "payment",
            Data = new MercadoPagoWebhookData { Id = dataId },
            UserId = 123456789,
            DateCreated = DateTime.UtcNow.ToString()
        };

        var serialized = JsonConvert.SerializeObject(webhookRequest);
        var invalidSignature = "ts=" + timestamp + ",v1=invalid_signature";

        SetupHttpContextWithInvalidSignature(requestId, invalidSignature, dataId);

        // Act
        var result = await _controller.InvokeAsync(serialized);

        // Assert
        result.Should().BeOfType<NotFoundResult>();

        _mediatorMock.Verify(
            x => x.Send(It.IsAny<ProcessMercadoPagoWebhookCommand>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact(DisplayName = "InvokeAsync should skip signature validation when no secret is configured")]
    [Trait("Api", "WebhooksController")]
    public async Task InvokeAsync_WithoutSecretKey_ShouldSkipSignatureValidation()
    {
        // Arrange
        var controllerWithoutSecret = new WebhooksController(
            _mediatorMock.Object,
            _loggerMock.Object,
            Options.Create(new MercadoPagoOptions { WebhookSecret = null }));

        controllerWithoutSecret.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };

        var dataId = Guid.NewGuid().ToString();
        var webhookRequest = new MercadoPagoWebhookRequest
        {
            Action = "payment.created",
            Type = "payment",
            Data = new MercadoPagoWebhookData { Id = dataId },
            UserId = 123456789,
            DateCreated = DateTime.UtcNow.ToString()
        };

        var serialized = JsonConvert.SerializeObject(webhookRequest);

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<ProcessMercadoPagoWebhookCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        // Act
        var result = await controllerWithoutSecret.InvokeAsync(serialized);

        // Assert
        result.Should().BeOfType<OkResult>();

        _mediatorMock.Verify(
            x => x.Send(It.IsAny<ProcessMercadoPagoWebhookCommand>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact(DisplayName = "InvokeAsync should return BadRequest with invalid JSON")]
    [Trait("Api", "WebhooksController")]
    public async Task InvokeAsync_WithInvalidJson_ShouldThrowJsonException()
    {
        // Arrange
        InitializeControllerContext();
        var invalidJson = "{ invalid json }";

        // Act & Assert
        await Assert.ThrowsAsync<Newtonsoft.Json.JsonReaderException>(() => 
            _controller.InvokeAsync(invalidJson));
    }

    [Fact(DisplayName = "InvokeAsync should handle webhook without signature header")]
    [Trait("Api", "WebhooksController")]
    public async Task InvokeAsync_WithoutSignatureHeader_ShouldProcessWebhook()
    {
        // Arrange
        InitializeControllerContext();

        var dataId = Guid.NewGuid().ToString();
        var webhookRequest = new MercadoPagoWebhookRequest
        {
            Action = "payment.created",
            Type = "payment",
            Data = new MercadoPagoWebhookData { Id = dataId },
            UserId = 123456789,
            DateCreated = DateTime.UtcNow.ToString()
        };

        var serialized = JsonConvert.SerializeObject(webhookRequest);

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<ProcessMercadoPagoWebhookCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        // Act
        var result = await _controller.InvokeAsync(serialized);

        // Assert
        result.Should().BeOfType<OkResult>();

        _mediatorMock.Verify(
            x => x.Send(It.IsAny<ProcessMercadoPagoWebhookCommand>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact(DisplayName = "InvokeAsync should handle different action types")]
    [Trait("Api", "WebhooksController")]
    public async Task InvokeAsync_WithDifferentActions_ShouldProcessCorrectly()
    {
        // Arrange
        var dataId = Guid.NewGuid().ToString();
        var requestId = Guid.NewGuid().ToString();
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        var actionType = "payment.updated";

        var webhookRequest = new MercadoPagoWebhookRequest
        {
            Action = actionType,
            Type = "payment",
            Data = new MercadoPagoWebhookData { Id = dataId },
            UserId = 987654321,
            DateCreated = DateTime.UtcNow.ToString()
        };

        var serialized = JsonConvert.SerializeObject(webhookRequest);
        var xSignature = GenerateSignature(dataId, requestId, timestamp);

        SetupHttpContext(requestId, timestamp, xSignature, dataId);

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<ProcessMercadoPagoWebhookCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        // Act
        var result = await _controller.InvokeAsync(serialized);

        // Assert
        result.Should().BeOfType<OkResult>();

        _mediatorMock.Verify(
            x => x.Send(
                It.Is<ProcessMercadoPagoWebhookCommand>(c => c.Action == actionType),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    private void SetupHttpContext(string requestId, string timestamp, string xSignature, string dataId)
    {
        var httpContext = _controller.HttpContext;
        httpContext.Request.Headers["x-request-id"] = requestId;
        httpContext.Request.Headers["x-signature"] = xSignature;
        httpContext.Request.QueryString = new QueryString($"?data.id={dataId}");
    }

    private void SetupHttpContextWithInvalidSignature(string requestId, string invalidSignature, string dataId)
    {
        var httpContext = _controller.HttpContext;
        httpContext.Request.Headers["x-request-id"] = requestId;
        httpContext.Request.Headers["x-signature"] = invalidSignature;
        httpContext.Request.QueryString = new QueryString($"?data.id={dataId}");
    }

    private string GenerateSignature(string dataId, string requestId, string timestamp)
    {
        var manifest = $"id:{dataId};request-id:{requestId};ts:{timestamp};";
        var keyBytes = Encoding.UTF8.GetBytes(_secretKey);
        var messageBytes = Encoding.UTF8.GetBytes(manifest);

        using var hmac = new HMACSHA256(keyBytes);
        var hashBytes = hmac.ComputeHash(messageBytes);
        var hash = Convert.ToHexString(hashBytes).ToLowerInvariant();

        return $"ts={timestamp},v1={hash}";
    }
}
