using Microsoft.EntityFrameworkCore;
using TechFood.Payment.Infra.Persistence.Contexts;
using TechFood.Payment.Infra.Persistence.Repositories;
using TechFood.Shared.Domain.Enums;

namespace TechFood.Payment.Infra.Tests.Repositories;

public class PaymentRepositoryTests
{
    private PaymentContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<PaymentContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new PaymentContext(options);
    }

    [Fact(DisplayName = "AddAsync should add payment and return id")]
    [Trait("Infra", "PaymentRepository")]
    public async Task AddAsync_ShouldAddPaymentAndReturnId()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new PaymentRepository(context);

        var payment = new Domain.Entities.Payment(
            Guid.NewGuid(),
            PaymentType.MercadoPago,
            100.00m);

        // Act
        var result = await repository.AddAsync(payment);
        await context.SaveChangesAsync();

        // Assert
        result.Should().NotBe(Guid.Empty);
        result.Should().Be(payment.Id);

        var savedPayment = await context.Payments.FindAsync(payment.Id);
        savedPayment.Should().NotBeNull();
        savedPayment!.Amount.Should().Be(100.00m);
    }

    [Fact(DisplayName = "GetByIdAsync should return payment when exists")]
    [Trait("Infra", "PaymentRepository")]
    public async Task GetByIdAsync_WhenExists_ShouldReturnPayment()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new PaymentRepository(context);

        var payment = new Domain.Entities.Payment(
            Guid.NewGuid(),
            PaymentType.CreditCard,
            250.00m);

        await context.Payments.AddAsync(payment);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByIdAsync(payment.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(payment.Id);
        result.Amount.Should().Be(250.00m);
        result.Type.Should().Be(PaymentType.CreditCard);
    }

    [Fact(DisplayName = "GetByIdAsync should return null when not exists")]
    [Trait("Infra", "PaymentRepository")]
    public async Task GetByIdAsync_WhenNotExists_ShouldReturnNull()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new PaymentRepository(context);
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await repository.GetByIdAsync(nonExistentId);

        // Assert
        result.Should().BeNull();
    }

    [Fact(DisplayName = "AddAsync should handle multiple payments")]
    [Trait("Infra", "PaymentRepository")]
    public async Task AddAsync_ShouldHandleMultiplePayments()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new PaymentRepository(context);

        var payment1 = new Domain.Entities.Payment(Guid.NewGuid(), PaymentType.MercadoPago, 100.00m);
        var payment2 = new Domain.Entities.Payment(Guid.NewGuid(), PaymentType.CreditCard, 200.00m);
        var payment3 = new Domain.Entities.Payment(Guid.NewGuid(), PaymentType.MercadoPago, 300.00m);

        // Act
        await repository.AddAsync(payment1);
        await repository.AddAsync(payment2);
        await repository.AddAsync(payment3);
        await context.SaveChangesAsync();

        // Assert
        var allPayments = await context.Payments.ToListAsync();
        allPayments.Should().HaveCount(3);
    }
}
