using MediatR;
using Microsoft.AspNetCore.Mvc;
using TechFood.Payment.Api.Controllers;
using TechFood.Payment.Application.Payments.Commands.ConfirmPayment;
using TechFood.Payment.Application.Payments.Commands.CreatePayment;
using TechFood.Payment.Application.Payments.Dto;
using TechFood.Payment.Contracts.Payments;
using TechFood.Shared.Domain.Enums;

namespace TechFood.Payment.Api.Tests.Controllers;

public class PaymentsControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly PaymentsController _controller;

    public PaymentsControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new PaymentsController(_mediatorMock.Object);
    }

    [Fact(DisplayName = "CreateAsync should return Ok with created payment")]
    [Trait("Api", "PaymentsController")]
    public async Task CreateAsync_WithValidRequest_ShouldReturnOkWithPayment()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var request = new CreatePaymentRequest(orderId, PaymentType.MercadoPago);

        var expectedPayment = new PaymentDto(
            Guid.NewGuid(),
            orderId,
            DateTime.UtcNow,
            null,
            PaymentType.MercadoPago,
            PaymentStatusType.Pending,
            100.00m,
            "QR_CODE_DATA");

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<CreatePaymentCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedPayment);

        // Act
        var result = await _controller.CreateAsync(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().Be(expectedPayment);

        _mediatorMock.Verify(
            x => x.Send(
                It.Is<CreatePaymentCommand>(c => c.OrderId == orderId && c.Type == PaymentType.MercadoPago),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact(DisplayName = "CreateAsync should return NotFound when payment is not created")]
    [Trait("Api", "PaymentsController")]
    public async Task CreateAsync_WhenPaymentNotCreated_ShouldReturnNotFound()
    {
        // Arrange
        var request = new CreatePaymentRequest(Guid.NewGuid(), PaymentType.MercadoPago);

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<CreatePaymentCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((PaymentDto?)null);

        // Act
        var result = await _controller.CreateAsync(request);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact(DisplayName = "ConfirmAsync should return Ok")]
    [Trait("Api", "PaymentsController")]
    public async Task ConfirmAsync_WithValidId_ShouldReturnOk()
    {
        // Arrange
        var paymentId = Guid.NewGuid();

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<ConfirmPaymentCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        // Act
        var result = await _controller.ConfirmAsync(paymentId);

        // Assert
        result.Should().BeOfType<OkResult>();

        _mediatorMock.Verify(
            x => x.Send(
                It.Is<ConfirmPaymentCommand>(c => c.Id == paymentId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact(DisplayName = "CreateAsync should create payment with CreditCard type")]
    [Trait("Api", "PaymentsController")]
    public async Task CreateAsync_WithCreditCardType_ShouldReturnOk()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var request = new CreatePaymentRequest(orderId, PaymentType.CreditCard);

        var expectedPayment = new PaymentDto(
            Guid.NewGuid(),
            orderId,
            DateTime.UtcNow,
            null,
            PaymentType.CreditCard,
            PaymentStatusType.Pending,
            150.00m);

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<CreatePaymentCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedPayment);

        // Act
        var result = await _controller.CreateAsync(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        var payment = okResult!.Value as PaymentDto;
        payment.Should().NotBeNull();
        payment!.Type.Should().Be(PaymentType.CreditCard);
    }

    [Fact(DisplayName = "CreateAsync should handle different order IDs")]
    [Trait("Api", "PaymentsController")]
    public async Task CreateAsync_WithDifferentOrderIds_ShouldHandleCorrectly()
    {
        // Arrange
        var orderIds = new[] { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };

        foreach (var orderId in orderIds)
        {
            var request = new CreatePaymentRequest(orderId, PaymentType.MercadoPago);
            var expectedPayment = new PaymentDto(
                Guid.NewGuid(),
                orderId,
                DateTime.UtcNow,
                null,
                PaymentType.MercadoPago,
                PaymentStatusType.Pending,
                100.00m,
                "QR_CODE_DATA");

            _mediatorMock
                .Setup(x => x.Send(It.Is<CreatePaymentCommand>(c => c.OrderId == orderId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedPayment);

            // Act
            var result = await _controller.CreateAsync(request);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            var payment = okResult!.Value as PaymentDto;
            payment!.OrderId.Should().Be(orderId);
        }
    }
}
