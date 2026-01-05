using TechFood.Payment.Application.Payments.Commands.ConfirmPayment;
using TechFood.Payment.Application.Payments.Commands.CreatePayment;
using TechFood.Payment.Application.Payments.Events;
using TechFood.Shared.Domain.Enums;

namespace TechFood.Payment.Application.Tests.Commands;

public class CommandsAndEventsTests
{
    [Fact(DisplayName = "CreatePaymentCommand should be created correctly")]
    [Trait("Application", "Commands")]
    public void CreatePaymentCommand_ShouldBeCreatedCorrectly()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var paymentType = PaymentType.MercadoPago;

        // Act
        var command = new CreatePaymentCommand(orderId, paymentType);

        // Assert
        command.OrderId.Should().Be(orderId);
        command.Type.Should().Be(paymentType);
    }

    [Fact(DisplayName = "ConfirmPaymentCommand should be created correctly")]
    [Trait("Application", "Commands")]
    public void ConfirmPaymentCommand_ShouldBeCreatedCorrectly()
    {
        // Arrange
        var paymentId = Guid.NewGuid();

        // Act
        var command = new ConfirmPaymentCommand(paymentId);

        // Assert
        command.Id.Should().Be(paymentId);
    }

    [Fact(DisplayName = "ConfirmedPaymentEvent should be created")]
    [Trait("Application", "Events")]
    public void ConfirmedPaymentEvent_ShouldBeCreated()
    {
        // Act
        var @event = new PaymentConfirmedEvent();

        // Assert
        @event.Should().NotBeNull();
    }

    [Fact(DisplayName = "CreatePaymentCommand with different payment types")]
    [Trait("Application", "Commands")]
    public void CreatePaymentCommand_WithDifferentPaymentTypes_ShouldBeValid()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var paymentTypes = new[] { PaymentType.MercadoPago, PaymentType.CreditCard };

        foreach (var type in paymentTypes)
        {
            // Act
            var command = new CreatePaymentCommand(orderId, type);

            // Assert
            command.Type.Should().Be(type);
            command.OrderId.Should().Be(orderId);
        }
    }
}
