using TechFood.Payment.Infra.Payments.MercadoPago;

namespace TechFood.Payment.Infra.Tests.Payments;

public class MercadoPagoOptionsTests
{
    [Fact(DisplayName = "MercadoPagoOptions should have correct section name")]
    [Trait("Infra", "MercadoPagoOptions")]
    public void MercadoPagoOptions_ShouldHaveCorrectSectionName()
    {
        // Assert
        MercadoPagoOptions.SectionName.Should().Be("Payments:MercadoPago");
    }

    [Fact(DisplayName = "MercadoPagoOptions should have correct client name")]
    [Trait("Infra", "MercadoPagoOptions")]
    public void MercadoPagoOptions_ShouldHaveCorrectClientName()
    {
        // Assert
        MercadoPagoOptions.ClientName.Should().Be("MercadoPagoClient");
    }

    [Fact(DisplayName = "MercadoPagoOptions should have correct base address")]
    [Trait("Infra", "MercadoPagoOptions")]
    public void MercadoPagoOptions_ShouldHaveCorrectBaseAddress()
    {
        // Assert
        MercadoPagoOptions.BaseAddress.Should().Be("https://api.mercadopago.com/");
    }

    [Fact(DisplayName = "MercadoPagoOptions should initialize with default values")]
    [Trait("Infra", "MercadoPagoOptions")]
    public void MercadoPagoOptions_ShouldInitializeWithDefaultValues()
    {
        // Act
        var options = new MercadoPagoOptions();

        // Assert
        options.Should().NotBeNull();
        options.UserId.Should().BeNull();
        options.AccessToken.Should().BeNull();
    }

    [Fact(DisplayName = "MercadoPagoOptions should allow setting UserId")]
    [Trait("Infra", "MercadoPagoOptions")]
    public void MercadoPagoOptions_ShouldAllowSettingUserId()
    {
        // Arrange
        var options = new MercadoPagoOptions();
        var userId = "123456789";

        // Act
        options.UserId = userId;

        // Assert
        options.UserId.Should().Be(userId);
    }

    [Fact(DisplayName = "MercadoPagoOptions should allow setting AccessToken")]
    [Trait("Infra", "MercadoPagoOptions")]
    public void MercadoPagoOptions_ShouldAllowSettingAccessToken()
    {
        // Arrange
        var options = new MercadoPagoOptions();
        var accessToken = "TEST-1234567890-121212-abcdef123456";

        // Act
        options.AccessToken = accessToken;

        // Assert
        options.AccessToken.Should().Be(accessToken);
    }

    [Fact(DisplayName = "MercadoPagoOptions should allow setting both properties")]
    [Trait("Infra", "MercadoPagoOptions")]
    public void MercadoPagoOptions_ShouldAllowSettingBothProperties()
    {
        // Arrange
        var options = new MercadoPagoOptions
        {
            UserId = "987654321",
            AccessToken = "TEST-token-xyz"
        };

        // Assert
        options.UserId.Should().Be("987654321");
        options.AccessToken.Should().Be("TEST-token-xyz");
    }
}
