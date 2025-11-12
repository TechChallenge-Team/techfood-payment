using TechFood.Domain.Entities;
using TechFood.Shared.Domain.Enums;
using TechFood.Shared.Domain.Exceptions;

namespace TechFood.Doman.Tests
{
    public class PaymentTests
    {
        [Fact(DisplayName = "Validate pay Payment when already paid")]
        [Trait("Payment", "Pay Payment")]
        public void ShoudThrowException_WhenPayPaymentThatHasPaidAt()
        {
            // Arrange
            var payment = new Payment(
                orderId: Guid.NewGuid(),
                type: PaymentType.CreditCard,
                amount: 10);

            payment.Confirm();

            // Act
            var result = Assert.Throws<DomainException>(payment.Confirm);

            //// Assert
            Assert.Equal(Domain.Resources.Exceptions.Payment_AlreadyPaid, result.Message);
        }

        [Fact(DisplayName = "Validate refused Payment when already paid")]
        [Trait("Payment", "Refused Payment")]
        public void ShoudThrowException_WhenRefusedPaymentThatHasPaidAt()
        {
            // Arrange
            var payment = new Payment(
                orderId: Guid.NewGuid(),
                type: PaymentType.CreditCard,
                amount: 10);

            payment.Confirm();

            // Act
            var result = Assert.Throws<DomainException>(payment.Refused);

            //// Assert
            Assert.Equal(Domain.Resources.Exceptions.Payment_AlreadyPaid, result.Message);
        }
    }
}
