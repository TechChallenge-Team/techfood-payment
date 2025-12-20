using TechFood.Payment.Domain.Events.Payment;
using TechFood.Shared.Domain.Enums;
using TechFood.Shared.Domain.Events;

namespace TechFood.Doman.Tests.Events;

public class PaymentCreatedEventTests
{
    [Fact(DisplayName = "PaymentCreatedEvent should be created with all required properties")]
    [Trait("Domain", "PaymentCreatedEvent")]
    public void Constructor_WithValidParameters_ShouldCreateEvent()
    {
        // Arrange
        var id = Guid.NewGuid();
        var orderId = Guid.NewGuid();
        var createdAt = DateTime.UtcNow;
        var paymentType = PaymentType.MercadoPago;
        var amount = 150.75m;

        // Act
        var @event = new PaymentCreatedEvent(id, orderId, createdAt, paymentType, amount);

        // Assert
        @event.Should().NotBeNull();
        @event.Id.Should().Be(id);
        @event.OrderId.Should().Be(orderId);
        @event.CreatedAt.Should().Be(createdAt);
        @event.PaymentType.Should().Be(paymentType);
        @event.Amount.Should().Be(amount);
    }

    [Fact(DisplayName = "PaymentCreatedEvent should implement IDomainEvent")]
    [Trait("Domain", "PaymentCreatedEvent")]
    public void PaymentCreatedEvent_ShouldImplementIDomainEvent()
    {
        // Arrange
        var @event = new PaymentCreatedEvent(
            Guid.NewGuid(),
            Guid.NewGuid(),
            DateTime.UtcNow,
            PaymentType.MercadoPago,
            100m);

        // Assert
        @event.Should().BeAssignableTo<IDomainEvent>();
    }

    [Fact(DisplayName = "PaymentCreatedEvent should be a record")]
    [Trait("Domain", "PaymentCreatedEvent")]
    public void PaymentCreatedEvent_ShouldBeRecord()
    {
        // Arrange
        var id = Guid.NewGuid();
        var orderId = Guid.NewGuid();
        var createdAt = DateTime.UtcNow;

        // Act
        var event1 = new PaymentCreatedEvent(id, orderId, createdAt, PaymentType.MercadoPago, 100m);
        var event2 = new PaymentCreatedEvent(id, orderId, createdAt, PaymentType.MercadoPago, 100m);
        var event3 = new PaymentCreatedEvent(Guid.NewGuid(), orderId, createdAt, PaymentType.MercadoPago, 100m);

        // Assert
        event1.Should().Be(event2);
        event1.Should().NotBe(event3);
    }

    [Fact(DisplayName = "PaymentCreatedEvent should handle zero amount")]
    [Trait("Domain", "PaymentCreatedEvent")]
    public void Constructor_WithZeroAmount_ShouldCreateEvent()
    {
        // Arrange & Act
        var @event = new PaymentCreatedEvent(
            Guid.NewGuid(),
            Guid.NewGuid(),
            DateTime.UtcNow,
            PaymentType.MercadoPago,
            0m);

        // Assert
        @event.Amount.Should().Be(0m);
    }

    [Fact(DisplayName = "PaymentCreatedEvent should handle different payment types")]
    [Trait("Domain", "PaymentCreatedEvent")]
    public void Constructor_WithDifferentPaymentTypes_ShouldCreateEvent()
    {
        // Arrange
        var id = Guid.NewGuid();
        var orderId = Guid.NewGuid();
        var createdAt = DateTime.UtcNow;
        var amount = 50m;

        // Act
        var mercadoPagoEvent = new PaymentCreatedEvent(id, orderId, createdAt, PaymentType.MercadoPago, amount);
        var creditCardEvent = new PaymentCreatedEvent(id, orderId, createdAt, PaymentType.CreditCard, amount);

        // Assert
        mercadoPagoEvent.PaymentType.Should().Be(PaymentType.MercadoPago);
        creditCardEvent.PaymentType.Should().Be(PaymentType.CreditCard);
    }

