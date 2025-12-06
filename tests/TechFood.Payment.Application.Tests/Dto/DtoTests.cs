using TechFood.Payment.Application.Common.Dto.Order;
using TechFood.Payment.Application.Common.Dto.Product;
using TechFood.Payment.Application.Common.Dto.QrCode;
using TechFood.Payment.Application.Payments.Dto;
using TechFood.Shared.Domain.Enums;

namespace TechFood.Payment.Application.Tests.Dto;

public class DtoTests
{
    [Fact(DisplayName = "OrderDto should be created with properties")]
    [Trait("Application", "DTOs")]
    public void OrderDto_ShouldBeCreatedWithProperties()
    {
        // Arrange & Act
        var dto = new OrderDto
        {
            Id = Guid.NewGuid(),
            Number = 123,
            Amount = 100.50m,
            Items = new List<OrderItemResult>
            {
                new OrderItemResult { Id = Guid.NewGuid(), Quantity = 2, Price = 50.25m }
            }
        };

        // Assert
        dto.Id.Should().NotBe(Guid.Empty);
        dto.Number.Should().Be(123);
        dto.Amount.Should().Be(100.50m);
        dto.Items.Should().HaveCount(1);
    }

    [Fact(DisplayName = "OrderItemResult should be created with properties")]
    [Trait("Application", "DTOs")]
    public void OrderItemResult_ShouldBeCreatedWithProperties()
    {
        // Arrange & Act
        var dto = new OrderItemResult
        {
            Id = Guid.NewGuid(),
            Quantity = 3,
            Price = 25.00m
        };

        // Assert
        dto.Id.Should().NotBe(Guid.Empty);
        dto.Quantity.Should().Be(3);
        dto.Price.Should().Be(25.00m);
    }

    [Fact(DisplayName = "ProductDto should have Id property")]
    [Trait("Application", "DTOs")]
    public void ProductDto_ShouldHaveIdProperty()
    {
        // Arrange & Act
        var dto = new ProductDto
        {
            Id = Guid.NewGuid()
        };

        // Assert
        dto.Id.Should().NotBe(Guid.Empty);
    }

    [Fact(DisplayName = "PaymentDto should be created with all properties")]
    [Trait("Application", "DTOs")]
    public void PaymentDto_ShouldBeCreatedWithAllProperties()
    {
        // Arrange
        var id = Guid.NewGuid();
        var orderId = Guid.NewGuid();
        var createdAt = DateTime.Now;
        var paidAt = DateTime.Now.AddMinutes(5);

        // Act
        var dto = new PaymentDto(
            id,
            orderId,
            createdAt,
            paidAt,
            PaymentType.MercadoPago,
            PaymentStatusType.Approved,
            150.00m,
            "QR_CODE_DATA");

        // Assert
        dto.Id.Should().Be(id);
        dto.OrderId.Should().Be(orderId);
        dto.CreatedAt.Should().Be(createdAt);
        dto.PaidAt.Should().Be(paidAt);
        dto.Type.Should().Be(PaymentType.MercadoPago);
        dto.Status.Should().Be(PaymentStatusType.Approved);
        dto.Amount.Should().Be(150.00m);
        dto.QrCodeData.Should().Be("QR_CODE_DATA");
    }

    [Fact(DisplayName = "PaymentDto should accept null QrCodeData")]
    [Trait("Application", "DTOs")]
    public void PaymentDto_ShouldAcceptNullQrCodeData()
    {
        // Arrange & Act
        var dto = new PaymentDto(
            Guid.NewGuid(),
            Guid.NewGuid(),
            DateTime.Now,
            null,
            PaymentType.CreditCard,
            PaymentStatusType.Pending,
            200.00m);

        // Assert
        dto.QrCodeData.Should().BeNull();
        dto.Type.Should().Be(PaymentType.CreditCard);
    }

    [Fact(DisplayName = "QrCodePaymentRequest should be created correctly")]
    [Trait("Application", "DTOs")]
    public void QrCodePaymentRequest_ShouldBeCreatedCorrectly()
    {
        // Arrange
        var posId = "POS001";
        var orderId = Guid.NewGuid();
        var title = "Order #123";
        var amount = 100.00m;
        var items = new List<Application.Common.Dto.PaymentItem>
        {
            new Application.Common.Dto.PaymentItem("Pizza", 2, "unit", 25.00m, 50.00m),
            new Application.Common.Dto.PaymentItem("Soda", 1, "unit", 5.00m, 5.00m)
        };

        // Act
        var request = new QrCodePaymentRequest(posId, orderId, title, amount, items);

        // Assert
        request.PosId.Should().Be(posId);
        request.OrderId.Should().Be(orderId);
        request.Title.Should().Be(title);
        request.Amount.Should().Be(amount);
        request.Items.Should().HaveCount(2);
    }

    [Fact(DisplayName = "QrCodePaymentResult should be created correctly")]
    [Trait("Application", "DTOs")]
    public void QrCodePaymentResult_ShouldBeCreatedCorrectly()
    {
        // Arrange
        var qrCodeId = "QR123";
        var qrCodeData = "QR_DATA_STRING";

        // Act
        var result = new QrCodePaymentResult(qrCodeId, qrCodeData);

        // Assert
        result.QrCodeId.Should().Be(qrCodeId);
        result.QrCodeData.Should().Be(qrCodeData);
    }

    [Fact(DisplayName = "PaymentItem should be created correctly")]
    [Trait("Application", "DTOs")]
    public void PaymentItem_ShouldBeCreatedCorrectly()
    {
        // Arrange & Act
        var item = new Application.Common.Dto.PaymentItem(
            "Burger",
            3,
            "unit",
            15.00m,
            45.00m);

        // Assert
        item.Title.Should().Be("Burger");
        item.Quantity.Should().Be(3);
        item.Unit.Should().Be("unit");
        item.UnitPrice.Should().Be(15.00m);
        item.Amount.Should().Be(45.00m);
    }
}
