//using TechFood.Payment.Application.Payments.Events;

//namespace TechFood.Payment.Application.Tests.Events;

//public class OrderItemCreatedDtoTests
//{
//    [Fact(DisplayName = "OrderItemCreatedDto should be created with all properties")]
//    [Trait("Application", "OrderItemCreatedDto")]
//    public void Constructor_WithAllProperties_ShouldSetThemCorrectly()
//    {
//        // Arrange
//        var productId = Guid.NewGuid();
//        var name = "Cheeseburger";
//        var unitPrice = 29.90m;
//        var quantity = 2;

//        // Act
//        var dto = new OrderItemCreatedDto(productId, name, unitPrice, quantity);

//        // Assert
//        dto.Should().NotBeNull();
//        dto.ProductId.Should().Be(productId);
//        dto.Name.Should().Be(name);
//        dto.UnitPrice.Should().Be(unitPrice);
//        dto.Quantity.Should().Be(quantity);
//    }

//    [Fact(DisplayName = "OrderItemCreatedDto should handle zero quantity")]
//    [Trait("Application", "OrderItemCreatedDto")]
//    public void Constructor_WithZeroQuantity_ShouldAcceptIt()
//    {
//        // Arrange
//        var productId = Guid.NewGuid();
//        var name = "Product";
//        var unitPrice = 10.00m;
//        var quantity = 0;

//        // Act
//        var dto = new OrderItemCreatedDto(productId, name, unitPrice, quantity);

//        // Assert
//        dto.Quantity.Should().Be(0);
//    }

//    [Fact(DisplayName = "OrderItemCreatedDto should handle zero price")]
//    [Trait("Application", "OrderItemCreatedDto")]
//    public void Constructor_WithZeroPrice_ShouldAcceptIt()
//    {
//        // Arrange
//        var productId = Guid.NewGuid();
//        var name = "Free Item";
//        var unitPrice = 0m;
//        var quantity = 1;

//        // Act
//        var dto = new OrderItemCreatedDto(productId, name, unitPrice, quantity);

//        // Assert
//        dto.UnitPrice.Should().Be(0m);
//    }

//    [Fact(DisplayName = "OrderItemCreatedDto should handle large prices")]
//    [Trait("Application", "OrderItemCreatedDto")]
//    public void Constructor_WithLargePrice_ShouldWork()
//    {
//        // Arrange
//        var productId = Guid.NewGuid();
//        var name = "Expensive Product";
//        var unitPrice = 999999.99m;
//        var quantity = 1;

//        // Act
//        var dto = new OrderItemCreatedDto(productId, name, unitPrice, quantity);

//        // Assert
//        dto.UnitPrice.Should().Be(999999.99m);
//    }

//    [Fact(DisplayName = "OrderItemCreatedDto should handle large quantities")]
//    [Trait("Application", "OrderItemCreatedDto")]
//    public void Constructor_WithLargeQuantity_ShouldWork()
//    {
//        // Arrange
//        var productId = Guid.NewGuid();
//        var name = "Bulk Product";
//        var unitPrice = 5.00m;
//        var quantity = 1000;

//        // Act
//        var dto = new OrderItemCreatedDto(productId, name, unitPrice, quantity);

//        // Assert
//        dto.Quantity.Should().Be(1000);
//    }

//    [Fact(DisplayName = "OrderItemCreatedDto should preserve decimal precision")]
//    [Trait("Application", "OrderItemCreatedDto")]
//    public void Constructor_ShouldPreserveDecimalPrecision()
//    {
//        // Arrange
//        var productId = Guid.NewGuid();
//        var name = "Precise Product";
//        var unitPrice = 12.345m;
//        var quantity = 3;

//        // Act
//        var dto = new OrderItemCreatedDto(productId, name, unitPrice, quantity);

//        // Assert
//        dto.UnitPrice.Should().Be(12.345m);
//    }

//    [Fact(DisplayName = "OrderItemCreatedDto should handle empty name")]
//    [Trait("Application", "OrderItemCreatedDto")]
//    public void Constructor_WithEmptyName_ShouldAcceptIt()
//    {
//        // Arrange
//        var productId = Guid.NewGuid();
//        var name = "";
//        var unitPrice = 10.00m;
//        var quantity = 1;

//        // Act
//        var dto = new OrderItemCreatedDto(productId, name, unitPrice, quantity);

//        // Assert
//        dto.Name.Should().Be("");
//    }

