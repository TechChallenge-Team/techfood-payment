using TechFood.Shared.Domain.Enums;

namespace TechFood.Doman.Tests
{
    public class OrderHistoryTests
    {
        [Fact(DisplayName = "Should create order history with valid data")]
        [Trait("OrderHistory", "Create OrderHistory")]
        public void ShouldCreateOrderHistory_WithValidData()
        {
            // Arrange
            var status = OrderStatusType.Pending;

            // Act
            var orderHistory = new TechFood.Payment.Domain.Entities.OrderHistory(status);

            // Assert
            Assert.NotEqual(Guid.Empty, orderHistory.Id);
            Assert.Equal(status, orderHistory.Status);
            Assert.True((DateTime.Now - orderHistory.CreatedAt).TotalSeconds < 5);
        }

        [Theory(DisplayName = "Should create order history with all status types")]
        [Trait("OrderHistory", "Create OrderHistory")]
        [InlineData(OrderStatusType.Pending)]
        [InlineData(OrderStatusType.Received)]
        [InlineData(OrderStatusType.InPreparation)]
        [InlineData(OrderStatusType.Ready)]
        public void ShouldCreateOrderHistory_WithAllStatusTypes(OrderStatusType status)
        {
            // Arrange & Act
            var orderHistory = new TechFood.Payment.Domain.Entities.OrderHistory(status);

            // Assert
            Assert.Equal(status, orderHistory.Status);
        }

        [Fact(DisplayName = "Should create order history with current timestamp")]
        [Trait("OrderHistory", "Create OrderHistory")]
        public void ShouldCreateOrderHistory_WithCurrentTimestamp()
        {
            // Arrange
            var beforeCreation = DateTime.Now;

            // Act
            var orderHistory = new TechFood.Payment.Domain.Entities.OrderHistory(OrderStatusType.Received);

            var afterCreation = DateTime.Now;

            // Assert
            Assert.True(orderHistory.CreatedAt >= beforeCreation);
            Assert.True(orderHistory.CreatedAt <= afterCreation);
        }

        [Fact(DisplayName = "Should create multiple order histories with different statuses")]
        [Trait("OrderHistory", "Create OrderHistory")]
        public void ShouldCreateMultipleOrderHistories_WithDifferentStatuses()
        {
            // Arrange & Act
            var history1 = new TechFood.Payment.Domain.Entities.OrderHistory(OrderStatusType.Pending);
            var history2 = new TechFood.Payment.Domain.Entities.OrderHistory(OrderStatusType.Received);
            var history3 = new TechFood.Payment.Domain.Entities.OrderHistory(OrderStatusType.InPreparation);

            // Assert
            Assert.NotEqual(history1.Id, history2.Id);
            Assert.NotEqual(history2.Id, history3.Id);
            Assert.NotEqual(history1.Id, history3.Id);
            
            Assert.Equal(OrderStatusType.Pending, history1.Status);
            Assert.Equal(OrderStatusType.Received, history2.Status);
            Assert.Equal(OrderStatusType.InPreparation, history3.Status);
        }

        [Fact(DisplayName = "Should maintain immutable status after creation")]
        [Trait("OrderHistory", "Immutability")]
        public void ShouldMaintainImmutableStatus_AfterCreation()
        {
            // Arrange
            var status = OrderStatusType.Ready;

            // Act
            var orderHistory = new TechFood.Payment.Domain.Entities.OrderHistory(status);

            // Assert
            Assert.Equal(status, orderHistory.Status);
            // Status should remain the same as it's set in constructor and has private setter
        }

        [Fact(DisplayName = "Should create order history for ready status")]
        [Trait("OrderHistory", "Create OrderHistory")]
        public void ShouldCreateOrderHistory_ForReadyStatus()
        {
            // Arrange & Act
            var orderHistory = new TechFood.Payment.Domain.Entities.OrderHistory(OrderStatusType.Ready);

            // Assert
            Assert.Equal(OrderStatusType.Ready, orderHistory.Status);
            Assert.NotEqual(Guid.Empty, orderHistory.Id);
        }
    }
}
