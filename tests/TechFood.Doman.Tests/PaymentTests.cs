using TechFood.Shared.Domain.Enums;
using TechFood.Shared.Domain.Exceptions;

namespace TechFood.Doman.Tests
{
    public class PaymentTests
    {
        [Fact(DisplayName = "Should create payment with valid data")]
        [Trait("Payment", "Create Payment")]
        public void ShouldCreatePayment_WithValidData()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var paymentType = PaymentType.CreditCard;
            var amount = 100.50m;

            // Act
            var payment = new TechFood.Payment.Domain.Entities.Payment(
                orderId: orderId,
                type: paymentType,
                amount: amount);

            // Assert
            Assert.NotEqual(Guid.Empty, payment.Id);
            Assert.Equal(orderId, payment.OrderId);
            Assert.Equal(paymentType, payment.Type);
            Assert.Equal(amount, payment.Amount);
            Assert.Equal(PaymentStatusType.Pending, payment.Status);
            Assert.Null(payment.PaidAt);
            Assert.True((DateTime.Now - payment.CreatedAt).TotalSeconds < 5);
        }

        [Fact(DisplayName = "Should create payment with MercadoPago type")]
        [Trait("Payment", "Create Payment")]
        public void ShouldCreatePayment_WithMercadoPagoType()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var amount = 50.00m;

            // Act
            var payment = new TechFood.Payment.Domain.Entities.Payment(
                orderId: orderId,
                type: PaymentType.MercadoPago,
                amount: amount);

            // Assert
            Assert.Equal(PaymentType.MercadoPago, payment.Type);
            Assert.Equal(PaymentStatusType.Pending, payment.Status);
        }

        [Fact(DisplayName = "Should confirm payment successfully")]
        [Trait("Payment", "Confirm Payment")]
        public void ShouldConfirmPayment_Successfully()
        {
            // Arrange
            var payment = new TechFood.Payment.Domain.Entities.Payment(
                orderId: Guid.NewGuid(),
                type: PaymentType.CreditCard,
                amount: 10);

            // Act
            payment.Confirm();

            // Assert
            Assert.NotNull(payment.PaidAt);
            Assert.Equal(PaymentStatusType.Approved, payment.Status);
            Assert.True((DateTime.Now - payment.PaidAt.Value).TotalSeconds < 5);
        }

        [Fact(DisplayName = "Should throw exception when confirming already paid payment")]
        [Trait("Payment", "Pay Payment")]
        public void ShouldThrowException_WhenPayPaymentThatHasPaidAt()
        {
            // Arrange
            var payment = new TechFood.Payment.Domain.Entities.Payment(
                orderId: Guid.NewGuid(),
                type: PaymentType.CreditCard,
                amount: 10);

            payment.Confirm();

            // Act & Assert
            Assert.Throws<DomainException>(payment.Confirm);
        }

        [Fact(DisplayName = "Should refuse payment successfully")]
        [Trait("Payment", "Refused Payment")]
        public void ShouldRefusePayment_Successfully()
        {
            // Arrange
            var payment = new TechFood.Payment.Domain.Entities.Payment(
                orderId: Guid.NewGuid(),
                type: PaymentType.CreditCard,
                amount: 10);

            // Act
            payment.Refused();

            // Assert
            Assert.Equal(PaymentStatusType.Refused, payment.Status);
            Assert.Null(payment.PaidAt);
        }

        [Fact(DisplayName = "Should throw exception when refusing already paid payment")]
        [Trait("Payment", "Refused Payment")]
        public void ShouldThrowException_WhenRefusedPaymentThatHasPaidAt()
        {
            // Arrange
            var payment = new TechFood.Payment.Domain.Entities.Payment(
                orderId: Guid.NewGuid(),
                type: PaymentType.CreditCard,
                amount: 10);

            payment.Confirm();

            // Act & Assert
            Assert.Throws<DomainException>(payment.Refused);
        }

        [Theory(DisplayName = "Should create payment with different amounts")]
        [Trait("Payment", "Create Payment")]
        [InlineData(0.01)]
        [InlineData(10.99)]
        [InlineData(100.00)]
        [InlineData(1000.50)]
        [InlineData(9999.99)]
        public void ShouldCreatePayment_WithDifferentAmounts(decimal amount)
        {
            // Arrange & Act
            var payment = new TechFood.Payment.Domain.Entities.Payment(
                orderId: Guid.NewGuid(),
                type: PaymentType.CreditCard,
                amount: amount);

            // Assert
            Assert.Equal(amount, payment.Amount);
            Assert.Equal(PaymentStatusType.Pending, payment.Status);
        }

        [Theory(DisplayName = "Should create payment with all payment types")]
        [Trait("Payment", "Create Payment")]
        [InlineData(PaymentType.CreditCard)]
        [InlineData(PaymentType.MercadoPago)]
        public void ShouldCreatePayment_WithAllPaymentTypes(PaymentType paymentType)
        {
            // Arrange & Act
            var payment = new TechFood.Payment.Domain.Entities.Payment(
                orderId: Guid.NewGuid(),
                type: paymentType,
                amount: 50m);

            // Assert
            Assert.Equal(paymentType, payment.Type);
        }
    }
}
