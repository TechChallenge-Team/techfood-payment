using TechFood.Payment.Domain.Entities;
using TechFood.Shared.Domain.Enums;

namespace TechFood.Doman.Tests.Entities;

[Trait("Category", "Unit")]
[Trait("Domain", "OrderHistory")]
public class OrderHistoryTests
{
    [Fact]
    public void Constructor_Should_CreateOrderHistory_WithValidStatus()
    {
        // Arrange
        var status = OrderStatusType.Ready;

        // Act
        var history = new OrderHistory(status);

        // Assert
        history.Should().NotBeNull();
        history.Status.Should().Be(status);
        history.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
    }

    [Theory]
    [InlineData(OrderStatusType.Pending)]
    [InlineData(OrderStatusType.InPreparation)]
    [InlineData(OrderStatusType.Ready)]
    public void Constructor_Should_CreateOrderHistory_WithDifferentStatuses(OrderStatusType status)
    {
        // Act
        var history = new OrderHistory(status);

        // Assert
        history.Status.Should().Be(status);
        history.CreatedAt.Should().BeBefore(DateTime.Now.AddSeconds(1));
    }

    [Fact]
    public void Properties_Should_BeInitializedCorrectly()
    {
        // Arrange
        var status = OrderStatusType.Ready;

        // Act
        var history = new OrderHistory(status);

        // Assert
        history.Id.Should().NotBeEmpty();
        history.Status.Should().Be(status);
        history.CreatedAt.Should().NotBe(default(DateTime));
    }
}
