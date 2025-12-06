using TechFood.Shared.Domain.Enums;

namespace TechFood.Doman.Tests
{
    public class OrderTests
    {
        [Fact(DisplayName = "Should create order with valid data")]
        [Trait("Order", "Create Order")]
        public void ShouldCreateOrder_WithValidData()
        {
            // Arrange
            var number = 1;
            var customerId = Guid.NewGuid();

            // Act
            var order = new TechFood.Payment.Domain.Entities.Order(
                number: number,
                customerId: customerId);

            // Assert
            Assert.NotEqual(Guid.Empty, order.Id);
            Assert.Equal(number, order.Number);
            Assert.Equal(customerId, order.CustomerId);
            Assert.Equal(OrderStatusType.Pending, order.Status);
            Assert.True((DateTime.Now - order.CreatedAt).TotalSeconds < 5);
            Assert.Equal(0, order.Amount);
            Assert.Equal(0, order.Discount);
        }

        [Fact(DisplayName = "Should create order without customer ID")]
        [Trait("Order", "Create Order")]
        public void ShouldCreateOrder_WithoutCustomerId()
        {
            // Arrange
            var number = 2;

            // Act
            var order = new TechFood.Payment.Domain.Entities.Order(
                number: number,
                customerId: null);

            // Assert
            Assert.NotEqual(Guid.Empty, order.Id);
            Assert.Equal(number, order.Number);
            Assert.Null(order.CustomerId);
            Assert.Equal(OrderStatusType.Pending, order.Status);
        }

        [Theory(DisplayName = "Should create order with different numbers")]
        [Trait("Order", "Create Order")]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(999)]
        [InlineData(9999)]
        public void ShouldCreateOrder_WithDifferentNumbers(int number)
        {
            // Arrange & Act
            var order = new TechFood.Payment.Domain.Entities.Order(
                number: number,
                customerId: Guid.NewGuid());

            // Assert
            Assert.Equal(number, order.Number);
        }

        [Fact(DisplayName = "Should initialize order with pending status")]
        [Trait("Order", "Create Order")]
        public void ShouldInitializeOrder_WithPendingStatus()
        {
            // Arrange & Act
            var order = new TechFood.Payment.Domain.Entities.Order(
                number: 1,
                customerId: Guid.NewGuid());

            // Assert
            Assert.Equal(OrderStatusType.Pending, order.Status);
        }

        [Fact(DisplayName = "Should create order with null customer for guest")]
        [Trait("Order", "Create Order")]
        public void ShouldCreateOrder_WithNullCustomerForGuest()
        {
            // Arrange
            var number = 5;

            // Act
            var order = new TechFood.Payment.Domain.Entities.Order(
                number: number);

            // Assert
            Assert.Null(order.CustomerId);
            Assert.Equal(number, order.Number);
        }

        [Fact(DisplayName = "Should initialize order with zero amount and discount")]
        [Trait("Order", "Create Order")]
        public void ShouldInitializeOrder_WithZeroAmountAndDiscount()
        {
            // Arrange & Act
            var order = new TechFood.Payment.Domain.Entities.Order(
                number: 1,
                customerId: Guid.NewGuid());

            // Assert
            Assert.Equal(0, order.Amount);
            Assert.Equal(0, order.Discount);
        }

        [Fact(DisplayName = "Should create order with current timestamp")]
        [Trait("Order", "Create Order")]
        public void ShouldCreateOrder_WithCurrentTimestamp()
        {
            // Arrange
            var beforeCreation = DateTime.Now;

            // Act
            var order = new TechFood.Payment.Domain.Entities.Order(
                number: 1,
                customerId: Guid.NewGuid());

            var afterCreation = DateTime.Now;

            // Assert
            Assert.True(order.CreatedAt >= beforeCreation);
            Assert.True(order.CreatedAt <= afterCreation);
        }
    }
}
