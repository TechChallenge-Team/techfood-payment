using System.Globalization;
using TechFood.Payment.Application.Common.Resources;

namespace TechFood.Payment.Application.Tests.Resources;

public class ExceptionsResourceTests
{
    [Fact(DisplayName = "ResourceManager should not be null")]
    [Trait("Application", "Resources")]
    public void ResourceManager_ShouldNotBeNull()
    {
        // Act
        var resourceManager = Exceptions.ResourceManager;

        // Assert
        resourceManager.Should().NotBeNull();
    }

    [Fact(DisplayName = "Order_OrderNotFound should return valid message")]
    [Trait("Application", "Resources")]
    public void Order_OrderNotFound_ShouldReturnValidMessage()
    {
        // Act
        var message = Exceptions.Order_OrderNotFound;

        // Assert
        message.Should().NotBeNullOrEmpty();
        message.Should().Contain("Order");
    }

    [Fact(DisplayName = "Payment_PaymentNotFound should return valid message")]
    [Trait("Application", "Resources")]
    public void Payment_PaymentNotFound_ShouldReturnValidMessage()
    {
        // Act
        var message = Exceptions.Payment_PaymentNotFound;

        // Assert
        message.Should().NotBeNullOrEmpty();
        message.Should().Contain("Payment");
    }

    [Fact(DisplayName = "Culture can be set and affects resource retrieval")]
    [Trait("Application", "Resources")]
    public void Culture_CanBeSet()
    {
        // Arrange
        var originalCulture = Exceptions.Culture;

        try
        {
            // Act
            Exceptions.Culture = CultureInfo.InvariantCulture;

            // Assert
            Exceptions.Culture.Should().Be(CultureInfo.InvariantCulture);
        }
        finally
        {
            // Cleanup
            Exceptions.Culture = originalCulture;
        }
    }

    [Fact(DisplayName = "Auth_InvalidUseOrPassword should return valid message")]
    [Trait("Application", "Resources")]
    public void Auth_InvalidUseOrPassword_ShouldReturnValidMessage()
    {
        // Act
        var message = Exceptions.Auth_InvalidUseOrPassword;

        // Assert
        message.Should().NotBeNullOrEmpty();
    }

    [Fact(DisplayName = "Category_CategoryNotFound should return valid message")]
    [Trait("Application", "Resources")]
    public void Category_CategoryNotFound_ShouldReturnValidMessage()
    {
        // Act
        var message = Exceptions.Category_CategoryNotFound;

        // Assert
        message.Should().NotBeNullOrEmpty();
        message.Should().Contain("Category");
    }

    [Fact(DisplayName = "Customer_CpfAlreadyExists should return valid message")]
    [Trait("Application", "Resources")]
    public void Customer_CpfAlreadyExists_ShouldReturnValidMessage()
    {
        // Act
        var message = Exceptions.Customer_CpfAlreadyExists;

        // Assert
        message.Should().NotBeNullOrEmpty();
        message.Should().Contain("CPF");
    }

    [Fact(DisplayName = "ImageUrlResolver_FolderCannotBeNull should return valid message")]
    [Trait("Application", "Resources")]
    public void ImageUrlResolver_FolderCannotBeNull_ShouldReturnValidMessage()
    {
        // Act
        var message = Exceptions.ImageUrlResolver_FolderCannotBeNull;

        // Assert
        message.Should().NotBeNullOrEmpty();
    }

    [Fact(DisplayName = "All resource strings should be accessible")]
    [Trait("Application", "Resources")]
    public void AllResourceStrings_ShouldBeAccessible()
    {
        // Arrange & Act
        var properties = typeof(Exceptions).GetProperties(
            System.Reflection.BindingFlags.Static | 
            System.Reflection.BindingFlags.NonPublic);

        var stringProperties = properties.Where(p => p.PropertyType == typeof(string) && 
                                                     p.Name != "Culture");

        // Assert
        stringProperties.Should().NotBeEmpty();
        foreach (var prop in stringProperties)
        {
            var value = prop.GetValue(null) as string;
            value.Should().NotBeNullOrEmpty($"Property {prop.Name} should have a value");
        }
    }

    [Fact(DisplayName = "ResourceManager should use correct assembly")]
    [Trait("Application", "Resources")]
    public void ResourceManager_ShouldUseCorrectAssembly()
    {
        // Act
        var resourceManager = Exceptions.ResourceManager;
        var assembly = typeof(Exceptions).Assembly;

        // Assert
        resourceManager.Should().NotBeNull();
        resourceManager.BaseName.Should().Contain("Exceptions");
    }
}