//    [Fact(DisplayName = "OrderItemCreatedDto should handle special characters in name")]
//    [Trait("Application", "OrderItemCreatedDto")]
//    public void Constructor_WithSpecialCharactersInName_ShouldPreserveThem()
//    {
//        // Arrange
//        var productId = Guid.NewGuid();
//        var name = "Product™ with Special®️ Chars & Symbols!";
//        var unitPrice = 15.50m;
//        var quantity = 1;

//        // Act
//        var dto = new OrderItemCreatedDto(productId, name, unitPrice, quantity);

//        // Assert
//        dto.Name.Should().Be(name);
//    }

//    [Theory(DisplayName = "OrderItemCreatedDto should work with various valid scenarios")]
//    [Trait("Application", "OrderItemCreatedDto")]
//    [InlineData("Burger", 25.50, 2)]
//    [InlineData("Pizza", 35.00, 1)]
//    [InlineData("Soda", 5.00, 3)]
//    [InlineData("Fries", 8.50, 4)]
//    public void Constructor_WithVariousScenarios_ShouldWorkCorrectly(string name, decimal price, int quantity)
//    {
//        // Arrange
//        var productId = Guid.NewGuid();

//        // Act
//        var dto = new OrderItemCreatedDto(productId, name, price, quantity);

//        // Assert
//        dto.ProductId.Should().NotBe(Guid.Empty);
//        dto.Name.Should().Be(name);
//        dto.UnitPrice.Should().Be(price);
//        dto.Quantity.Should().Be(quantity);
//    }

//    [Fact(DisplayName = "OrderItemCreatedDto should be serializable")]
//    [Trait("Application", "OrderItemCreatedDto")]
//    public void OrderItemCreatedDto_ShouldBeSerializable()
//    {
//        // Arrange
//        var productId = Guid.NewGuid();
//        var dto = new OrderItemCreatedDto(productId, "Burger", 25.50m, 2);

//        // Act
//        var json = System.Text.Json.JsonSerializer.Serialize(dto);
//        var deserializedDto = System.Text.Json.JsonSerializer.Deserialize<OrderItemCreatedDto>(json);

//        // Assert
//        deserializedDto.Should().NotBeNull();
//        deserializedDto!.ProductId.Should().Be(productId);
//        deserializedDto.Name.Should().Be("Burger");
//        deserializedDto.UnitPrice.Should().Be(25.50m);
//        deserializedDto.Quantity.Should().Be(2);
//    }

//    [Fact(DisplayName = "OrderItemCreatedDto record equality should work correctly")]
//    [Trait("Application", "OrderItemCreatedDto")]
//    public void RecordEquality_WithSameValues_ShouldBeEqual()
//    {
//        // Arrange
//        var productId = Guid.NewGuid();
//        var dto1 = new OrderItemCreatedDto(productId, "Burger", 25.50m, 2);
//        var dto2 = new OrderItemCreatedDto(productId, "Burger", 25.50m, 2);

//        // Assert
//        dto1.Should().Be(dto2);
//        dto1.GetHashCode().Should().Be(dto2.GetHashCode());
//    }

//    [Fact(DisplayName = "OrderItemCreatedDto with statement should create new instance")]
//    [Trait("Application", "OrderItemCreatedDto")]
//    public void WithStatement_ShouldCreateNewInstance()
//    {
//        // Arrange
//        var productId = Guid.NewGuid();
//        var original = new OrderItemCreatedDto(productId, "Burger", 25.50m, 2);

//        // Act
//        var modified = original with { Quantity = 5 };

//        // Assert
//        modified.Quantity.Should().Be(5);
//        modified.ProductId.Should().Be(productId);
//        modified.Name.Should().Be("Burger");
//        modified.UnitPrice.Should().Be(25.50m);
//        original.Quantity.Should().Be(2); // Original unchanged
//    }

//    [Fact(DisplayName = "OrderItemCreatedDto should handle negative prices")]
//    [Trait("Application", "OrderItemCreatedDto")]
//    public void Constructor_WithNegativePrice_ShouldAcceptIt()
//    {
//        // Arrange & Act
//        var dto = new OrderItemCreatedDto(Guid.NewGuid(), "Discount", -10.00m, 1);

//        // Assert
//        dto.UnitPrice.Should().Be(-10.00m);
//    }

//    [Fact(DisplayName = "OrderItemCreatedDto should handle negative quantities")]
//    [Trait("Application", "OrderItemCreatedDto")]
//    public void Constructor_WithNegativeQuantity_ShouldAcceptIt()
//    {
//        // Arrange & Act
//        var dto = new OrderItemCreatedDto(Guid.NewGuid(), "Return", 10.00m, -5);

