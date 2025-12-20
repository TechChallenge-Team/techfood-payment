using MediatR;
using Microsoft.Extensions.Logging;
using TechFood.Payment.Application.Payments.Commands.CreatePayment;
using TechFood.Payment.Application.Payments.Dto;
using TechFood.Payment.Application.Payments.Events;
using TechFood.Shared.Domain.Enums;

namespace TechFood.Payment.Application.Tests.Events;

public class OrderCreatedEventHandlerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<ILogger<OrderCreatedEventHandler>> _loggerMock;
    private readonly OrderCreatedEventHandler _handler;

    public OrderCreatedEventHandlerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _loggerMock = new Mock<ILogger<OrderCreatedEventHandler>>();
        _handler = new OrderCreatedEventHandler(_mediatorMock.Object, _loggerMock.Object);
    }

    [Fact(DisplayName = "Handle should process OrderCreated event and create payment successfully")]
    [Trait("Application", "OrderCreatedEventHandler")]
    public async Task Handle_WithValidEvent_ShouldCreatePaymentSuccessfully()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var items = new List<OrderItemCreatedDto>
        {
            new(Guid.NewGuid(), "Burger", 25.50m, 2),
            new(Guid.NewGuid(), "Fries", 10.00m, 1)
        };
        var notification = new OrderCreatedIntegrationEvent(orderId, items);

        var paymentDto = new PaymentDto(
            Guid.NewGuid(),
            orderId,
            DateTime.UtcNow,
            null,
            PaymentType.MercadoPago,
            PaymentStatusType.Pending,
            61.00m
        );

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<CreatePaymentCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(paymentDto);

        // Act
        await _handler.Handle(notification, CancellationToken.None);

        // Assert
        _mediatorMock.Verify(
            x => x.Send(
                It.Is<CreatePaymentCommand>(cmd =>
                    cmd.OrderId == orderId &&
                    cmd.Type == PaymentType.MercadoPago),
                It.IsAny<CancellationToken>()),
            Times.Once);

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains($"Order created event received for OrderId: {orderId}")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Payment created successfully")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact(DisplayName = "Handle should log error and rethrow when payment creation fails")]
    [Trait("Application", "OrderCreatedEventHandler")]
    public async Task Handle_WhenPaymentCreationFails_ShouldLogErrorAndRethrow()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var items = new List<OrderItemCreatedDto>
        {
            new(Guid.NewGuid(), "Product", 50.00m, 1)
        };
        var notification = new OrderCreatedIntegrationEvent(orderId, items);
        var expectedException = new Exception("Payment service unavailable");

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<CreatePaymentCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            _handler.Handle(notification, CancellationToken.None));

        exception.Should().Be(expectedException);

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains($"Error creating payment for OrderId: {orderId}")),
                expectedException,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact(DisplayName = "Handle should create payment command with correct OrderId")]
    [Trait("Application", "OrderCreatedEventHandler")]
    public async Task Handle_ShouldCreateCommandWithCorrectOrderId()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var items = new List<OrderItemCreatedDto>();
        var notification = new OrderCreatedIntegrationEvent(orderId, items);

        var paymentDto = new PaymentDto(
            Guid.NewGuid(),
            orderId,
            DateTime.UtcNow,
            null,
            PaymentType.MercadoPago,
            PaymentStatusType.Pending,
            0m
        );

        CreatePaymentCommand? capturedCommand = null;
        _mediatorMock
            .Setup(x => x.Send(It.IsAny<CreatePaymentCommand>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest<PaymentDto>, CancellationToken>((cmd, ct) =>
                capturedCommand = cmd as CreatePaymentCommand)
            .ReturnsAsync(paymentDto);

        // Act
        await _handler.Handle(notification, CancellationToken.None);

        // Assert
        capturedCommand.Should().NotBeNull();
        capturedCommand!.OrderId.Should().Be(orderId);
        capturedCommand.Type.Should().Be(PaymentType.MercadoPago);
    }

    [Fact(DisplayName = "Handle should respect cancellation token")]
    [Trait("Application", "OrderCreatedEventHandler")]
    public async Task Handle_WithCancellationToken_ShouldPassTokenToMediator()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var items = new List<OrderItemCreatedDto>();
        var notification = new OrderCreatedIntegrationEvent(orderId, items);
        var cancellationToken = new CancellationToken();

        var paymentDto = new PaymentDto(
            Guid.NewGuid(),
            orderId,
            DateTime.UtcNow,
            null,
            PaymentType.MercadoPago,
            PaymentStatusType.Pending,
            0m
        );

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<CreatePaymentCommand>(), cancellationToken))
            .ReturnsAsync(paymentDto);

        // Act
        await _handler.Handle(notification, cancellationToken);

        // Assert
        _mediatorMock.Verify(
            x => x.Send(It.IsAny<CreatePaymentCommand>(), cancellationToken),
            Times.Once);
    }

    [Fact(DisplayName = "Handle should log payment ID after successful creation")]
    [Trait("Application", "OrderCreatedEventHandler")]
    public async Task Handle_AfterSuccessfulCreation_ShouldLogPaymentId()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var paymentId = Guid.NewGuid();
        var items = new List<OrderItemCreatedDto>
        {
            new(Guid.NewGuid(), "Test Product", 100.00m, 1)
        };
        var notification = new OrderCreatedIntegrationEvent(orderId, items);

        var paymentDto = new PaymentDto(
            paymentId,
            orderId,
            DateTime.UtcNow,
            null,
            PaymentType.MercadoPago,
            PaymentStatusType.Pending,
            100.00m
        );

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<CreatePaymentCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(paymentDto);

        // Act
        await _handler.Handle(notification, CancellationToken.None);

        // Assert
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) =>
                    v.ToString()!.Contains($"PaymentId: {paymentId}") &&
                    v.ToString()!.Contains($"OrderId: {orderId}")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact(DisplayName = "Handle should work with empty items list")]
    [Trait("Application", "OrderCreatedEventHandler")]
    public async Task Handle_WithEmptyItemsList_ShouldStillCreatePayment()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var items = new List<OrderItemCreatedDto>();
        var notification = new OrderCreatedIntegrationEvent(orderId, items);

        var paymentDto = new PaymentDto(
            Guid.NewGuid(),
            orderId,
            DateTime.UtcNow,
            null,
            PaymentType.MercadoPago,
            PaymentStatusType.Pending,
            0m
        );

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<CreatePaymentCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(paymentDto);

        // Act
        await _handler.Handle(notification, CancellationToken.None);

        // Assert
        _mediatorMock.Verify(
            x => x.Send(It.IsAny<CreatePaymentCommand>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact(DisplayName = "Handle should work with multiple items")]
    [Trait("Application", "OrderCreatedEventHandler")]
    public async Task Handle_WithMultipleItems_ShouldCreatePayment()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var items = new List<OrderItemCreatedDto>
        {
            new(Guid.NewGuid(), "Burger", 25.00m, 2),
            new(Guid.NewGuid(), "Fries", 10.00m, 3),
            new(Guid.NewGuid(), "Soda", 5.00m, 2)
        };
        var notification = new OrderCreatedIntegrationEvent(orderId, items);

        var paymentDto = new PaymentDto(
            Guid.NewGuid(),
            orderId,
            DateTime.UtcNow,
            null,
            PaymentType.MercadoPago,
            PaymentStatusType.Pending,
            90.00m
        );

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<CreatePaymentCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(paymentDto);

        // Act
        await _handler.Handle(notification, CancellationToken.None);

        // Assert
        _mediatorMock.Verify(
            x => x.Send(
                It.Is<CreatePaymentCommand>(cmd => cmd.OrderId == orderId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact(DisplayName = "Handle should handle ApplicationException and rethrow")]
    [Trait("Application", "OrderCreatedEventHandler")]
    public async Task Handle_WhenApplicationExceptionOccurs_ShouldLogAndRethrow()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var items = new List<OrderItemCreatedDto>
        {
            new(Guid.NewGuid(), "Product", 50.00m, 1)
        };
        var notification = new OrderCreatedIntegrationEvent(orderId, items);
        var expectedException = new ApplicationException("Order not found");

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<CreatePaymentCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ApplicationException>(() =>
            _handler.Handle(notification, CancellationToken.None));

        exception.Should().Be(expectedException);
        exception.Message.Should().Be("Order not found");

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Error creating payment")),
                expectedException,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact(DisplayName = "Handle should use MercadoPago as default payment type")]
    [Trait("Application", "OrderCreatedEventHandler")]
    public async Task Handle_ShouldUseMercadoPagoAsDefaultPaymentType()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var items = new List<OrderItemCreatedDto>
        {
            new(Guid.NewGuid(), "Product", 99.99m, 1)
        };
        var notification = new OrderCreatedIntegrationEvent(orderId, items);

        var paymentDto = new PaymentDto(
            Guid.NewGuid(),
            orderId,
            DateTime.UtcNow,
            null,
            PaymentType.MercadoPago,
            PaymentStatusType.Pending,
            99.99m
        );

        CreatePaymentCommand? capturedCommand = null;
        _mediatorMock
            .Setup(x => x.Send(It.IsAny<CreatePaymentCommand>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest<PaymentDto>, CancellationToken>((cmd, ct) =>
                capturedCommand = cmd as CreatePaymentCommand)
            .ReturnsAsync(paymentDto);

        // Act
        await _handler.Handle(notification, CancellationToken.None);

        // Assert
        capturedCommand.Should().NotBeNull();
        capturedCommand!.Type.Should().Be(PaymentType.MercadoPago);
    }

    [Fact(DisplayName = "Handle should log initial information message before processing")]
    [Trait("Application", "OrderCreatedEventHandler")]
    public async Task Handle_ShouldLogInitialInformationMessage()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var items = new List<OrderItemCreatedDto>();
        var notification = new OrderCreatedIntegrationEvent(orderId, items);

        var paymentDto = new PaymentDto(
            Guid.NewGuid(),
            orderId,
            DateTime.UtcNow,
            null,
            PaymentType.MercadoPago,
            PaymentStatusType.Pending,
            0m
        );

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<CreatePaymentCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(paymentDto);

        // Act
        await _handler.Handle(notification, CancellationToken.None);

        // Assert
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Order created event received")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}
