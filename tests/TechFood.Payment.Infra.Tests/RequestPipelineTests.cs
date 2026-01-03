using Microsoft.AspNetCore.Builder;

namespace TechFood.Payment.Infra.Tests;

[Trait("Category", "Unit")]
[Trait("Infra", "RequestPipeline")]
public class RequestPipelineTests
{
    [Fact]
    public void UseInfra_Should_ReturnApplicationBuilder()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder();
        var app = builder.Build();

        // Act
        var result = app.UseInfra();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeSameAs(app);
    }

    [Fact]
    public void UseInfra_Should_BeExtensionMethod()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder();
        var app = builder.Build();

        // Act
        var act = () => app.UseInfra();

        // Assert
        act.Should().NotThrow();
    }
}