//        // Assert
//        dto.Quantity.Should().Be(-5);
//    }

//    [Fact(DisplayName = "OrderItemCreatedDto should handle null or whitespace names")]
//    [Trait("Application", "OrderItemCreatedDto")]
//    public void Constructor_WithWhitespaceName_ShouldPreserveIt()
//    {
//        // Arrange
//        var whitespace = "   ";

//        // Act
//        var dto = new OrderItemCreatedDto(Guid.NewGuid(), whitespace, 10.00m, 1);

//        // Assert
//        dto.Name.Should().Be(whitespace);
//    }

//    [Fact(DisplayName = "OrderItemCreatedDto should handle very long product names")]
//    [Trait("Application", "OrderItemCreatedDto")]
//    public void Constructor_WithLongName_ShouldWork()
//    {
//        // Arrange
//        var longName = new string('A', 1000);

//        // Act
//        var dto = new OrderItemCreatedDto(Guid.NewGuid(), longName, 10.00m, 1);

//        // Assert
//        dto.Name.Should().HaveLength(1000);
//    }

//    [Fact(DisplayName = "OrderItemCreatedDto should handle unicode characters in name")]
//    [Trait("Application", "OrderItemCreatedDto")]
//    public void Constructor_WithUnicodeCharacters_ShouldPreserveThem()
//    {
//        // Arrange
//        var unicodeName = "Café ☕ with 日本語 and émojis 🍔🍕";

//        // Act
//        var dto = new OrderItemCreatedDto(Guid.NewGuid(), unicodeName, 15.50m, 1);

//        // Assert
//        dto.Name.Should().Be(unicodeName);
//    }

//    [Fact(DisplayName = "OrderItemCreatedDto should handle maximum decimal value")]
//    [Trait("Application", "OrderItemCreatedDto")]
//    public void Constructor_WithMaxDecimalPrice_ShouldWork()
//    {
//        // Arrange
//        var maxPrice = decimal.MaxValue;

//        // Act
//        var dto = new OrderItemCreatedDto(Guid.NewGuid(), "Expensive", maxPrice, 1);

//        // Assert
//        dto.UnitPrice.Should().Be(maxPrice);
//    }

//    [Fact(DisplayName = "OrderItemCreatedDto should handle minimum decimal value")]
//    [Trait("Application", "OrderItemCreatedDto")]
//    public void Constructor_WithMinDecimalPrice_ShouldWork()
//    {
//        // Arrange
//        var minPrice = decimal.MinValue;

//        // Act
//        var dto = new OrderItemCreatedDto(Guid.NewGuid(), "Min Price", minPrice, 1);

//        // Assert
//        dto.UnitPrice.Should().Be(minPrice);
//    }

//    [Fact(DisplayName = "OrderItemCreatedDto should handle maximum int quantity")]
//    [Trait("Application", "OrderItemCreatedDto")]
//    public void Constructor_WithMaxIntQuantity_ShouldWork()
//    {
//        // Arrange
//        var maxQuantity = int.MaxValue;

//        // Act
//        var dto = new OrderItemCreatedDto(Guid.NewGuid(), "Bulk", 1.00m, maxQuantity);

//        // Assert
//        dto.Quantity.Should().Be(maxQuantity);
//    }

//    [Fact(DisplayName = "OrderItemCreatedDto ToString should provide meaningful output")]
//    [Trait("Application", "OrderItemCreatedDto")]
//    public void ToString_ShouldProvideUsefulInformation()
//    {
//        // Arrange
//        var productId = Guid.NewGuid();
//        var dto = new OrderItemCreatedDto(productId, "Burger", 25.50m, 2);

//        // Act
//        var result = dto.ToString();

//        // Assert
//        result.Should().NotBeNullOrEmpty();
//        result.Should().Contain("Burger");
//    }

//    [Fact(DisplayName = "OrderItemCreatedDto with different values should not be equal")]
//    [Trait("Application", "OrderItemCreatedDto")]
//    public void RecordEquality_WithDifferentValues_ShouldNotBeEqual()
//    {
//        // Arrange
//        var productId1 = Guid.NewGuid();
//        var productId2 = Guid.NewGuid();
//        var dto1 = new OrderItemCreatedDto(productId1, "Burger", 25.50m, 2);
//        var dto2 = new OrderItemCreatedDto(productId2, "Pizza", 35.00m, 1);

//        // Assert
//        dto1.Should().NotBe(dto2);
//    }
//}
