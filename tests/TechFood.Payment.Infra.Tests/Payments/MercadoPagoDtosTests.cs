using TechFood.Payment.Infra.Payments.MercadoPago;

namespace TechFood.Payment.Infra.Tests.Payments;

[Trait("Category", "Unit")]
[Trait("Infra", "MercadoPago")]
public class MercadoPagoDtosTests
{
    [Fact]
    public void OrderRequest_Should_CreateInstance_WithValidData()
    {
        // Arrange
        var type = OrderType.QR;
        var externalReference = Guid.NewGuid().ToString();
        var description = "Order #123";
        var totalAmount = "100.50";
        var config = new OrderConfig(new OrderQRConfig("POS001", OrderQRConfigMode.Dynamic));
        var transactions = new OrderTransaction([new OrderPayment("100.50")]);
        var items = new List<OrderItem>
        {
            new("Pizza", 1, "unit", "50.00"),
            new("Soda", 2, "unit", "25.25")
        };

        // Act
        var orderRequest = new OrderRequest(type, externalReference, description, totalAmount, config, transactions, items);

        // Assert
        orderRequest.Should().NotBeNull();
        orderRequest.Type.Should().Be(type);
        orderRequest.ExternalReference.Should().Be(externalReference);
        orderRequest.Description.Should().Be(description);
        orderRequest.TotalAmount.Should().Be(totalAmount);
        orderRequest.Config.Should().Be(config);
        orderRequest.Transactions.Should().Be(transactions);
        orderRequest.Items.Should().HaveCount(2);
    }

    [Fact]
    public void OrderItem_Should_CreateInstance_WithValidData()
    {
        // Arrange
        var title = "Hamburguer";
        var quantity = 2;
        var unitMeasure = "unit";
        var unitPrice = "25.50";

        // Act
        var orderItem = new OrderItem(title, quantity, unitMeasure, unitPrice);

        // Assert
        orderItem.Should().NotBeNull();
        orderItem.Title.Should().Be(title);
        orderItem.Quantity.Should().Be(quantity);
        orderItem.UnitMeasure.Should().Be(unitMeasure);
        orderItem.UnitPrice.Should().Be(unitPrice);
    }

    [Theory]
    [InlineData("Pizza", 1, "unit", "10.50")]
    [InlineData("Soda", 5, "unit", "5.00")]
    [InlineData("Fries", 3, "kg", "15.75")]
    public void OrderItem_Should_CreateInstance_WithDifferentValues(string title, int quantity, string unitMeasure, string unitPrice)
    {
        // Act
        var orderItem = new OrderItem(title, quantity, unitMeasure, unitPrice);

        // Assert
        orderItem.Title.Should().Be(title);
        orderItem.Quantity.Should().Be(quantity);
        orderItem.UnitMeasure.Should().Be(unitMeasure);
        orderItem.UnitPrice.Should().Be(unitPrice);
    }

    [Fact]
    public void OrderTransaction_Should_CreateInstance_WithPayments()
    {
        // Arrange
        var payments = new List<OrderPayment>
        {
            new("50.00"),
            new("25.50")
        };

        // Act
        var transaction = new OrderTransaction(payments);

        // Assert
        transaction.Should().NotBeNull();
        transaction.Payments.Should().HaveCount(2);
        transaction.Payments.Should().Contain(p => p.Amount == "50.00");
        transaction.Payments.Should().Contain(p => p.Amount == "25.50");
    }

    [Fact]
    public void OrderPayment_Should_CreateInstance_WithAmount()
    {
        // Arrange
        var amount = "100.00";

        // Act
        var payment = new OrderPayment(amount);

        // Assert
        payment.Should().NotBeNull();
        payment.Amount.Should().Be(amount);
    }

