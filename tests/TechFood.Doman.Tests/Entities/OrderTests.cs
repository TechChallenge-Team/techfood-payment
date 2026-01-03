using TechFood.Payment.Domain.Entities;
using TechFood.Shared.Domain.Enums;

namespace TechFood.Doman.Tests.Entities;

[Trait("Category", "Unit")]
[Trait("Domain", "Order")]
public class OrderTests
{
    [Fact]
    public void Constructor_Should_CreateOrder_WithValidData()
    {
        // Arrange
        var number = 123;
        var customerId = Guid.NewGuid();

        // Act
        var order = new Order(number, customerId);

        // Assert
        order.Should().NotBeNull();
        order.Number.Should().Be(number);
        order.CustomerId.Should().Be(customerId);
        order.Status.Should().Be(OrderStatusType.Pending);
        order.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        order.Amount.Should().Be(0);
        order.Discount.Should().Be(0);
    }

    [Fact]
    public void Constructor_Should_CreateOrder_WithoutCustomerId()
    {
        // Arrange
        var number = 456;

        // Act
        var order = new Order(number);

        // Assert
        order.Should().NotBeNull();
        order.Number.Should().Be(number);
        order.CustomerId.Should().BeNull();
        order.Status.Should().Be(OrderStatusType.Pending);
    }

    [Fact]
    public void Constructor_Should_CreateOrder_WithNullCustomerId()
    {
        // Arrange
        var number = 789;

        // Act
        var order = new Order(number, null);

        // Assert
        order.Should().NotBeNull();
        order.Number.Should().Be(number);
        order.CustomerId.Should().BeNull();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(9999)]
    public void Constructor_Should_SetNumber_Correctly(int number)
    {
        // Act
        var order = new Order(number);

        // Assert
        order.Number.Should().Be(number);
    }

    [Fact]
    public void Properties_Should_BeInitializedCorrectly()
    {
        // Act
        var order = new Order(1, Guid.NewGuid());

        // Assert
        order.Id.Should().NotBeEmpty();
        order.Number.Should().BeGreaterThan(0);
        order.CreatedAt.Should().BeBefore(DateTime.Now.AddSeconds(1));
        order.Status.Should().Be(OrderStatusType.Pending);
    }
}
