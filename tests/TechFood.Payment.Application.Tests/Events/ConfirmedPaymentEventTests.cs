using TechFood.Payment.Application.Payments.Events.Integration.Outgoing;
using TechFood.Shared.Application.Events;

namespace TechFood.Payment.Application.Tests.Events;

public class ConfirmedPaymentEventTests
{
    [Fact(DisplayName = "PaymentConfirmedEvent should be created with PaymentId and OrderId")]
    [Trait("Application", "PaymentConfirmedEvent")]
    public void Constructor_WithPaymentIdAndOrderId_ShouldSetProperties()
    {
        // Arrange
        var paymentId = Guid.NewGuid();
        var orderId = Guid.NewGuid();

        // Act
        var @event = new PaymentConfirmedEvent(paymentId, orderId);

        // Assert
        @event.Should().NotBeNull();
        @event.PaymentId.Should().Be(paymentId);
        @event.OrderId.Should().Be(orderId);
    }

    [Fact(DisplayName = "PaymentConfirmedEvent should implement IIntegrationEvent")]
    [Trait("Application", "PaymentConfirmedEvent")]
    public void PaymentConfirmedEvent_ShouldImplementIIntegrationEvent()
    {
        // Arrange
        var paymentId = Guid.NewGuid();
        var orderId = Guid.NewGuid();

        // Act
        var @event = new PaymentConfirmedEvent(paymentId, orderId);

        // Assert
        @event.Should().BeAssignableTo<IIntegrationEvent>();
    }

    [Fact(DisplayName = "PaymentConfirmedEvent OrderId should be settable")]
    [Trait("Application", "PaymentConfirmedEvent")]
    public void OrderId_ShouldBeSettable()
    {
        // Arrange
        var paymentId = Guid.NewGuid();
        var initialOrderId = Guid.NewGuid();
        var newOrderId = Guid.NewGuid();
        var @event = new PaymentConfirmedEvent(paymentId, initialOrderId);

        // Act
        @event.OrderId = newOrderId;

        // Assert
        @event.OrderId.Should().Be(newOrderId);
        @event.OrderId.Should().NotBe(initialOrderId);
    }

    [Fact(DisplayName = "Multiple PaymentConfirmedEvents should have different OrderIds")]
    [Trait("Application", "PaymentConfirmedEvent")]
    public void MultipleEvents_ShouldHaveDifferentOrderIds()
    {
        // Arrange
        var paymentId1 = Guid.NewGuid();
        var paymentId2 = Guid.NewGuid();
        var orderId1 = Guid.NewGuid();
        var orderId2 = Guid.NewGuid();

        // Act
        var event1 = new PaymentConfirmedEvent(paymentId1, orderId1);
        var event2 = new PaymentConfirmedEvent(paymentId2, orderId2);

        // Assert
        event1.OrderId.Should().NotBe(event2.OrderId);
        event1.OrderId.Should().Be(orderId1);
        event2.OrderId.Should().Be(orderId2);
    }

    [Fact(DisplayName = "PaymentConfirmedEvent should handle empty Guid")]
    [Trait("Application", "PaymentConfirmedEvent")]
    public void Constructor_WithEmptyGuid_ShouldStillWork()
    {
        // Arrange
        var emptyGuid = Guid.Empty;

        // Act
        var @event = new PaymentConfirmedEvent(emptyGuid, emptyGuid);

        // Assert
        @event.PaymentId.Should().Be(Guid.Empty);
        @event.OrderId.Should().Be(Guid.Empty);
    }

    [Fact(DisplayName = "PaymentConfirmedEvent should be serializable for event publishing")]
    [Trait("Application", "PaymentConfirmedEvent")]
    public void PaymentConfirmedEvent_ShouldBeSerializable()
    {
        // Arrange
        var paymentId = Guid.NewGuid();
        var orderId = Guid.NewGuid();
        var @event = new PaymentConfirmedEvent(paymentId, orderId);

        // Act
        var json = System.Text.Json.JsonSerializer.Serialize(@event);
        var deserializedEvent = System.Text.Json.JsonSerializer.Deserialize<PaymentConfirmedEvent>(json);

        // Assert
        deserializedEvent.Should().NotBeNull();
        deserializedEvent!.PaymentId.Should().Be(paymentId);
        deserializedEvent!.OrderId.Should().Be(orderId);
    }

    [Fact(DisplayName = "PaymentConfirmedEvent with same OrderId should be equal")]
    [Trait("Application", "PaymentConfirmedEvent")]
    public void TwoEvents_WithSameOrderId_ShouldHaveSameOrderId()
    {
        // Arrange
        var paymentId = Guid.NewGuid();
        var orderId = Guid.NewGuid();

        // Act
        var event1 = new PaymentConfirmedEvent(paymentId, orderId);
        var event2 = new PaymentConfirmedEvent(paymentId, orderId);

        // Assert
        event1.OrderId.Should().Be(event2.OrderId);
    }

    [Fact(DisplayName = "PaymentConfirmedEvent should allow OrderId to be reset")]
    [Trait("Application", "PaymentConfirmedEvent")]
    public void OrderId_CanBeResetToEmpty()
    {
        // Arrange
        var paymentId = Guid.NewGuid();
        var orderId = Guid.NewGuid();
        var @event = new PaymentConfirmedEvent(paymentId, orderId);

        // Act
        @event.OrderId = Guid.Empty;

        // Assert
        @event.OrderId.Should().Be(Guid.Empty);
        @event.OrderId.Should().NotBe(orderId);
    }

    [Fact(DisplayName = "PaymentConfirmedEvent should maintain state after multiple assignments")]
    [Trait("Application", "PaymentConfirmedEvent")]
    public void OrderId_MultipleAssignments_ShouldMaintainLastValue()
    {
        // Arrange
        var paymentId = Guid.NewGuid();
        var orderId1 = Guid.NewGuid();
        var orderId2 = Guid.NewGuid();
        var orderId3 = Guid.NewGuid();
        var @event = new PaymentConfirmedEvent(paymentId, orderId1);

        // Act
        @event.OrderId = orderId2;
        @event.OrderId = orderId3;

        // Assert
        @event.OrderId.Should().Be(orderId3);
        @event.OrderId.Should().NotBe(orderId1);
        @event.OrderId.Should().NotBe(orderId2);
    }

    [Theory(DisplayName = "PaymentConfirmedEvent should work with different Guid formats")]
    [Trait("Application", "PaymentConfirmedEvent")]
    [InlineData("00000000-0000-0000-0000-000000000000")]
    [InlineData("11111111-1111-1111-1111-111111111111")]
    [InlineData("ffffffff-ffff-ffff-ffff-ffffffffffff")]
    public void Constructor_WithVariousGuidFormats_ShouldWork(string guidString)
    {
        // Arrange
        var guid = Guid.Parse(guidString);

        // Act
        var @event = new PaymentConfirmedEvent(guid, guid);

        // Assert
        @event.PaymentId.Should().Be(guid);
        @event.OrderId.Should().Be(guid);
    }
}
