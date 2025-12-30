//using TechFood.Payment.Application.Payments.Events;
//using TechFood.Shared.Application.Events;

//namespace TechFood.Payment.Application.Tests.Events;

//public class OrderCreatedIntegrationEventTests
//{
//    [Fact(DisplayName = "OrderCreatedIntegrationEvent should be created with OrderId and Items")]
//    [Trait("Application", "OrderCreatedIntegrationEvent")]
//    public void Constructor_WithOrderIdAndItems_ShouldSetProperties()
//    {
//        // Arrange
//        var orderId = Guid.NewGuid();
//        var items = new List<OrderItemCreatedDto>
//        {
//            new(Guid.NewGuid(), "Product 1", 10.50m, 2),
//            new(Guid.NewGuid(), "Product 2", 25.00m, 1)
//        };

//        // Act
//        var @event = new OrderCreatedIntegrationEvent(orderId, items);

//        // Assert
//        @event.Should().NotBeNull();
//        @event.OrderId.Should().Be(orderId);
//        @event.Items.Should().BeEquivalentTo(items);
//        @event.Items.Should().HaveCount(2);
//    }

//    [Fact(DisplayName = "OrderCreatedIntegrationEvent should implement IIntegrationEvent")]
//    [Trait("Application", "OrderCreatedIntegrationEvent")]
//    public void OrderCreatedIntegrationEvent_ShouldImplementIIntegrationEvent()
//    {
//        // Arrange
//        var orderId = Guid.NewGuid();
//        var items = new List<OrderItemCreatedDto>();

//        // Act
//        var @event = new OrderCreatedIntegrationEvent(orderId, items);

//        // Assert
//        @event.Should().BeAssignableTo<IIntegrationEvent>();
//    }

//    [Fact(DisplayName = "OrderCreatedIntegrationEvent should handle empty items list")]
//    [Trait("Application", "OrderCreatedIntegrationEvent")]
//    public void Constructor_WithEmptyItemsList_ShouldWork()
//    {
//        // Arrange
//        var orderId = Guid.NewGuid();
//        var items = new List<OrderItemCreatedDto>();

//        // Act
//        var @event = new OrderCreatedIntegrationEvent(orderId, items);

//        // Assert
//        @event.OrderId.Should().Be(orderId);
//        @event.Items.Should().NotBeNull();
//        @event.Items.Should().BeEmpty();
//    }

//    [Fact(DisplayName = "OrderCreatedIntegrationEvent should preserve all item properties")]
//    [Trait("Application", "OrderCreatedIntegrationEvent")]
//    public void Constructor_ShouldPreserveAllItemProperties()
//    {
//        // Arrange
//        var orderId = Guid.NewGuid();
//        var productId = Guid.NewGuid();
//        var productName = "Premium Burger";
//        var unitPrice = 45.90m;
//        var quantity = 3;

//        var items = new List<OrderItemCreatedDto>
//        {
//            new(productId, productName, unitPrice, quantity)
//        };

//        // Act
//        var @event = new OrderCreatedIntegrationEvent(orderId, items);

//        // Assert
//        var item = @event.Items.First();
//        item.ProductId.Should().Be(productId);
//        item.Name.Should().Be(productName);
//        item.UnitPrice.Should().Be(unitPrice);
//        item.Quantity.Should().Be(quantity);
//    }

//    [Fact(DisplayName = "OrderCreatedIntegrationEvent should handle multiple items correctly")]
//    [Trait("Application", "OrderCreatedIntegrationEvent")]
//    public void Constructor_WithMultipleItems_ShouldPreserveOrder()
//    {
//        // Arrange
//        var orderId = Guid.NewGuid();
//        var items = new List<OrderItemCreatedDto>
//        {
//            new(Guid.NewGuid(), "Item 1", 10.00m, 1),
//            new(Guid.NewGuid(), "Item 2", 20.00m, 2),
//            new(Guid.NewGuid(), "Item 3", 30.00m, 3)
//        };

//        // Act
//        var @event = new OrderCreatedIntegrationEvent(orderId, items);

//        // Assert
//        @event.Items.Should().HaveCount(3);
//        @event.Items[0].Name.Should().Be("Item 1");
//        @event.Items[1].Name.Should().Be("Item 2");
//        @event.Items[2].Name.Should().Be("Item 3");
//    }

//    [Fact(DisplayName = "OrderCreatedIntegrationEvent should be serializable")]
//    [Trait("Application", "OrderCreatedIntegrationEvent")]
//    public void OrderCreatedIntegrationEvent_ShouldBeSerializable()
//    {
//        // Arrange
//        var orderId = Guid.NewGuid();
//        var items = new List<OrderItemCreatedDto>
//        {
//            new(Guid.NewGuid(), "Burger", 25.50m, 2)
//        };
//        var @event = new OrderCreatedIntegrationEvent(orderId, items);

//        // Act
//        var json = System.Text.Json.JsonSerializer.Serialize(@event);
//        var deserializedEvent = System.Text.Json.JsonSerializer.Deserialize<OrderCreatedIntegrationEvent>(json);

