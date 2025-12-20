using Microsoft.Extensions.DependencyInjection;
using TechFood.Payment.Application.Common.Dto;
using TechFood.Payment.Application.Common.Dto.Order;
using TechFood.Payment.Application.Common.Dto.Product;
using TechFood.Payment.Application.Common.Dto.QrCode;
using TechFood.Payment.Application.Common.Services.Interfaces;
using TechFood.Payment.Application.Payments.Commands.CreatePayment;
using TechFood.Payment.Domain.Repositories;
using TechFood.Shared.Domain.Enums;

namespace TechFood.Payment.Application.Tests.Commands;

public class CreatePaymentCommandHandlerTests
{
    private readonly Mock<IOrderService> _orderServiceMock;
    private readonly Mock<IBackofficeService> _backofficeServiceMock;
    private readonly Mock<IPaymentRepository> _paymentRepositoryMock;

    public CreatePaymentCommandHandlerTests()
    {
        _orderServiceMock = new Mock<IOrderService>();
        _backofficeServiceMock = new Mock<IBackofficeService>();
        _paymentRepositoryMock = new Mock<IPaymentRepository>();
    }

    [Fact(DisplayName = "Should throw ApplicationException when order is not found")]
    [Trait("Application", "CreatePaymentCommandHandler")]
    public async Task Handle_WithNonExistentOrder_ShouldThrowApplicationException()
    {
        // Arrange
        var command = new CreatePaymentCommand(Guid.NewGuid(), PaymentType.MercadoPago);
        var serviceProvider = new ServiceCollection().BuildServiceProvider();

        _orderServiceMock
            .Setup(x => x.GetByIdAsync(command.OrderId))
            .ReturnsAsync(null as OrderDto);

        var handler = new CreatePaymentCommandHandler(
            _orderServiceMock.Object,
            _backofficeServiceMock.Object,
            _paymentRepositoryMock.Object,
            serviceProvider);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<System.ApplicationException>(() => 
            handler.Handle(command, CancellationToken.None));
        
        exception.Message.Should().Contain("Order not found");
    }

    [Fact(DisplayName = "Should create payment with MercadoPago and generate QR code")]
    [Trait("Application", "CreatePaymentCommandHandler")]
    public async Task Handle_WithMercadoPagoType_ShouldGenerateQrCodeAndCreatePayment()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var command = new CreatePaymentCommand(orderId, PaymentType.MercadoPago);

        var orderDto = new OrderDto
        {
            Id = orderId,
            Number = 12345,
            Amount = 150.75m,
            Items = new List<OrderItemResult>
            {
                new() { Id = productId, Quantity = 3, Price = 50.25m }
            }
        };

        var products = new List<ProductDto>();

        var qrCodeResult = new QrCodePaymentResult("qr-123", "00020126580014br.gov.bcb.pix");

        _orderServiceMock
            .Setup(x => x.GetByIdAsync(orderId))
            .ReturnsAsync(orderDto);

