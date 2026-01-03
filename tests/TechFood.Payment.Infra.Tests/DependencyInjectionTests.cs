using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TechFood.Payment.Application.Common.Services.Interfaces;
using TechFood.Payment.Domain.Repositories;
using TechFood.Payment.Infra.Backoffice;
using TechFood.Payment.Infra.Order;
using TechFood.Payment.Infra.Payments.MercadoPago;
using TechFood.Payment.Infra.Persistence.Contexts;
using TechFood.Payment.Infra.Persistence.Repositories;
using TechFood.Shared.Domain.Enums;

namespace TechFood.Payment.Infra.Tests;

[Trait("Category", "Unit")]
[Trait("Infra", "DependencyInjection")]
public class DependencyInjectionTests
{
    [Fact]
    public void AddInfra_Should_RegisterPaymentContext()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = CreateConfiguration();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddLogging();

        // Act
        services.AddInfra();
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        var context = serviceProvider.GetService<PaymentContext>();
        context.Should().NotBeNull();
    }

    [Fact]
    public void AddInfra_Should_RegisterPaymentRepository()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = CreateConfiguration();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddLogging();

        // Act
        services.AddInfra();
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        var repository = serviceProvider.GetService<IPaymentRepository>();
        repository.Should().NotBeNull();
        repository.Should().BeOfType<PaymentRepository>();
    }

    [Fact]
    public void AddInfra_Should_RegisterOrderService()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = CreateConfiguration();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddLogging();
        services.AddHttpContextAccessor();

        // Act
        services.AddInfra();
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        var orderService = serviceProvider.GetService<IOrderService>();
        orderService.Should().NotBeNull();
        orderService.Should().BeOfType<OrderService>();
    }

    [Fact]
    public void AddInfra_Should_RegisterBackofficeService()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = CreateConfiguration();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddLogging();
        services.AddHttpContextAccessor();

        // Act
        services.AddInfra();
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        var backofficeService = serviceProvider.GetService<IBackofficeService>();
        backofficeService.Should().NotBeNull();
        backofficeService.Should().BeOfType<BackofficeService>();
    }

    [Fact]
    public void AddInfra_Should_RegisterMercadoPagoPaymentService()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = CreateConfiguration();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddLogging();
        services.AddHttpContextAccessor();

        // Act
        services.AddInfra();
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        var paymentService = serviceProvider.GetKeyedService<IPaymentService>(PaymentType.MercadoPago);
        paymentService.Should().NotBeNull();
        paymentService.Should().BeOfType<MercadoPagoPaymentService>();
    }

    [Fact]
    public void AddInfra_Should_ConfigureMercadoPagoOptions()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = CreateConfiguration();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddLogging();
        services.AddHttpContextAccessor();

        // Act
        services.AddInfra();
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        var options = serviceProvider.GetService<Microsoft.Extensions.Options.IOptions<MercadoPagoOptions>>();
        options.Should().NotBeNull();
        options!.Value.Should().NotBeNull();
    }

    [Fact]
    public void AddInfra_Should_RegisterHttpClientForMercadoPago()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = CreateConfiguration();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddLogging();
        services.AddHttpContextAccessor();

        // Act
        services.AddInfra();
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
        httpClientFactory.Should().NotBeNull();

        var client = httpClientFactory!.CreateClient(MercadoPagoOptions.ClientName);
        client.Should().NotBeNull();
    }

    [Fact]
    public void AddInfra_Should_ReturnServiceCollection()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = CreateConfiguration();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddLogging();

        // Act
        var result = services.AddInfra();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeSameAs(services);
    }

    [Fact]
    public void AddInfra_Should_NotThrowException()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = CreateConfiguration();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddLogging();

        // Act
        var act = () => services.AddInfra();

        // Assert
        act.Should().NotThrow();
    }

    private static IConfiguration CreateConfiguration()
    {
        var inMemorySettings = new Dictionary<string, string>
        {
            {"ConnectionStrings:DataBaseConection", "Server=(localdb)\\mssqllocaldb;Database=TestDb;Trusted_Connection=True;"},
            {"Payments:MercadoPago:AccessToken", "test-token"},
            {"Payments:MercadoPago:BaseAddress", "http://localhost:5000"},
            {"Order:BaseAddress", "http://localhost:45001"},
            {"Backoffice:BaseAddress", "http://localhost:45002"},
            {"Services:Authentication", "http://localhost:5001"},
            {"Services:Order", "http://localhost:45001"},
            {"Services:Backoffice", "http://localhost:45002"}
        };

        return new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
            .Build();
    }
}
