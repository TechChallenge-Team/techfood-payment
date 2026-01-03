using TechFood.Payment.Domain.Entities;
using TechFood.Shared.Domain.Exceptions;

namespace TechFood.Doman.Tests.Entities;

[Trait("Category", "Unit")]
[Trait("Domain", "OrderItem")]
public class OrderItemTests
{
    [Fact]
    public void Constructor_Should_CreateOrderItem_WithValidData()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var unitPrice = 15.50m;
        var quantity = 2;

        // Act
        var orderItem = new OrderItem(productId, unitPrice, quantity);

        // Assert
        orderItem.Should().NotBeNull();
        orderItem.ProductId.Should().Be(productId);
        orderItem.UnitPrice.Should().Be(unitPrice);
        orderItem.Quantity.Should().Be(quantity);
    }

    [Fact]
    public void Constructor_Should_ThrowDomainException_WhenProductIdIsEmpty()
    {
        // Arrange
        var productId = Guid.Empty;
        var unitPrice = 10.00m;
        var quantity = 1;

        // Act
        var act = () => new OrderItem(productId, unitPrice, quantity);

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-10)]
    public void Constructor_Should_ThrowDomainException_WhenUnitPriceIsNegative(decimal unitPrice)
    {
        // Arrange
        var productId = Guid.NewGuid();
        var quantity = 1;

        // Act
        var act = () => new OrderItem(productId, unitPrice, quantity);

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-5)]
    public void Constructor_Should_ThrowDomainException_WhenQuantityIsNegative(int quantity)
    {
        // Arrange
        var productId = Guid.NewGuid();
        var unitPrice = 10.00m;

        // Act
        var act = () => new OrderItem(productId, unitPrice, quantity);

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void UpdateQuantity_Should_UpdateQuantity_WithValidValue()
    {
        // Arrange
        var orderItem = new OrderItem(Guid.NewGuid(), 10.00m, 2);
        var newQuantity = 5;

        // Act
        orderItem.UpdateQuantity(newQuantity);

        // Assert
        orderItem.Quantity.Should().Be(newQuantity);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(100)]
    public void UpdateQuantity_Should_UpdateQuantity_WithDifferentValidValues(int newQuantity)
    {
        // Arrange
        var orderItem = new OrderItem(Guid.NewGuid(), 10.00m, 1);

        // Act
        orderItem.UpdateQuantity(newQuantity);

        // Assert
        orderItem.Quantity.Should().Be(newQuantity);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-10)]
    public void UpdateQuantity_Should_ThrowDomainException_WhenQuantityIsNegative(int invalidQuantity)
    {
        // Arrange
        var orderItem = new OrderItem(Guid.NewGuid(), 10.00m, 1);

        // Act
        var act = () => orderItem.UpdateQuantity(invalidQuantity);

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Properties_Should_BeInitializedCorrectly()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var unitPrice = 25.75m;
        var quantity = 3;

        // Act
        var orderItem = new OrderItem(productId, unitPrice, quantity);

        // Assert
        orderItem.Id.Should().NotBeEmpty();
        orderItem.ProductId.Should().Be(productId);
        orderItem.UnitPrice.Should().Be(unitPrice);
        orderItem.Quantity.Should().Be(quantity);
    }
}