    [Fact(DisplayName = "PaymentCreatedEvent should preserve datetime precision")]
    [Trait("Domain", "PaymentCreatedEvent")]
    public void Constructor_ShouldPreserveDateTimePrecision()
    {
        // Arrange
        var createdAt = new DateTime(2025, 12, 20, 10, 30, 45, 123, DateTimeKind.Utc);

        // Act
        var @event = new PaymentCreatedEvent(
            Guid.NewGuid(),
            Guid.NewGuid(),
            createdAt,
            PaymentType.MercadoPago,
            100m);

        // Assert
        @event.CreatedAt.Should().Be(createdAt);
        @event.CreatedAt.Millisecond.Should().Be(123);
    }

    [Fact(DisplayName = "PaymentCreatedEvent should handle large amounts")]
    [Trait("Domain", "PaymentCreatedEvent")]
    public void Constructor_WithLargeAmount_ShouldCreateEvent()
    {
        // Arrange
        var largeAmount = 999999.99m;

        // Act
        var @event = new PaymentCreatedEvent(
            Guid.NewGuid(),
            Guid.NewGuid(),
            DateTime.UtcNow,
            PaymentType.MercadoPago,
            largeAmount);

        // Assert
        @event.Amount.Should().Be(largeAmount);
    }
}

public class PaymentConfirmedEventTests
{
    [Fact(DisplayName = "PaymentConfirmedEvent should be created with all required properties")]
    [Trait("Domain", "PaymentConfirmedEvent")]
    public void Constructor_WithValidParameters_ShouldCreateEvent()
    {
        // Arrange
        var id = Guid.NewGuid();
        var orderId = Guid.NewGuid();
        var paidAt = DateTime.UtcNow;
        var paymentType = PaymentType.MercadoPago;
        var amount = 200.50m;

        // Act
        var @event = new PaymentConfirmedEvent(id, orderId, paidAt, paymentType, amount);

        // Assert
        @event.Should().NotBeNull();
        @event.Id.Should().Be(id);
        @event.OrderId.Should().Be(orderId);
        @event.PaidAt.Should().Be(paidAt);
        @event.PaymentType.Should().Be(paymentType);
        @event.Amount.Should().Be(amount);
    }

    [Fact(DisplayName = "PaymentConfirmedEvent should implement IDomainEvent")]
    [Trait("Domain", "PaymentConfirmedEvent")]
    public void PaymentConfirmedEvent_ShouldImplementIDomainEvent()
    {
        // Arrange
        var @event = new PaymentConfirmedEvent(
            Guid.NewGuid(),
            Guid.NewGuid(),
            DateTime.UtcNow,
            PaymentType.MercadoPago,
            100m);

        // Assert
        @event.Should().BeAssignableTo<IDomainEvent>();
    }

    [Fact(DisplayName = "PaymentConfirmedEvent should be a record")]
    [Trait("Domain", "PaymentConfirmedEvent")]
    public void PaymentConfirmedEvent_ShouldBeRecord()
    {
        // Arrange
        var id = Guid.NewGuid();
        var orderId = Guid.NewGuid();
        var paidAt = DateTime.UtcNow;

        // Act
        var event1 = new PaymentConfirmedEvent(id, orderId, paidAt, PaymentType.MercadoPago, 100m);
        var event2 = new PaymentConfirmedEvent(id, orderId, paidAt, PaymentType.MercadoPago, 100m);
        var event3 = new PaymentConfirmedEvent(Guid.NewGuid(), orderId, paidAt, PaymentType.MercadoPago, 100m);

        // Assert
        event1.Should().Be(event2);
        event1.Should().NotBe(event3);
    }

