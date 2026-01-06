using Microsoft.Extensions.DependencyInjection;
namespace TechFood.Payment.Application.Tests.DependencyInjection;

public class DependecyInjectionTests
{
    [Fact(DisplayName = "AddApplication should register MediatR")]
    [Trait("Application", "DependencyInjection")]
    public void AddApplication_ShouldRegisterMediatR()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddApplication();
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        services.Should().NotBeEmpty();
        services.Should().Contain(s => s.ServiceType.FullName!.Contains("MediatR"));
    }

    [Fact(DisplayName = "AddApplication should return the same IServiceCollection")]
    [Trait("Application", "DependencyInjection")]
    public void AddApplication_ShouldReturnServiceCollection()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var result = services.AddApplication();

        // Assert
        result.Should().BeSameAs(services);
    }

    [Fact(DisplayName = "AddApplication should configure services successfully")]
    [Trait("Application", "DependencyInjection")]
    public void AddApplication_ShouldConfigureServicesSuccessfully()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        Action act = () => services.AddApplication();

        // Assert
        act.Should().NotThrow();
    }

    [Fact(DisplayName = "AddApplication should allow building ServiceProvider")]
    [Trait("Application", "DependencyInjection")]
    public void AddApplication_ShouldAllowBuildingServiceProvider()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddApplication();

        // Act
        Action act = () => services.BuildServiceProvider();

        // Assert
        act.Should().NotThrow();
    }

    [Fact(DisplayName = "AddApplication should register services from Application assembly")]
    [Trait("Application", "DependencyInjection")]
    public void AddApplication_ShouldRegisterServicesFromApplicationAssembly()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddApplication();

        // Assert
        var hasApplicationService = services.Any(s => 
            s.ServiceType.Assembly == typeof(DependecyInjection).Assembly ||
            (s.ImplementationType != null && s.ImplementationType.Assembly == typeof(DependecyInjection).Assembly));
        hasApplicationService.Should().BeTrue();
    }
}