//        // Assert
//        deserializedEvent.Should().NotBeNull();
//        deserializedEvent!.OrderId.Should().Be(orderId);
//        deserializedEvent.Items.Should().HaveCount(1);
//    }

//    [Fact(DisplayName = "OrderCreatedIntegrationEvent with null items should throw")]
//    [Trait("Application", "OrderCreatedIntegrationEvent")]
//    public void Constructor_WithNullItems_ShouldHandleGracefully()
//    {
//        // Arrange
//        var orderId = Guid.NewGuid();

//        // Act & Assert - depending on implementation, might accept null or throw
//        // If it accepts null, the property should handle it
//        var @event = new OrderCreatedIntegrationEvent(orderId, null!);
        
//        // Verify behavior (null or empty)
//        @event.OrderId.Should().Be(orderId);
//    }

//    [Fact(DisplayName = "OrderCreatedIntegrationEvent should handle large number of items")]
//    [Trait("Application", "OrderCreatedIntegrationEvent")]
//    public void Constructor_WithManyItems_ShouldWork()
//    {
//        // Arrange
//        var orderId = Guid.NewGuid();
//        var items = Enumerable.Range(1, 100)
//            .Select(i => new OrderItemCreatedDto(Guid.NewGuid(), $"Item {i}", i * 10.0m, i))
//            .ToList();

//        // Act
//        var @event = new OrderCreatedIntegrationEvent(orderId, items);

//        // Assert
//        @event.Items.Should().HaveCount(100);
//        @event.Items.Should().BeEquivalentTo(items);
//    }

//    [Fact(DisplayName = "OrderCreatedIntegrationEvent should preserve item order")]
//    [Trait("Application", "OrderCreatedIntegrationEvent")]
//    public void Constructor_ShouldPreserveItemOrder()
//    {
//        // Arrange
//        var orderId = Guid.NewGuid();
//        var items = new List<OrderItemCreatedDto>
//        {
//            new(Guid.NewGuid(), "First", 10m, 1),
//            new(Guid.NewGuid(), "Second", 20m, 2),
//            new(Guid.NewGuid(), "Third", 30m, 3)
//        };

//        // Act
//        var @event = new OrderCreatedIntegrationEvent(orderId, items);

//        // Assert
//        @event.Items[0].Name.Should().Be("First");
//        @event.Items[1].Name.Should().Be("Second");
//        @event.Items[2].Name.Should().Be("Third");
//    }

//    [Fact(DisplayName = "OrderCreatedIntegrationEvent with items having same ProductId")]
//    [Trait("Application", "OrderCreatedIntegrationEvent")]
//    public void Constructor_WithDuplicateProductIds_ShouldAllowIt()
//    {
//        // Arrange
//        var orderId = Guid.NewGuid();
//        var sameProductId = Guid.NewGuid();
//        var items = new List<OrderItemCreatedDto>
//        {
//            new(sameProductId, "Product A", 10m, 1),
//            new(sameProductId, "Product A", 10m, 2)
//        };

//        // Act
//        var @event = new OrderCreatedIntegrationEvent(orderId, items);

//        // Assert
//        @event.Items.Should().HaveCount(2);
//        @event.Items.All(i => i.ProductId == sameProductId).Should().BeTrue();
//    }

//    [Fact(DisplayName = "OrderCreatedIntegrationEvent should handle items with zero prices")]
//    [Trait("Application", "OrderCreatedIntegrationEvent")]
//    public void Constructor_WithZeroPriceItems_ShouldWork()
//    {
//        // Arrange
//        var orderId = Guid.NewGuid();
//        var items = new List<OrderItemCreatedDto>
//        {
//            new(Guid.NewGuid(), "Free Item", 0m, 1)
//        };

//        // Act
//        var @event = new OrderCreatedIntegrationEvent(orderId, items);

//        // Assert
//        @event.Items[0].UnitPrice.Should().Be(0m);
//    }

//    [Fact(DisplayName = "OrderCreatedIntegrationEvent should handle items with negative quantities")]
//    [Trait("Application", "OrderCreatedIntegrationEvent")]
//    public void Constructor_WithNegativeQuantities_ShouldAcceptIt()
//    {
//        // Arrange
//        var orderId = Guid.NewGuid();
//        var items = new List<OrderItemCreatedDto>
//        {
//            new(Guid.NewGuid(), "Return Item", 10m, -1)
//        };

//        // Act
//        var @event = new OrderCreatedIntegrationEvent(orderId, items);

//        // Assert
//        @event.Items[0].Quantity.Should().Be(-1);
//    }

//    [Fact(DisplayName = "OrderCreatedIntegrationEvent with empty OrderId")]
//    [Trait("Application", "OrderCreatedIntegrationEvent")]
//    public void Constructor_WithEmptyOrderId_ShouldWork()
//    {
//        // Arrange
//        var orderId = Guid.Empty;
//        var items = new List<OrderItemCreatedDto>();

//        // Act
//        var @event = new OrderCreatedIntegrationEvent(orderId, items);

//        // Assert
//        @event.OrderId.Should().Be(Guid.Empty);
//    }
//}
