using Microsoft.EntityFrameworkCore;
using TechFood.Payment.Infra.Persistence.Mappings;

namespace TechFood.Payment.Infra.Tests.Persistence;

[Trait("Category", "Unit")]
[Trait("Infra", "PaymentMap")]
public class PaymentMapTests
{
    [Fact]
    public void Configure_Should_MapToPaymentTable()
    {
        // Arrange
        var builder = new DbContextOptionsBuilder<DbContext>();
        builder.UseInMemoryDatabase("TestDb");

        var modelBuilder = new ModelBuilder();
        var entityBuilder = modelBuilder.Entity<Domain.Entities.Payment>();
        var map = new PaymentMap();

        // Act
        map.Configure(entityBuilder);
        var model = modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(Domain.Entities.Payment));

        // Assert
        entityType.Should().NotBeNull();
        entityType!.GetTableName().Should().Be("Payment");
    }

    [Fact]
    public void PaymentMap_Should_ImplementIEntityTypeConfiguration()
    {
        // Arrange
        var map = new PaymentMap();

        // Assert
        map.Should().BeAssignableTo<IEntityTypeConfiguration<Domain.Entities.Payment>>();
    }

    [Fact]
    public void Configure_Should_NotThrowException()
    {
        // Arrange
        var modelBuilder = new ModelBuilder();
        var entityBuilder = modelBuilder.Entity<Domain.Entities.Payment>();
        var map = new PaymentMap();

        // Act
        var act = () => map.Configure(entityBuilder);

        // Assert
        act.Should().NotThrow();
    }
}
