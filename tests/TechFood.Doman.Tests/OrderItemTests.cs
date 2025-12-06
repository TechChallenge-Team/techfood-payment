namespace TechFood.Doman.Tests
{
    public class OrderItemTests
    {
        [Fact(DisplayName = "Should create order item with valid data")]
        [Trait("OrderItem", "Create OrderItem")]
        public void ShouldCreateOrderItem_WithValidData()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var unitPrice = 25.90m;
            var quantity = 2;

            // Act
            var orderItem = new TechFood.Payment.Domain.Entities.OrderItem(
                productId: productId,
                unitPrice: unitPrice,
                quantity: quantity);

            // Assert
            Assert.NotEqual(Guid.Empty, orderItem.Id);
            Assert.Equal(productId, orderItem.ProductId);
            Assert.Equal(unitPrice, orderItem.UnitPrice);
            Assert.Equal(quantity, orderItem.Quantity);
        }

        [Fact(DisplayName = "Should throw exception when product ID is empty")]
        [Trait("OrderItem", "Validation")]
        public void ShouldThrowException_WhenProductIdIsEmpty()
        {
            // Arrange & Act & Assert
            Assert.Throws<TechFood.Shared.Domain.Exceptions.DomainException>(() =>
                new TechFood.Payment.Domain.Entities.OrderItem(
                    productId: Guid.Empty,
                    unitPrice: 10m,
                    quantity: 1));
        }

        [Theory(DisplayName = "Should throw exception when unit price is negative")]
        [Trait("OrderItem", "Validation")]
        [InlineData(-1)]
        [InlineData(-10.50)]
        public void ShouldThrowException_WhenUnitPriceIsNegative(decimal unitPrice)
        {
            // Arrange & Act & Assert
            Assert.Throws<TechFood.Shared.Domain.Exceptions.DomainException>(() =>
                new TechFood.Payment.Domain.Entities.OrderItem(
                    productId: Guid.NewGuid(),
                    unitPrice: unitPrice,
                    quantity: 1));
        }

        [Theory(DisplayName = "Should throw exception when quantity is negative")]
        [Trait("OrderItem", "Validation")]
        [InlineData(-1)]
        [InlineData(-10)]
        public void ShouldThrowException_WhenQuantityIsNegative(int quantity)
        {
            // Arrange & Act & Assert
            Assert.Throws<TechFood.Shared.Domain.Exceptions.DomainException>(() =>
                new TechFood.Payment.Domain.Entities.OrderItem(
                    productId: Guid.NewGuid(),
                    unitPrice: 10m,
                    quantity: quantity));
        }

        [Fact(DisplayName = "Should update quantity successfully")]
        [Trait("OrderItem", "Update Quantity")]
        public void ShouldUpdateQuantity_Successfully()
        {
            // Arrange
            var orderItem = new TechFood.Payment.Domain.Entities.OrderItem(
                productId: Guid.NewGuid(),
                unitPrice: 10m,
                quantity: 1);

            var newQuantity = 5;

            // Act
            orderItem.UpdateQuantity(newQuantity);

            // Assert
            Assert.Equal(newQuantity, orderItem.Quantity);
        }

        [Theory(DisplayName = "Should throw exception when updating quantity to negative")]
        [Trait("OrderItem", "Update Quantity")]
        [InlineData(-1)]
        [InlineData(-5)]
        public void ShouldThrowException_WhenUpdatingQuantityToNegative(int quantity)
        {
            // Arrange
            var orderItem = new TechFood.Payment.Domain.Entities.OrderItem(
                productId: Guid.NewGuid(),
                unitPrice: 10m,
                quantity: 1);

            // Act & Assert
            Assert.Throws<TechFood.Shared.Domain.Exceptions.DomainException>(() => orderItem.UpdateQuantity(quantity));
        }

        [Theory(DisplayName = "Should create order item with various valid quantities")]
        [Trait("OrderItem", "Create OrderItem")]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(100)]
        public void ShouldCreateOrderItem_WithVariousValidQuantities(int quantity)
        {
            // Arrange & Act
            var orderItem = new TechFood.Payment.Domain.Entities.OrderItem(
                productId: Guid.NewGuid(),
                unitPrice: 10m,
                quantity: quantity);

            // Assert
            Assert.Equal(quantity, orderItem.Quantity);
        }

        [Theory(DisplayName = "Should create order item with various valid prices")]
        [Trait("OrderItem", "Create OrderItem")]
        [InlineData(0.01)]
        [InlineData(5.50)]
        [InlineData(10.00)]
        [InlineData(99.99)]
        [InlineData(1000.00)]
        public void ShouldCreateOrderItem_WithVariousValidPrices(decimal unitPrice)
        {
            // Arrange & Act
            var orderItem = new TechFood.Payment.Domain.Entities.OrderItem(
                productId: Guid.NewGuid(),
                unitPrice: unitPrice,
                quantity: 1);

            // Assert
            Assert.Equal(unitPrice, orderItem.UnitPrice);
        }

        [Fact(DisplayName = "Should maintain product ID and unit price after quantity update")]
        [Trait("OrderItem", "Update Quantity")]
        public void ShouldMaintainProductIdAndUnitPrice_AfterQuantityUpdate()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var unitPrice = 15.99m;
            var orderItem = new TechFood.Payment.Domain.Entities.OrderItem(
                productId: productId,
                unitPrice: unitPrice,
                quantity: 1);

            // Act
            orderItem.UpdateQuantity(3);

            // Assert
            Assert.Equal(productId, orderItem.ProductId);
            Assert.Equal(unitPrice, orderItem.UnitPrice);
            Assert.Equal(3, orderItem.Quantity);
        }
    }
}