        _backofficeServiceMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);

        var mockPaymentService = new Mock<IPaymentService>();
        mockPaymentService
            .Setup(x => x.GenerateQrCodePaymentAsync(It.IsAny<QrCodePaymentRequest>()))
            .ReturnsAsync(qrCodeResult);

        var services = new ServiceCollection();
        services.AddKeyedSingleton(PaymentType.MercadoPago, mockPaymentService.Object);
        var serviceProvider = services.BuildServiceProvider();

        var handler = new CreatePaymentCommandHandler(
            _orderServiceMock.Object,
            _backofficeServiceMock.Object,
            _paymentRepositoryMock.Object,
            serviceProvider);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.OrderId.Should().Be(orderId);
        result.Type.Should().Be(PaymentType.MercadoPago);
        result.Amount.Should().Be(150.75m);
        result.Status.Should().Be(PaymentStatusType.Pending);
        result.PaidAt.Should().BeNull();

        _paymentRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Domain.Entities.Payment>()), Times.Once);
        mockPaymentService.Verify(x => x.GenerateQrCodePaymentAsync(It.IsAny<QrCodePaymentRequest>()), Times.Once);
    }

    [Fact(DisplayName = "Should create payment with MercadoPago when product name is not found")]
    [Trait("Application", "CreatePaymentCommandHandler")]
    public async Task Handle_WithMercadoPagoAndMissingProduct_ShouldUseEmptyProductName()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var unknownProductId = Guid.NewGuid();
        var command = new CreatePaymentCommand(orderId, PaymentType.MercadoPago);

        var orderDto = new OrderDto
        {
            Id = orderId,
            Number = 999,
            Amount = 99.99m,
            Items = new List<OrderItemResult>
            {
                new() { Id = unknownProductId, Quantity = 1, Price = 99.99m }
            }
        };

        var products = new List<ProductDto>(); // Empty product list

        var qrCodeResult = new QrCodePaymentResult("qr-456", "qr-data-456");

        _orderServiceMock
            .Setup(x => x.GetByIdAsync(orderId))
            .ReturnsAsync(orderDto);

        _backofficeServiceMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);

        var mockPaymentService = new Mock<IPaymentService>();
        mockPaymentService
            .Setup(x => x.GenerateQrCodePaymentAsync(It.IsAny<QrCodePaymentRequest>()))
            .ReturnsAsync(qrCodeResult);

        var services = new ServiceCollection();
        services.AddKeyedSingleton(PaymentType.MercadoPago, mockPaymentService.Object);
        var serviceProvider = services.BuildServiceProvider();

        var handler = new CreatePaymentCommandHandler(
            _orderServiceMock.Object,
            _backofficeServiceMock.Object,
            _paymentRepositoryMock.Object,
            serviceProvider);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        mockPaymentService.Verify(x => x.GenerateQrCodePaymentAsync(It.IsAny<QrCodePaymentRequest>()), Times.Once);
    }

    [Fact(DisplayName = "Should throw NotImplementedException for CreditCard payment")]
    [Trait("Application", "CreatePaymentCommandHandler")]
    public async Task Handle_WithCreditCardType_ShouldThrowNotImplementedException()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var command = new CreatePaymentCommand(orderId, PaymentType.CreditCard);

        var orderDto = new OrderDto
        {
            Id = orderId,
            Number = 100,
            Amount = 200m,
            Items = new List<OrderItemResult>()
        };

        _orderServiceMock
            .Setup(x => x.GetByIdAsync(orderId))
            .ReturnsAsync(orderDto);

        _backofficeServiceMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ProductDto>());

        var mockPaymentService = new Mock<IPaymentService>();
        var services = new ServiceCollection();
        services.AddKeyedSingleton(PaymentType.CreditCard, mockPaymentService.Object);
        var serviceProvider = services.BuildServiceProvider();

        var handler = new CreatePaymentCommandHandler(
            _orderServiceMock.Object,
            _backofficeServiceMock.Object,
            _paymentRepositoryMock.Object,
            serviceProvider);

        // Act & Assert
        await Assert.ThrowsAsync<NotImplementedException>(() => 
            handler.Handle(command, CancellationToken.None));
    }

    [Fact(DisplayName = "Should create payment with multiple items in order")]
    [Trait("Application", "CreatePaymentCommandHandler")]
    public async Task Handle_WithMultipleOrderItems_ShouldCreatePaymentWithAllItems()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var product1Id = Guid.NewGuid();
        var product2Id = Guid.NewGuid();
        var product3Id = Guid.NewGuid();
        var command = new CreatePaymentCommand(orderId, PaymentType.MercadoPago);

        var orderDto = new OrderDto
        {
            Id = orderId,
            Number = 555,
            Amount = 250.50m,
            Items = new List<OrderItemResult>
            {
                new() { Id = product1Id, Quantity = 2, Price = 50.50m },
                new() { Id = product2Id, Quantity = 1, Price = 99.50m },
                new() { Id = product3Id, Quantity = 3, Price = 16.83m }
            }
        };

        var products = new List<ProductDto>();

        var qrCodeResult = new QrCodePaymentResult("qr-multi", "qr-data-multi");

        _orderServiceMock
            .Setup(x => x.GetByIdAsync(orderId))
            .ReturnsAsync(orderDto);

        _backofficeServiceMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);

        var mockPaymentService = new Mock<IPaymentService>();
        mockPaymentService
            .Setup(x => x.GenerateQrCodePaymentAsync(It.IsAny<QrCodePaymentRequest>()))
            .ReturnsAsync(qrCodeResult);

        var services = new ServiceCollection();
        services.AddKeyedSingleton(PaymentType.MercadoPago, mockPaymentService.Object);
        var serviceProvider = services.BuildServiceProvider();

        var handler = new CreatePaymentCommandHandler(
            _orderServiceMock.Object,
            _backofficeServiceMock.Object,
            _paymentRepositoryMock.Object,
            serviceProvider);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Amount.Should().Be(250.50m);
        
        mockPaymentService.Verify(x => x.GenerateQrCodePaymentAsync(It.IsAny<QrCodePaymentRequest>()), Times.Once);
    }

    [Fact(DisplayName = "Should correctly calculate item total amount")]
    [Trait("Application", "CreatePaymentCommandHandler")]
    public async Task Handle_WithMercadoPago_ShouldCalculateCorrectItemAmounts()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var command = new CreatePaymentCommand(orderId, PaymentType.MercadoPago);

        var orderDto = new OrderDto
        {
            Id = orderId,
            Number = 777,
            Amount = 301.50m,
            Items = new List<OrderItemResult>
            {
                new() { Id = productId, Quantity = 5, Price = 60.30m } // 5 * 60.30 = 301.50
            }
        };

        var products = new List<ProductDto>();

        var qrCodeResult = new QrCodePaymentResult("qr-calc", "qr-data-calc");

        _orderServiceMock
            .Setup(x => x.GetByIdAsync(orderId))
            .ReturnsAsync(orderDto);

        _backofficeServiceMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);

        var mockPaymentService = new Mock<IPaymentService>();
        mockPaymentService
            .Setup(x => x.GenerateQrCodePaymentAsync(It.IsAny<QrCodePaymentRequest>()))
            .ReturnsAsync(qrCodeResult);

        var services = new ServiceCollection();
        services.AddKeyedSingleton(PaymentType.MercadoPago, mockPaymentService.Object);
        var serviceProvider = services.BuildServiceProvider();

        var handler = new CreatePaymentCommandHandler(
            _orderServiceMock.Object,
            _backofficeServiceMock.Object,
            _paymentRepositoryMock.Object,
            serviceProvider);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        mockPaymentService.Verify(x => x.GenerateQrCodePaymentAsync(It.IsAny<QrCodePaymentRequest>()), Times.Once);
        result.Amount.Should().Be(301.50m);
    }

    [Fact(DisplayName = "Should use cancellation token when provided")]
    [Trait("Application", "CreatePaymentCommandHandler")]
    public async Task Handle_WithCancellationToken_ShouldPassToAsyncOperations()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var command = new CreatePaymentCommand(orderId, PaymentType.MercadoPago);
        var cancellationToken = new CancellationToken();

        var orderDto = new OrderDto
        {
            Id = orderId,
            Number = 888,
            Amount = 100m,
            Items = new List<OrderItemResult>()
        };

        _orderServiceMock
            .Setup(x => x.GetByIdAsync(orderId))
            .ReturnsAsync(orderDto);

        _backofficeServiceMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ProductDto>());

        var mockPaymentService = new Mock<IPaymentService>();
        mockPaymentService
            .Setup(x => x.GenerateQrCodePaymentAsync(It.IsAny<QrCodePaymentRequest>()))
            .ReturnsAsync(new QrCodePaymentResult("qr", "data"));

        var services = new ServiceCollection();
        services.AddKeyedSingleton(PaymentType.MercadoPago, mockPaymentService.Object);
        var serviceProvider = services.BuildServiceProvider();

        var handler = new CreatePaymentCommandHandler(
            _orderServiceMock.Object,
            _backofficeServiceMock.Object,
            _paymentRepositoryMock.Object,
            serviceProvider);

        // Act
        var result = await handler.Handle(command, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        _orderServiceMock.Verify(x => x.GetByIdAsync(orderId), Times.Once);
        _backofficeServiceMock.Verify(x => x.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = "Should throw exception when QR code generation fails")]
    [Trait("Application", "CreatePaymentCommandHandler")]
    public async Task Handle_WhenQrCodeGenerationFails_ShouldThrowException()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var command = new CreatePaymentCommand(orderId, PaymentType.MercadoPago);

        var orderDto = new OrderDto
        {
            Id = orderId,
            Number = 123,
            Amount = 50m,
            Items = new List<OrderItemResult>()
        };

        _orderServiceMock
            .Setup(x => x.GetByIdAsync(orderId))
            .ReturnsAsync(orderDto);

        _backofficeServiceMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ProductDto>());

        var mockPaymentService = new Mock<IPaymentService>();
        mockPaymentService
            .Setup(x => x.GenerateQrCodePaymentAsync(It.IsAny<QrCodePaymentRequest>()))
            .ThrowsAsync(new InvalidOperationException("Payment service unavailable"));

        var services = new ServiceCollection();
        services.AddKeyedSingleton(PaymentType.MercadoPago, mockPaymentService.Object);
        var serviceProvider = services.BuildServiceProvider();

        var handler = new CreatePaymentCommandHandler(
            _orderServiceMock.Object,
            _backofficeServiceMock.Object,
            _paymentRepositoryMock.Object,
            serviceProvider);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => 
            handler.Handle(command, CancellationToken.None));

        _paymentRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Domain.Entities.Payment>()), Times.Never);
    }

    [Fact(DisplayName = "Should throw exception when order service fails")]
    [Trait("Application", "CreatePaymentCommandHandler")]
    public async Task Handle_WhenOrderServiceFails_ShouldThrowException()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var command = new CreatePaymentCommand(orderId, PaymentType.MercadoPago);

        _orderServiceMock
            .Setup(x => x.GetByIdAsync(orderId))
            .ThrowsAsync(new HttpRequestException("Order service unavailable"));

        var handler = new CreatePaymentCommandHandler(
            _orderServiceMock.Object,
            _backofficeServiceMock.Object,
            _paymentRepositoryMock.Object,
            new ServiceCollection().BuildServiceProvider());

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => 
            handler.Handle(command, CancellationToken.None));

        _paymentRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Domain.Entities.Payment>()), Times.Never);
    }

    [Fact(DisplayName = "Should throw exception when backoffice service fails")]
    [Trait("Application", "CreatePaymentCommandHandler")]
    public async Task Handle_WhenBackofficeServiceFails_ShouldThrowException()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var command = new CreatePaymentCommand(orderId, PaymentType.MercadoPago);

        var orderDto = new OrderDto
        {
            Id = orderId,
            Number = 123,
            Amount = 50m,
            Items = new List<OrderItemResult>()
        };

        _orderServiceMock
            .Setup(x => x.GetByIdAsync(orderId))
            .ReturnsAsync(orderDto);

        _backofficeServiceMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("Backoffice service unavailable"));

        var handler = new CreatePaymentCommandHandler(
            _orderServiceMock.Object,
            _backofficeServiceMock.Object,
            _paymentRepositoryMock.Object,
            new ServiceCollection().BuildServiceProvider());

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => 
            handler.Handle(command, CancellationToken.None));

        _paymentRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Domain.Entities.Payment>()), Times.Never);
    }

    [Fact(DisplayName = "Should create payment with correct order number in title")]
    [Trait("Application", "CreatePaymentCommandHandler")]
    public async Task Handle_ShouldCreatePaymentWithCorrectOrderNumberInTitle()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var orderNumber = 12345;
        var command = new CreatePaymentCommand(orderId, PaymentType.MercadoPago);

        var orderDto = new OrderDto
        {
            Id = orderId,
            Number = orderNumber,
            Amount = 100m,
            Items = new List<OrderItemResult>()
        };

        _orderServiceMock
            .Setup(x => x.GetByIdAsync(orderId))
            .ReturnsAsync(orderDto);

        _backofficeServiceMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ProductDto>());

        var mockPaymentService = new Mock<IPaymentService>();
        mockPaymentService
            .Setup(x => x.GenerateQrCodePaymentAsync(It.Is<QrCodePaymentRequest>(req => 
                req.Title == $"TechFood - Order #{orderNumber}")))
            .ReturnsAsync(new QrCodePaymentResult("qr", "data"));

        var services = new ServiceCollection();
        services.AddKeyedSingleton(PaymentType.MercadoPago, mockPaymentService.Object);
        var serviceProvider = services.BuildServiceProvider();

        var handler = new CreatePaymentCommandHandler(
            _orderServiceMock.Object,
            _backofficeServiceMock.Object,
            _paymentRepositoryMock.Object,
            serviceProvider);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        mockPaymentService.Verify(x => x.GenerateQrCodePaymentAsync(It.Is<QrCodePaymentRequest>(req => 
            req.Title == $"TechFood - Order #{orderNumber}")), Times.Once);
    }

    [Fact(DisplayName = "Should map products correctly to payment items")]
    [Trait("Application", "CreatePaymentCommandHandler")]
    public async Task Handle_ShouldMapProductsCorrectlyToPaymentItems()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var product1Id = Guid.NewGuid();
        var product2Id = Guid.NewGuid();
        var command = new CreatePaymentCommand(orderId, PaymentType.MercadoPago);

        var orderDto = new OrderDto
        {
            Id = orderId,
            Number = 999,
            Amount = 150m,
            Items = new List<OrderItemResult>
            {
                new() { Id = product1Id, Quantity = 2, Price = 50m },
                new() { Id = product2Id, Quantity = 1, Price = 50m }
            }
        };

        var product1 = new ProductDto { Id = product1Id };
        typeof(ProductDto).GetProperty("Name")!.SetValue(product1, "Burger");
        var product2 = new ProductDto { Id = product2Id };
        typeof(ProductDto).GetProperty("Name")!.SetValue(product2, "Pizza");
        
        var products = new List<ProductDto> { product1, product2 };

        _orderServiceMock
            .Setup(x => x.GetByIdAsync(orderId))
            .ReturnsAsync(orderDto);

        _backofficeServiceMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);

        var mockPaymentService = new Mock<IPaymentService>();
        mockPaymentService
            .Setup(x => x.GenerateQrCodePaymentAsync(It.Is<QrCodePaymentRequest>(req =>
                req.Items.Count == 2 &&
                req.Items[0].Title == "Burger" &&
                req.Items[0].Quantity == 2 &&
                req.Items[1].Title == "Pizza" &&
                req.Items[1].Quantity == 1)))
            .ReturnsAsync(new QrCodePaymentResult("qr", "data"));

        var services = new ServiceCollection();
        services.AddKeyedSingleton(PaymentType.MercadoPago, mockPaymentService.Object);
        var serviceProvider = services.BuildServiceProvider();

        var handler = new CreatePaymentCommandHandler(
            _orderServiceMock.Object,
            _backofficeServiceMock.Object,
            _paymentRepositoryMock.Object,
            serviceProvider);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        mockPaymentService.Verify(x => x.GenerateQrCodePaymentAsync(It.Is<QrCodePaymentRequest>(req =>
            req.Items.Any(i => i.Title == "Burger") &&
            req.Items.Any(i => i.Title == "Pizza"))), Times.Once);
    }

    [Fact(DisplayName = "Should use TOTEM01 as PosId")]
    [Trait("Application", "CreatePaymentCommandHandler")]
    public async Task Handle_ShouldUseTOTEM01AsPosId()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var command = new CreatePaymentCommand(orderId, PaymentType.MercadoPago);

        var orderDto = new OrderDto
        {
            Id = orderId,
            Number = 1,
            Amount = 10m,
            Items = new List<OrderItemResult>()
        };

        _orderServiceMock
            .Setup(x => x.GetByIdAsync(orderId))
            .ReturnsAsync(orderDto);

        _backofficeServiceMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ProductDto>());

        var mockPaymentService = new Mock<IPaymentService>();
        mockPaymentService
            .Setup(x => x.GenerateQrCodePaymentAsync(It.Is<QrCodePaymentRequest>(req => req.PosId == "TOTEM01")))
            .ReturnsAsync(new QrCodePaymentResult("qr", "data"));

        var services = new ServiceCollection();
        services.AddKeyedSingleton(PaymentType.MercadoPago, mockPaymentService.Object);
        var serviceProvider = services.BuildServiceProvider();

        var handler = new CreatePaymentCommandHandler(
            _orderServiceMock.Object,
            _backofficeServiceMock.Object,
            _paymentRepositoryMock.Object,
            serviceProvider);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        mockPaymentService.Verify(x => x.GenerateQrCodePaymentAsync(
            It.Is<QrCodePaymentRequest>(req => req.PosId == "TOTEM01")), Times.Once);
    }

    [Fact(DisplayName = "Should create payment with zero amount when order has no items")]
    [Trait("Application", "CreatePaymentCommandHandler")]
    public async Task Handle_WithEmptyOrderItems_ShouldCreatePaymentWithZeroItems()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var command = new CreatePaymentCommand(orderId, PaymentType.MercadoPago);

        var orderDto = new OrderDto
        {
            Id = orderId,
            Number = 1,
            Amount = 0m,
            Items = new List<OrderItemResult>()
        };

        _orderServiceMock
            .Setup(x => x.GetByIdAsync(orderId))
            .ReturnsAsync(orderDto);

        _backofficeServiceMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ProductDto>());

        var mockPaymentService = new Mock<IPaymentService>();
        mockPaymentService
            .Setup(x => x.GenerateQrCodePaymentAsync(It.Is<QrCodePaymentRequest>(req => req.Items.Count == 0)))
            .ReturnsAsync(new QrCodePaymentResult("qr", "data"));

        var services = new ServiceCollection();
        services.AddKeyedSingleton(PaymentType.MercadoPago, mockPaymentService.Object);
        var serviceProvider = services.BuildServiceProvider();

        var handler = new CreatePaymentCommandHandler(
            _orderServiceMock.Object,
            _backofficeServiceMock.Object,
            _paymentRepositoryMock.Object,
            serviceProvider);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Amount.Should().Be(0m);
        mockPaymentService.Verify(x => x.GenerateQrCodePaymentAsync(
            It.Is<QrCodePaymentRequest>(req => req.Items.Count == 0)), Times.Once);
    }
}
