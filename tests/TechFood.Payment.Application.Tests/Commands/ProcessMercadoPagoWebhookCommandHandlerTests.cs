using MediatR;
using Microsoft.Extensions.Logging;
using TechFood.Payment.Application.Payments.Events.Integration.Outgoing;
using TechFood.Payment.Application.Webhooks.Commands.ProcessMercadoPagoWebhook;
using TechFood.Payment.Domain.Repositories;
using TechFood.Shared.Domain.Enums;

namespace TechFood.Payment.Application.Tests.Commands;

public class ProcessMercadoPagoWebhookCommandHandlerTests
{
    private readonly Mock<IPaymentRepository> _paymentRepositoryMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly ProcessMercadoPagoWebhookCommandHandler _handler;

    public ProcessMercadoPagoWebhookCommandHandlerTests()
    {
        _paymentRepositoryMock = new Mock<IPaymentRepository>();
        _mediatorMock = new Mock<IMediator>();

        _handler = new ProcessMercadoPagoWebhookCommandHandler(
            _paymentRepositoryMock.Object,
            _mediatorMock.Object,
            new Mock<ILogger<ProcessMercadoPagoWebhookCommandHandler>>().Object);
    }

    [Fact(DisplayName = "Handle should confirm payment when webhook is valid")]
    [Trait("Application", "ProcessMercadoPagoWebhookCommandHandler")]
    public async Task Handle_WithValidWebhook_ShouldConfirmPayment()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var command = new ProcessMercadoPagoWebhookCommand(
            "payment.approved",
            "payment",
            orderId.ToString(),
            123456789);

        var payment = new Domain.Entities.Payment(
            orderId,
            PaymentType.MercadoPago,
            100.00m);

        _paymentRepositoryMock
            .Setup(x => x.GetByOrderIdAsync(orderId))
            .ReturnsAsync(payment);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);
        payment.Status.Should().Be(PaymentStatusType.Approved);
        payment.PaidAt.Should().NotBeNull();

        _mediatorMock.Verify(
            x => x.Publish(
                It.Is<PaymentConfirmedEvent>(e => e.PaymentId == payment.Id && e.OrderId == orderId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact(DisplayName = "Handle should ignore webhook when type is not payment")]
    [Trait("Application", "ProcessMercadoPagoWebhookCommandHandler")]
    public async Task Handle_WithNonPaymentType_ShouldIgnoreWebhook()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var command = new ProcessMercadoPagoWebhookCommand(
            "notification.received",
            "merchant_account",
            orderId.ToString(),
            123456789);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);

        _paymentRepositoryMock.Verify(
            x => x.GetByOrderIdAsync(It.IsAny<Guid>()),
            Times.Never);

        _mediatorMock.Verify(
            x => x.Publish(It.IsAny<PaymentConfirmedEvent>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact(DisplayName = "Handle should return when webhook type is payment (case insensitive)")]
    [Trait("Application", "ProcessMercadoPagoWebhookCommandHandler")]
    public async Task Handle_WithPaymentTypeIgnoreCase_ShouldProcessWebhook()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var command = new ProcessMercadoPagoWebhookCommand(
            "payment.approved",
            "PAYMENT",
            orderId.ToString(),
            123456789);

        var payment = new Domain.Entities.Payment(
            orderId,
            PaymentType.MercadoPago,
            100.00m);

        _paymentRepositoryMock
            .Setup(x => x.GetByOrderIdAsync(orderId))
            .ReturnsAsync(payment);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);
        payment.Status.Should().Be(PaymentStatusType.Approved);

        _mediatorMock.Verify(
            x => x.Publish(It.IsAny<PaymentConfirmedEvent>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact(DisplayName = "Handle should return when DataId is not a valid GUID")]
    [Trait("Application", "ProcessMercadoPagoWebhookCommandHandler")]
    public async Task Handle_WithInvalidGuidDataId_ShouldIgnoreWebhook()
    {
        // Arrange
        var command = new ProcessMercadoPagoWebhookCommand(
            "payment.approved",
            "payment",
            "invalid-guid-format",
            123456789);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);

        _paymentRepositoryMock.Verify(
            x => x.GetByOrderIdAsync(It.IsAny<Guid>()),
            Times.Never);

        _mediatorMock.Verify(
            x => x.Publish(It.IsAny<PaymentConfirmedEvent>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact(DisplayName = "Handle should return when payment is not found")]
    [Trait("Application", "ProcessMercadoPagoWebhookCommandHandler")]
    public async Task Handle_WithNonExistentPayment_ShouldIgnoreWebhook()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var command = new ProcessMercadoPagoWebhookCommand(
            "payment.approved",
            "payment",
            orderId.ToString(),
            123456789);

        _paymentRepositoryMock
            .Setup(x => x.GetByOrderIdAsync(orderId))
            .ReturnsAsync((Domain.Entities.Payment?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);

        _mediatorMock.Verify(
            x => x.Publish(It.IsAny<PaymentConfirmedEvent>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact(DisplayName = "Handle should skip confirmation if payment is already confirmed")]
    [Trait("Application", "ProcessMercadoPagoWebhookCommandHandler")]
    public async Task Handle_WithAlreadyConfirmedPayment_ShouldSkipConfirmation()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var command = new ProcessMercadoPagoWebhookCommand(
            "payment.approved",
            "payment",
            orderId.ToString(),
            123456789);

        var payment = new Domain.Entities.Payment(
            orderId,
            PaymentType.MercadoPago,
            100.00m);
        
        // Confirm the payment first
        payment.Confirm();

        _paymentRepositoryMock
            .Setup(x => x.GetByOrderIdAsync(orderId))
            .ReturnsAsync(payment);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);

        _mediatorMock.Verify(
            x => x.Publish(It.IsAny<PaymentConfirmedEvent>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact(DisplayName = "Handle should publish event with correct payment and order IDs")]
    [Trait("Application", "ProcessMercadoPagoWebhookCommandHandler")]
    public async Task Handle_ShouldPublishEventWithCorrectIds()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var command = new ProcessMercadoPagoWebhookCommand(
            "payment.approved",
            "payment",
            orderId.ToString(),
            123456789);

        var payment = new Domain.Entities.Payment(
            orderId,
            PaymentType.MercadoPago,
            100.00m);

        _paymentRepositoryMock
            .Setup(x => x.GetByOrderIdAsync(orderId))
            .ReturnsAsync(payment);

        PaymentConfirmedEvent? publishedEvent = null;

        _mediatorMock
            .Setup(x => x.Publish(It.IsAny<PaymentConfirmedEvent>(), It.IsAny<CancellationToken>()))
            .Callback<PaymentConfirmedEvent, CancellationToken>((evt, ct) => publishedEvent = evt)
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        publishedEvent.Should().NotBeNull();
        publishedEvent!.PaymentId.Should().Be(payment.Id);
        publishedEvent.OrderId.Should().Be(orderId);
    }

    [Fact(DisplayName = "Handle should handle empty DataId")]
    [Trait("Application", "ProcessMercadoPagoWebhookCommandHandler")]
    public async Task Handle_WithEmptyDataId_ShouldIgnoreWebhook()
    {
        // Arrange
        var command = new ProcessMercadoPagoWebhookCommand(
            "payment.approved",
            "payment",
            string.Empty,
            123456789);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);

        _paymentRepositoryMock.Verify(
            x => x.GetByOrderIdAsync(It.IsAny<Guid>()),
            Times.Never);

        _mediatorMock.Verify(
            x => x.Publish(It.IsAny<PaymentConfirmedEvent>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
