using TechFood.Payment.Application.Payments.Events;

namespace TechFood.Payment.Application.Tests.Events;

public class OrderItemCreatedDtoTests
{
    [Fact(DisplayName = "OrderItemCreatedDto should be created with all properties")]
    [Trait("Application", "OrderItemCreatedDto")]
    public void Constructor_WithAllProperties_ShouldSetThemCorrectly()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var name = "Cheeseburger";
        var unitPrice = 29.90m;
        var quantity = 2;

        // Act
        var dto = new OrderItemCreatedDto(productId, name, unitPrice, quantity);

        // Assert
        dto.Should().NotBeNull();
        dto.ProductId.Should().Be(productId);
        dto.Name.Should().Be(name);
        dto.UnitPrice.Should().Be(unitPrice);
        dto.Quantity.Should().Be(quantity);
    }

    [Fact(DisplayName = "OrderItemCreatedDto should handle zero quantity")]
    [Trait("Application", "OrderItemCreatedDto")]
    public void Constructor_WithZeroQuantity_ShouldAcceptIt()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var name = "Product";
        var unitPrice = 10.00m;
        var quantity = 0;

        // Act
        var dto = new OrderItemCreatedDto(productId, name, unitPrice, quantity);

        // Assert
        dto.Quantity.Should().Be(0);
    }

    [Fact(DisplayName = "OrderItemCreatedDto should handle zero price")]
    [Trait("Application", "OrderItemCreatedDto")]
    public void Constructor_WithZeroPrice_ShouldAcceptIt()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var name = "Free Item";
        var unitPrice = 0m;
        var quantity = 1;

        // Act
        var dto = new OrderItemCreatedDto(productId, name, unitPrice, quantity);

        // Assert
        dto.UnitPrice.Should().Be(0m);
    }

    [Fact(DisplayName = "OrderItemCreatedDto should handle large prices")]
    [Trait("Application", "OrderItemCreatedDto")]
    public void Constructor_WithLargePrice_ShouldWork()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var name = "Expensive Product";
        var unitPrice = 999999.99m;
        var quantity = 1;

        // Act
        var dto = new OrderItemCreatedDto(productId, name, unitPrice, quantity);

        // Assert
        dto.UnitPrice.Should().Be(999999.99m);
    }

    [Fact(DisplayName = "OrderItemCreatedDto should handle large quantities")]
    [Trait("Application", "OrderItemCreatedDto")]
    public void Constructor_WithLargeQuantity_ShouldWork()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var name = "Bulk Product";
        var unitPrice = 5.00m;
        var quantity = 1000;

        // Act
        var dto = new OrderItemCreatedDto(productId, name, unitPrice, quantity);

        // Assert
        dto.Quantity.Should().Be(1000);
    }

    [Fact(DisplayName = "OrderItemCreatedDto should preserve decimal precision")]
    [Trait("Application", "OrderItemCreatedDto")]
    public void Constructor_ShouldPreserveDecimalPrecision()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var name = "Precise Product";
        var unitPrice = 12.345m;
        var quantity = 3;

        // Act
        var dto = new OrderItemCreatedDto(productId, name, unitPrice, quantity);

        // Assert
        dto.UnitPrice.Should().Be(12.345m);
    }

    [Fact(DisplayName = "OrderItemCreatedDto should handle empty name")]
    [Trait("Application", "OrderItemCreatedDto")]
    public void Constructor_WithEmptyName_ShouldAcceptIt()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var name = "";
        var unitPrice = 10.00m;
        var quantity = 1;

        // Act
        var dto = new OrderItemCreatedDto(productId, name, unitPrice, quantity);

        // Assert
        dto.Name.Should().Be("");
    }

    [Fact(DisplayName = "OrderItemCreatedDto should handle special characters in name")]
    [Trait("Application", "OrderItemCreatedDto")]
    public void Constructor_WithSpecialCharactersInName_ShouldPreserveThem()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var name = "Product™ with Special®️ Chars & Symbols!";
        var unitPrice = 15.50m;
        var quantity = 1;

        // Act
        var dto = new OrderItemCreatedDto(productId, name, unitPrice, quantity);

        // Assert
        dto.Name.Should().Be(name);
    }

    [Theory(DisplayName = "OrderItemCreatedDto should work with various valid scenarios")]
    [Trait("Application", "OrderItemCreatedDto")]
    [InlineData("Burger", 25.50, 2)]
    [InlineData("Pizza", 35.00, 1)]
    [InlineData("Soda", 5.00, 3)]
    [InlineData("Fries", 8.50, 4)]
    public void Constructor_WithVariousScenarios_ShouldWorkCorrectly(string name, decimal price, int quantity)
    {
        // Arrange
        var productId = Guid.NewGuid();

        // Act
        var dto = new OrderItemCreatedDto(productId, name, price, quantity);

        // Assert
        dto.ProductId.Should().NotBe(Guid.Empty);
        dto.Name.Should().Be(name);
        dto.UnitPrice.Should().Be(price);
        dto.Quantity.Should().Be(quantity);
    }
}