    [Fact(DisplayName = "PaymentConfirmedEvent with different properties should not be equal")]
    [Trait("Domain", "PaymentConfirmedEvent")]
    public void PaymentConfirmedEvent_WithDifferentProperties_ShouldNotBeEqual()
    {
        // Arrange
        var id = Guid.NewGuid();
        var orderId = Guid.NewGuid();
        var paidAt = DateTime.UtcNow;

        // Act
        var event1 = new PaymentConfirmedEvent(id, orderId, paidAt, PaymentType.MercadoPago, 100m);
        var event2 = new PaymentConfirmedEvent(id, orderId, paidAt, PaymentType.MercadoPago, 200m);

        // Assert
        event1.Should().NotBe(event2);
    }

    [Fact(DisplayName = "PaymentConfirmedEvent should handle different payment types")]
    [Trait("Domain", "PaymentConfirmedEvent")]
    public void Constructor_WithDifferentPaymentTypes_ShouldCreateEvent()
    {
        // Arrange & Act
        var mercadoPagoEvent = new PaymentConfirmedEvent(
            Guid.NewGuid(), 
            Guid.NewGuid(), 
            DateTime.UtcNow, 
            PaymentType.MercadoPago, 
            50m);
        
        var creditCardEvent = new PaymentConfirmedEvent(
            Guid.NewGuid(), 
            Guid.NewGuid(), 
            DateTime.UtcNow, 
            PaymentType.CreditCard, 
            50m);

        // Assert
        mercadoPagoEvent.PaymentType.Should().Be(PaymentType.MercadoPago);
        creditCardEvent.PaymentType.Should().Be(PaymentType.CreditCard);
    }

    [Fact(DisplayName = "PaymentConfirmedEvent should preserve PaidAt datetime")]
    [Trait("Domain", "PaymentConfirmedEvent")]
    public void Constructor_ShouldPreservePaidAtDateTime()
    {
        // Arrange
        var paidAt = new DateTime(2025, 12, 20, 14, 30, 0, DateTimeKind.Utc);

        // Act
        var @event = new PaymentConfirmedEvent(
            Guid.NewGuid(),
            Guid.NewGuid(),
            paidAt,
            PaymentType.MercadoPago,
            100m);

        // Assert
        @event.PaidAt.Should().Be(paidAt);
    }
}

public class PaymentRefusedEventTests
{
    [Fact(DisplayName = "PaymentRefusedEvent should be created with all required properties")]
    [Trait("Domain", "PaymentRefusedEvent")]
    public void Constructor_WithValidParameters_ShouldCreateEvent()
    {
        // Arrange
        var id = Guid.NewGuid();
        var orderId = Guid.NewGuid();
        var refusedAt = DateTime.UtcNow;
        var type = PaymentType.MercadoPago;
        var amount = 99.99m;

        // Act
        var @event = new PaymentRefusedEvent(id, orderId, refusedAt, type, amount);

        // Assert
        @event.Should().NotBeNull();
        @event.Id.Should().Be(id);
        @event.OrderId.Should().Be(orderId);
        @event.RefusedAt.Should().Be(refusedAt);
        @event.Type.Should().Be(type);
        @event.Amount.Should().Be(amount);
    }

    [Fact(DisplayName = "PaymentRefusedEvent should implement IDomainEvent")]
    [Trait("Domain", "PaymentRefusedEvent")]
    public void PaymentRefusedEvent_ShouldImplementIDomainEvent()
    {
        // Arrange
        var @event = new PaymentRefusedEvent(
            Guid.NewGuid(),
            Guid.NewGuid(),
            DateTime.UtcNow,
            PaymentType.MercadoPago,
            100m);

        // Assert
        @event.Should().BeAssignableTo<IDomainEvent>();
    }

    [Fact(DisplayName = "PaymentRefusedEvent should be a record")]
    [Trait("Domain", "PaymentRefusedEvent")]
    public void PaymentRefusedEvent_ShouldBeRecord()
    {
        // Arrange
        var id = Guid.NewGuid();
        var orderId = Guid.NewGuid();
        var refusedAt = DateTime.UtcNow;

        // Act
        var event1 = new PaymentRefusedEvent(id, orderId, refusedAt, PaymentType.MercadoPago, 100m);
        var event2 = new PaymentRefusedEvent(id, orderId, refusedAt, PaymentType.MercadoPago, 100m);
        var event3 = new PaymentRefusedEvent(Guid.NewGuid(), orderId, refusedAt, PaymentType.MercadoPago, 100m);

        // Assert
        event1.Should().Be(event2);
        event1.Should().NotBe(event3);
    }

