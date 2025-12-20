using TechFood.Payment.Application.Payments.Events;
using TechFood.Shared.Application.Events;

namespace TechFood.Payment.Application.Tests.Events;

public class OrderCreatedIntegrationEventTests
{
    [Fact(DisplayName = "OrderCreatedIntegrationEvent should be created with OrderId and Items")]
    [Trait("Application", "OrderCreatedIntegrationEvent")]
    public void Constructor_WithOrderIdAndItems_ShouldSetProperties()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var items = new List<OrderItemCreatedDto>
        {
            new(Guid.NewGuid(), "Product 1", 10.50m, 2),
            new(Guid.NewGuid(), "Product 2", 25.00m, 1)
        };

        // Act
        var @event = new OrderCreatedIntegrationEvent(orderId, items);

        // Assert
        @event.Should().NotBeNull();
        @event.OrderId.Should().Be(orderId);
        @event.Items.Should().BeEquivalentTo(items);
        @event.Items.Should().HaveCount(2);
    }

    [Fact(DisplayName = "OrderCreatedIntegrationEvent should implement IIntegrationEvent")]
    [Trait("Application", "OrderCreatedIntegrationEvent")]
    public void OrderCreatedIntegrationEvent_ShouldImplementIIntegrationEvent()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var items = new List<OrderItemCreatedDto>();

        // Act
        var @event = new OrderCreatedIntegrationEvent(orderId, items);

        // Assert
        @event.Should().BeAssignableTo<IIntegrationEvent>();
    }

    [Fact(DisplayName = "OrderCreatedIntegrationEvent should handle empty items list")]
    [Trait("Application", "OrderCreatedIntegrationEvent")]
    public void Constructor_WithEmptyItemsList_ShouldWork()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var items = new List<OrderItemCreatedDto>();

        // Act
        var @event = new OrderCreatedIntegrationEvent(orderId, items);

        // Assert
        @event.OrderId.Should().Be(orderId);
        @event.Items.Should().NotBeNull();
        @event.Items.Should().BeEmpty();
    }

    [Fact(DisplayName = "OrderCreatedIntegrationEvent should preserve all item properties")]
    [Trait("Application", "OrderCreatedIntegrationEvent")]
    public void Constructor_ShouldPreserveAllItemProperties()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var productName = "Premium Burger";
        var unitPrice = 45.90m;
        var quantity = 3;

        var items = new List<OrderItemCreatedDto>
        {
            new(productId, productName, unitPrice, quantity)
        };

        // Act
        var @event = new OrderCreatedIntegrationEvent(orderId, items);

        // Assert
        var item = @event.Items.First();
        item.ProductId.Should().Be(productId);
        item.Name.Should().Be(productName);
        item.UnitPrice.Should().Be(unitPrice);
        item.Quantity.Should().Be(quantity);
    }

    [Fact(DisplayName = "OrderCreatedIntegrationEvent should handle multiple items correctly")]
    [Trait("Application", "OrderCreatedIntegrationEvent")]
    public void Constructor_WithMultipleItems_ShouldPreserveOrder()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var items = new List<OrderItemCreatedDto>
        {
            new(Guid.NewGuid(), "Item 1", 10.00m, 1),
            new(Guid.NewGuid(), "Item 2", 20.00m, 2),
            new(Guid.NewGuid(), "Item 3", 30.00m, 3)
        };

        // Act
        var @event = new OrderCreatedIntegrationEvent(orderId, items);

        // Assert
        @event.Items.Should().HaveCount(3);
        @event.Items[0].Name.Should().Be("Item 1");
        @event.Items[1].Name.Should().Be("Item 2");
        @event.Items[2].Name.Should().Be("Item 3");
    }
}
