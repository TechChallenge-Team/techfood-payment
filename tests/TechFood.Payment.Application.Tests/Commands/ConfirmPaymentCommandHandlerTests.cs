using MediatR;
using TechFood.Payment.Application.Payments.Commands.ConfirmPayment;
using TechFood.Payment.Application.Payments.Events;
using TechFood.Payment.Domain.Repositories;
using TechFood.Shared.Domain.Enums;

namespace TechFood.Payment.Application.Tests.Commands;

public class ConfirmPaymentCommandHandlerTests
{
    private readonly Mock<IPaymentRepository> _paymentRepositoryMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly ConfirmPaymentCommandHandler _handler;

    public ConfirmPaymentCommandHandlerTests()
    {
        _paymentRepositoryMock = new Mock<IPaymentRepository>();
        _mediatorMock = new Mock<IMediator>();

        _handler = new ConfirmPaymentCommandHandler(
            _paymentRepositoryMock.Object,
            _mediatorMock.Object);
    }

    [Fact(DisplayName = "Should confirm payment successfully")]
    [Trait("Application", "ConfirmPaymentCommandHandler")]
    public async Task Handle_WithValidPayment_ShouldConfirmSuccessfully()
    {
        // Arrange
        var paymentId = Guid.NewGuid();
        var payment = new Domain.Entities.Payment(
            Guid.NewGuid(),
            PaymentType.MercadoPago,
            100.00m);

        var command = new ConfirmPaymentCommand(paymentId);

        _paymentRepositoryMock
            .Setup(x => x.GetByIdAsync(paymentId))
            .ReturnsAsync(payment);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);
        payment.Status.Should().Be(PaymentStatusType.Approved);
        payment.PaidAt.Should().NotBeNull();

        _mediatorMock.Verify(
            x => x.Publish(It.IsAny<PaymentConfirmedEvent>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact(DisplayName = "Should throw exception when payment is not found")]
    [Trait("Application", "ConfirmPaymentCommandHandler")]
    public async Task Handle_WithNonExistentPayment_ShouldThrowException()
    {
        // Arrange
        var paymentId = Guid.NewGuid();
        var command = new ConfirmPaymentCommand(paymentId);

        _paymentRepositoryMock
            .Setup(x => x.GetByIdAsync(paymentId))
            .ReturnsAsync((Domain.Entities.Payment?)null);

        // Act & Assert
        await Assert.ThrowsAsync<TechFood.Shared.Application.Exceptions.ApplicationException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact(DisplayName = "Should publish ConfirmedPaymentEvent after confirmation")]
    [Trait("Application", "ConfirmPaymentCommandHandler")]
    public async Task Handle_AfterConfirmation_ShouldPublishEvent()
    {
        // Arrange
        var paymentId = Guid.NewGuid();
        var payment = new Domain.Entities.Payment(
            Guid.NewGuid(),
            PaymentType.CreditCard,
            250.00m);

        var command = new ConfirmPaymentCommand(paymentId);

        _paymentRepositoryMock
            .Setup(x => x.GetByIdAsync(paymentId))
            .ReturnsAsync(payment);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mediatorMock.Verify(
            x => x.Publish(
                It.IsAny<PaymentConfirmedEvent>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact(DisplayName = "Should update payment status to Paid")]
    [Trait("Application", "ConfirmPaymentCommandHandler")]
    public async Task Handle_ShouldUpdatePaymentStatusToPaid()
    {
        // Arrange
        var paymentId = Guid.NewGuid();
        var payment = new Domain.Entities.Payment(
            Guid.NewGuid(),
            PaymentType.CreditCard,
            75.50m);

        var command = new ConfirmPaymentCommand(paymentId);

        _paymentRepositoryMock
            .Setup(x => x.GetByIdAsync(paymentId))
            .ReturnsAsync(payment);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        payment.Status.Should().Be(PaymentStatusType.Approved);
    }

    [Fact(DisplayName = "Should set PaidAt timestamp when confirming")]
    [Trait("Application", "ConfirmPaymentCommandHandler")]
    public async Task Handle_ShouldSetPaidAtTimestamp()
    {
        // Arrange
        var paymentId = Guid.NewGuid();
        var beforeConfirmation = DateTime.Now;
        var payment = new Domain.Entities.Payment(
            Guid.NewGuid(),
            PaymentType.MercadoPago,
            150.00m);

        var command = new ConfirmPaymentCommand(paymentId);

        _paymentRepositoryMock
            .Setup(x => x.GetByIdAsync(paymentId))
            .ReturnsAsync(payment);

        // Act
        await _handler.Handle(command, CancellationToken.None);
        var afterConfirmation = DateTime.Now;

        // Assert
        payment.PaidAt.Should().NotBeNull();
        payment.PaidAt.Should().BeOnOrAfter(beforeConfirmation);
        payment.PaidAt.Should().BeOnOrBefore(afterConfirmation);
    }

    [Fact(DisplayName = "Should handle different payment types")]
    [Trait("Application", "ConfirmPaymentCommandHandler")]
    public async Task Handle_WithDifferentPaymentTypes_ShouldConfirmAll()
    {
        // Arrange & Act & Assert
        var paymentTypes = new[] { PaymentType.MercadoPago, PaymentType.CreditCard };

        foreach (var paymentType in paymentTypes)
        {
            var paymentId = Guid.NewGuid();
            var payment = new Domain.Entities.Payment(
                Guid.NewGuid(),
                paymentType,
                100.00m);

            var command = new ConfirmPaymentCommand(paymentId);

            _paymentRepositoryMock
                .Setup(x => x.GetByIdAsync(paymentId))
                .ReturnsAsync(payment);

            await _handler.Handle(command, CancellationToken.None);

            payment.Status.Should().Be(PaymentStatusType.Approved);
            payment.Type.Should().Be(paymentType);
        }
    }

    [Fact(DisplayName = "Should retrieve payment from repository")]
    [Trait("Application", "ConfirmPaymentCommandHandler")]
    public async Task Handle_ShouldRetrievePaymentFromRepository()
    {
        // Arrange
        var paymentId = Guid.NewGuid();
        var payment = new Domain.Entities.Payment(
            Guid.NewGuid(),
            PaymentType.MercadoPago,
            200.00m);

        var command = new ConfirmPaymentCommand(paymentId);

        _paymentRepositoryMock
            .Setup(x => x.GetByIdAsync(paymentId))
            .ReturnsAsync(payment);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _paymentRepositoryMock.Verify(x => x.GetByIdAsync(paymentId), Times.Once);
    }
}