    [Fact]
    public void OrderConfig_Should_CreateInstance_WithQrConfig()
    {
        // Arrange
        var qrConfig = new OrderQRConfig("POS123", OrderQRConfigMode.Static);

        // Act
        var config = new OrderConfig(qrConfig);

        // Assert
        config.Should().NotBeNull();
        config.Qr.Should().Be(qrConfig);
    }

    [Fact]
    public void OrderConfig_Should_CreateInstance_WithNullQrConfig()
    {
        // Act
        var config = new OrderConfig(null);

        // Assert
        config.Should().NotBeNull();
        config.Qr.Should().BeNull();
    }

    [Fact]
    public void OrderQRConfig_Should_CreateInstance_WithValidData()
    {
        // Arrange
        var externalPosId = "POS456";
        var mode = OrderQRConfigMode.Dynamic;

        // Act
        var qrConfig = new OrderQRConfig(externalPosId, mode);

        // Assert
        qrConfig.Should().NotBeNull();
        qrConfig.ExternalPosId.Should().Be(externalPosId);
        qrConfig.Mode.Should().Be(mode);
    }

    [Fact]
    public void OrderQRConfig_Should_CreateInstance_WithStaticMode()
    {
        // Arrange
        var externalPosId = "POS789";
        var mode = OrderQRConfigMode.Static;

        // Act
        var qrConfig = new OrderQRConfig(externalPosId, mode);

        // Assert
        qrConfig.Mode.Should().Be(mode);
    }

    [Fact]
    public void OrderQRConfig_Should_CreateInstance_WithDynamicMode()
    {
        // Arrange
        var externalPosId = "POS999";
        var mode = OrderQRConfigMode.Dynamic;

        // Act
        var qrConfig = new OrderQRConfig(externalPosId, mode);

        // Assert
        qrConfig.Mode.Should().Be(mode);
    }

    [Fact]
    public void OrderResult_Should_CreateInstance_WithValidData()
    {
        // Arrange
        var id = "order-123";
        var typeResponse = new OrderTypeResponse("qr-data-string");

        // Act
        var result = new OrderResult(id, typeResponse);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(id);
        result.TypeResponse.Should().Be(typeResponse);
    }

    [Fact]
    public void OrderTypeResponse_Should_CreateInstance_WithQrData()
    {
        // Arrange
        var qrData = "00020101021243650016COM.MERCADOLIBRE";

        // Act
        var response = new OrderTypeResponse(qrData);

        // Assert
        response.Should().NotBeNull();
        response.QrData.Should().Be(qrData);
    }

    [Fact]
    public void ErrorResult_Should_CreateInstance_WithErrors()
    {
        // Arrange
        var errors = new List<ErrorData>
        {
            new("ERR001", "Invalid payment"),
            new("ERR002", "Insufficient funds")
        };

        // Act
        var errorResult = new ErrorResult(errors);

        // Assert
        errorResult.Should().NotBeNull();
        errorResult.Errors.Should().HaveCount(2);
    }

    [Fact]
    public void ErrorData_Should_CreateInstance_WithCodeAndMessage()
    {
        // Arrange
        var code = "ERR123";
        var message = "An error occurred";

        // Act
        var error = new ErrorData(code, message);

        // Assert
        error.Should().NotBeNull();
        error.Code.Should().Be(code);
        error.Message.Should().Be(message);
    }

    [Fact]
    public void Records_Should_SupportEquality()
    {
        // Arrange
        var item1 = new OrderItem("Pizza", 1, "unit", "10.00");
        var item2 = new OrderItem("Pizza", 1, "unit", "10.00");

        // Assert
        item1.Should().Be(item2);
        item1.GetHashCode().Should().Be(item2.GetHashCode());
    }

    [Fact]
    public void Records_Should_NotBeEqual_WhenPropertiesDiffer()
    {
        // Arrange
        var item1 = new OrderItem("Pizza", 1, "unit", "10.00");
        var item2 = new OrderItem("Burger", 1, "unit", "10.00");

        // Assert
        item1.Should().NotBe(item2);
    }
}
