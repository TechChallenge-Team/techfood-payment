using TechFood.Payment.Application.Payments.Events;
using TechFood.Shared.Application.Events;

namespace TechFood.Payment.Application.Tests.Events;

public class ConfirmedPaymentEventTests
{
    [Fact(DisplayName = "ConfirmedPaymentEvent should be created with OrderId")]
    [Trait("Application", "ConfirmedPaymentEvent")]
    public void Constructor_WithOrderId_ShouldSetOrderIdProperty()
    {
        // Arrange
        var orderId = Guid.NewGuid();

        // Act
        var @event = new ConfirmedPaymentEvent(orderId);

        // Assert
        @event.Should().NotBeNull();
        @event.OrderId.Should().Be(orderId);
    }

    [Fact(DisplayName = "ConfirmedPaymentEvent should implement IIntegrationEvent")]
    [Trait("Application", "ConfirmedPaymentEvent")]
    public void ConfirmedPaymentEvent_ShouldImplementIIntegrationEvent()
    {
        // Arrange
        var orderId = Guid.NewGuid();

        // Act
        var @event = new ConfirmedPaymentEvent(orderId);

        // Assert
        @event.Should().BeAssignableTo<IIntegrationEvent>();
    }

    [Fact(DisplayName = "ConfirmedPaymentEvent OrderId should be settable")]
    [Trait("Application", "ConfirmedPaymentEvent")]
    public void OrderId_ShouldBeSettable()
    {
        // Arrange
        var initialOrderId = Guid.NewGuid();
        var newOrderId = Guid.NewGuid();
        var @event = new ConfirmedPaymentEvent(initialOrderId);

        // Act
        @event.OrderId = newOrderId;

        // Assert
        @event.OrderId.Should().Be(newOrderId);
        @event.OrderId.Should().NotBe(initialOrderId);
    }

    [Fact(DisplayName = "Multiple ConfirmedPaymentEvents should have different OrderIds")]
    [Trait("Application", "ConfirmedPaymentEvent")]
    public void MultipleEvents_ShouldHaveDifferentOrderIds()
    {
        // Arrange
        var orderId1 = Guid.NewGuid();
        var orderId2 = Guid.NewGuid();

        // Act
        var event1 = new ConfirmedPaymentEvent(orderId1);
        var event2 = new ConfirmedPaymentEvent(orderId2);

        // Assert
        event1.OrderId.Should().NotBe(event2.OrderId);
        event1.OrderId.Should().Be(orderId1);
        event2.OrderId.Should().Be(orderId2);
    }

    [Fact(DisplayName = "ConfirmedPaymentEvent should handle empty Guid")]
    [Trait("Application", "ConfirmedPaymentEvent")]
    public void Constructor_WithEmptyGuid_ShouldStillWork()
    {
        // Arrange
        var emptyGuid = Guid.Empty;

        // Act
        var @event = new ConfirmedPaymentEvent(emptyGuid);

        // Assert
        @event.OrderId.Should().Be(Guid.Empty);
    }
}