    [Fact(DisplayName = "PaymentRefusedEvent with different amounts should not be equal")]
    [Trait("Domain", "PaymentRefusedEvent")]
    public void PaymentRefusedEvent_WithDifferentAmounts_ShouldNotBeEqual()
    {
        // Arrange
        var id = Guid.NewGuid();
        var orderId = Guid.NewGuid();
        var refusedAt = DateTime.UtcNow;

        // Act
        var event1 = new PaymentRefusedEvent(id, orderId, refusedAt, PaymentType.MercadoPago, 100m);
        var event2 = new PaymentRefusedEvent(id, orderId, refusedAt, PaymentType.MercadoPago, 200m);

        // Assert
        event1.Should().NotBe(event2);
    }

    [Fact(DisplayName = "PaymentRefusedEvent should handle different payment types")]
    [Trait("Domain", "PaymentRefusedEvent")]
    public void Constructor_WithDifferentPaymentTypes_ShouldCreateEvent()
    {
        // Arrange & Act
        var mercadoPagoEvent = new PaymentRefusedEvent(
            Guid.NewGuid(),
            Guid.NewGuid(),
            DateTime.UtcNow,
            PaymentType.MercadoPago,
            75m);

        var creditCardEvent = new PaymentRefusedEvent(
            Guid.NewGuid(),
            Guid.NewGuid(),
            DateTime.UtcNow,
            PaymentType.CreditCard,
            75m);

        // Assert
        mercadoPagoEvent.Type.Should().Be(PaymentType.MercadoPago);
        creditCardEvent.Type.Should().Be(PaymentType.CreditCard);
    }

    [Fact(DisplayName = "PaymentRefusedEvent should preserve RefusedAt datetime")]
    [Trait("Domain", "PaymentRefusedEvent")]
    public void Constructor_ShouldPreserveRefusedAtDateTime()
    {
        // Arrange
        var refusedAt = new DateTime(2025, 12, 20, 16, 45, 30, DateTimeKind.Utc);

        // Act
        var @event = new PaymentRefusedEvent(
            Guid.NewGuid(),
            Guid.NewGuid(),
            refusedAt,
            PaymentType.MercadoPago,
            100m);

        // Assert
        @event.RefusedAt.Should().Be(refusedAt);
    }

    [Fact(DisplayName = "PaymentRefusedEvent should handle zero amount")]
    [Trait("Domain", "PaymentRefusedEvent")]
    public void Constructor_WithZeroAmount_ShouldCreateEvent()
    {
        // Arrange & Act
        var @event = new PaymentRefusedEvent(
            Guid.NewGuid(),
            Guid.NewGuid(),
            DateTime.UtcNow,
            PaymentType.MercadoPago,
            0m);

        // Assert
        @event.Amount.Should().Be(0m);
    }

    [Fact(DisplayName = "PaymentRefusedEvent with different OrderIds should not be equal")]
    [Trait("Domain", "PaymentRefusedEvent")]
    public void PaymentRefusedEvent_WithDifferentOrderIds_ShouldNotBeEqual()
    {
        // Arrange
        var id = Guid.NewGuid();
        var refusedAt = DateTime.UtcNow;

        // Act
        var event1 = new PaymentRefusedEvent(id, Guid.NewGuid(), refusedAt, PaymentType.MercadoPago, 100m);
        var event2 = new PaymentRefusedEvent(id, Guid.NewGuid(), refusedAt, PaymentType.MercadoPago, 100m);

        // Assert
        event1.Should().NotBe(event2);
    }
}
