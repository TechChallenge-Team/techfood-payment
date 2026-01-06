using TechFood.Payment.Application.Common.Data;

namespace TechFood.Payment.Application.Tests.Data;

public class PaymentItemTests
{
    [Fact(DisplayName = "PaymentItem should be created with all properties")]
    [Trait("Application", "PaymentItem (Data)")]
    public void Constructor_WithAllProperties_ShouldSetThemCorrectly()
    {
        // Arrange
        var title = "Premium Burger";
        var quantity = 3;
        var unit = "unit";
        var unitPrice = 25.50m;
        var amount = 76.50m;

        // Act
        var item = new PaymentItem(title, quantity, unit, unitPrice, amount);

        // Assert
        item.Should().NotBeNull();
        item.Title.Should().Be(title);
        item.Quantity.Should().Be(quantity);
        item.Unit.Should().Be(unit);
        item.UnitPrice.Should().Be(unitPrice);
        item.Amount.Should().Be(amount);
    }

    [Fact(DisplayName = "PaymentItem should handle zero quantity")]
    [Trait("Application", "PaymentItem (Data)")]
    public void Constructor_WithZeroQuantity_ShouldAcceptIt()
    {
        // Arrange & Act
        var item = new PaymentItem("Product", 0, "unit", 10.00m, 0m);

        // Assert
        item.Quantity.Should().Be(0);
        item.Amount.Should().Be(0m);
    }

    [Fact(DisplayName = "PaymentItem should handle zero price and amount")]
    [Trait("Application", "PaymentItem (Data)")]
    public void Constructor_WithZeroPriceAndAmount_ShouldAcceptIt()
    {
        // Arrange & Act
        var item = new PaymentItem("Free Item", 1, "unit", 0m, 0m);

        // Assert
        item.UnitPrice.Should().Be(0m);
        item.Amount.Should().Be(0m);
    }

    [Fact(DisplayName = "PaymentItem should calculate amount correctly")]
    [Trait("Application", "PaymentItem (Data)")]
    public void Constructor_WithQuantityAndPrice_AmountShouldBeCorrect()
    {
        // Arrange
        var quantity = 5;
        var unitPrice = 12.50m;
        var expectedAmount = 62.50m;

        // Act
        var item = new PaymentItem("Pizza", quantity, "unit", unitPrice, expectedAmount);

        // Assert
        item.Quantity.Should().Be(quantity);
        item.UnitPrice.Should().Be(unitPrice);
        item.Amount.Should().Be(expectedAmount);
    }

    [Fact(DisplayName = "PaymentItem should handle decimal precision")]
    [Trait("Application", "PaymentItem (Data)")]
    public void Constructor_ShouldPreserveDecimalPrecision()
    {
        // Arrange
        var unitPrice = 12.345m;
        var amount = 37.035m; // 12.345 * 3

        // Act
        var item = new PaymentItem("Precise Product", 3, "kg", unitPrice, amount);

        // Assert
        item.UnitPrice.Should().Be(12.345m);
        item.Amount.Should().Be(37.035m);
    }

    [Fact(DisplayName = "PaymentItem should handle different unit types")]
    [Trait("Application", "PaymentItem (Data)")]
    public void Constructor_WithDifferentUnits_ShouldAcceptThem()
    {
        // Arrange & Act
        var item1 = new PaymentItem("Beverage", 2, "liters", 5.00m, 10.00m);
        var item2 = new PaymentItem("Meat", 1, "kg", 45.00m, 45.00m);
        var item3 = new PaymentItem("Sandwich", 3, "unit", 8.50m, 25.50m);

        // Assert
        item1.Unit.Should().Be("liters");
        item2.Unit.Should().Be("kg");
        item3.Unit.Should().Be("unit");
    }

    [Fact(DisplayName = "PaymentItem should handle large quantities")]
    [Trait("Application", "PaymentItem (Data)")]
    public void Constructor_WithLargeQuantity_ShouldWork()
    {
        // Arrange & Act
        var item = new PaymentItem("Bulk Item", 1000, "unit", 0.50m, 500.00m);

        // Assert
        item.Quantity.Should().Be(1000);
        item.Amount.Should().Be(500.00m);
    }

    [Fact(DisplayName = "PaymentItem should handle large prices")]
    [Trait("Application", "PaymentItem (Data)")]
    public void Constructor_WithLargePrice_ShouldWork()
    {
        // Arrange & Act
        var item = new PaymentItem("Expensive Item", 1, "unit", 999999.99m, 999999.99m);

        // Assert
        item.UnitPrice.Should().Be(999999.99m);
        item.Amount.Should().Be(999999.99m);
    }

    [Fact(DisplayName = "PaymentItem should handle empty title")]
    [Trait("Application", "PaymentItem (Data)")]
    public void Constructor_WithEmptyTitle_ShouldAcceptIt()
    {
        // Arrange & Act
        var item = new PaymentItem("", 1, "unit", 10.00m, 10.00m);

        // Assert
        item.Title.Should().Be("");
    }

    [Fact(DisplayName = "PaymentItem should handle special characters in title")]
    [Trait("Application", "PaymentItem (Data)")]
    public void Constructor_WithSpecialCharactersInTitle_ShouldPreserveThem()
    {
        // Arrange
        var title = "Product™ with Special®️ Chars & Symbols! @#$%";

        // Act
        var item = new PaymentItem(title, 1, "unit", 15.00m, 15.00m);

        // Assert
        item.Title.Should().Be(title);
    }

    [Theory(DisplayName = "PaymentItem should work with various valid scenarios")]
    [Trait("Application", "PaymentItem (Data)")]
    [InlineData("Burger", 2, "unit", 25.50, 51.00)]
    [InlineData("Pizza", 1, "unit", 35.00, 35.00)]
    [InlineData("Soda", 3, "liters", 5.00, 15.00)]
    [InlineData("Fries", 4, "unit", 8.50, 34.00)]
    public void Constructor_WithVariousScenarios_ShouldWorkCorrectly(
        string title, int quantity, string unit, decimal unitPrice, decimal amount)
    {
        // Act
        var item = new PaymentItem(title, quantity, unit, unitPrice, amount);

        // Assert
        item.Title.Should().Be(title);
        item.Quantity.Should().Be(quantity);
        item.Unit.Should().Be(unit);
        item.UnitPrice.Should().Be(unitPrice);
        item.Amount.Should().Be(amount);
    }

    [Fact(DisplayName = "PaymentItem equality should work for records")]
    [Trait("Application", "PaymentItem (Data)")]
    public void PaymentItem_RecordEquality_ShouldWork()
    {
        // Arrange
        var item1 = new PaymentItem("Burger", 2, "unit", 25.50m, 51.00m);
        var item2 = new PaymentItem("Burger", 2, "unit", 25.50m, 51.00m);
        var item3 = new PaymentItem("Pizza", 2, "unit", 25.50m, 51.00m);

        // Assert
        item1.Should().Be(item2);
        item1.Should().NotBe(item3);
    }

    [Fact(DisplayName = "PaymentItem with statement should create new instance")]
    [Trait("Application", "PaymentItem (Data)")]
    public void PaymentItem_WithStatement_ShouldCreateNewInstance()
    {
        // Arrange
        var original = new PaymentItem("Burger", 2, "unit", 25.50m, 51.00m);

        // Act
        var modified = original with { Quantity = 3, Amount = 76.50m };

        // Assert
        modified.Quantity.Should().Be(3);
        modified.Amount.Should().Be(76.50m);
        modified.Title.Should().Be("Burger");
        original.Quantity.Should().Be(2); // Original unchanged
    }
}
