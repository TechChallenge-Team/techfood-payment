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

    [Fact(DisplayName = "ConfirmedPaymentEvent should be serializable for event publishing")]
    [Trait("Application", "ConfirmedPaymentEvent")]
    public void ConfirmedPaymentEvent_ShouldBeSerializable()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var @event = new ConfirmedPaymentEvent(orderId);

        // Act
        var json = System.Text.Json.JsonSerializer.Serialize(@event);
        var deserializedEvent = System.Text.Json.JsonSerializer.Deserialize<ConfirmedPaymentEvent>(json);

        // Assert
        deserializedEvent.Should().NotBeNull();
        deserializedEvent!.OrderId.Should().Be(orderId);
    }

    [Fact(DisplayName = "ConfirmedPaymentEvent with same OrderId should be equal")]
    [Trait("Application", "ConfirmedPaymentEvent")]
    public void TwoEvents_WithSameOrderId_ShouldHaveSameOrderId()
    {
        // Arrange
        var orderId = Guid.NewGuid();

        // Act
        var event1 = new ConfirmedPaymentEvent(orderId);
        var event2 = new ConfirmedPaymentEvent(orderId);

        // Assert
        event1.OrderId.Should().Be(event2.OrderId);
    }

    [Fact(DisplayName = "ConfirmedPaymentEvent should allow OrderId to be reset")]
    [Trait("Application", "ConfirmedPaymentEvent")]
    public void OrderId_CanBeResetToEmpty()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var @event = new ConfirmedPaymentEvent(orderId);

        // Act
        @event.OrderId = Guid.Empty;

        // Assert
        @event.OrderId.Should().Be(Guid.Empty);
        @event.OrderId.Should().NotBe(orderId);
    }

    [Fact(DisplayName = "ConfirmedPaymentEvent should maintain state after multiple assignments")]
    [Trait("Application", "ConfirmedPaymentEvent")]
    public void OrderId_MultipleAssignments_ShouldMaintainLastValue()
    {
        // Arrange
        var orderId1 = Guid.NewGuid();
        var orderId2 = Guid.NewGuid();
        var orderId3 = Guid.NewGuid();
        var @event = new ConfirmedPaymentEvent(orderId1);

        // Act
        @event.OrderId = orderId2;
        @event.OrderId = orderId3;

        // Assert
        @event.OrderId.Should().Be(orderId3);
        @event.OrderId.Should().NotBe(orderId1);
        @event.OrderId.Should().NotBe(orderId2);
    }

    [Theory(DisplayName = "ConfirmedPaymentEvent should work with different Guid formats")]
    [Trait("Application", "ConfirmedPaymentEvent")]
    [InlineData("00000000-0000-0000-0000-000000000000")]
    [InlineData("11111111-1111-1111-1111-111111111111")]
    [InlineData("ffffffff-ffff-ffff-ffff-ffffffffffff")]
    public void Constructor_WithVariousGuidFormats_ShouldWork(string guidString)
    {
        // Arrange
        var guid = Guid.Parse(guidString);

        // Act
        var @event = new ConfirmedPaymentEvent(guid);

        // Assert
        @event.OrderId.Should().Be(guid);
    }
}
