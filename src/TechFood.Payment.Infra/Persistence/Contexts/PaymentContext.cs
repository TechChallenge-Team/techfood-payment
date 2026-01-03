using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechFood.Shared.Domain.Entities;
using TechFood.Shared.Infra.Persistence.Contexts;

namespace TechFood.Payment.Infra.Persistence.Contexts;

public class PaymentContext : TechFoodContext
{
    public DbSet<Domain.Entities.Payment> Payments { get; set; } = null!;

    public PaymentContext(DbContextOptions<PaymentContext> options) : base(options) { }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker
            .Entries<Entity>()
            .Where(e => e.State == EntityState.Deleted))
        {
            entry.State = EntityState.Modified;
            entry.Entity.IsDeleted = true;
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TechFoodContext).Assembly);

        var properties = modelBuilder.Model
            .GetEntityTypes()
            .SelectMany(t => t.GetProperties());

        var stringProperties = properties.Where(p => p.ClrType == typeof(string));
        foreach (var property in stringProperties)
        {
            var maxLength = property.GetMaxLength() ?? 50;

            property.SetColumnType($"varchar({maxLength})");
        }

        var booleanProperties = properties
            .Where(p => p.ClrType == typeof(bool) ||
                        p.ClrType == typeof(bool?));

        foreach (var property in booleanProperties)
        {
            property.SetColumnType("bit");
            property.IsNullable = false;
        }

        var dateTimeProperties = properties.Where(p => p.ClrType == typeof(DateTime));

        foreach (var property in dateTimeProperties)
        {
            property.SetColumnType("datetime");
        }

        var enumProperties = properties.Where(p => p.ClrType == typeof(Enum));

        foreach (var property in enumProperties)
        {
            property.SetColumnType("smallint");
        }

        var amountProperties = properties
            .Where(p => p.ClrType == typeof(decimal) ||
                        p.ClrType == typeof(decimal?));

        foreach (var property in amountProperties)
        {
            property.SetColumnType("decimal(6, 2)");
        }

        SeedContext(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    private static void SeedContext(ModelBuilder modelBuilder)
    {
        //modelBuilder.Entity<PaymentType>()
        //    .HasData(
        //        new { Id = 1, Code = "MCMA", Description = "Mastercard" },
        //        new { Id = 2, Code = "VIS", Description = "Visa" },
        //        new { Id = 3, Code = "ELO", Description = "Elo" },
        //        new { Id = 4, Code = "DNR", Description = "Sodexo" },
        //        new { Id = 5, Code = "VR", Description = "Vale Refeição" },
        //        new { Id = 6, Code = "PIX", Description = "Pix" }
        //    );
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
#if DEBUG
        optionsBuilder.LogTo(Console.WriteLine);
#endif
    }
}
