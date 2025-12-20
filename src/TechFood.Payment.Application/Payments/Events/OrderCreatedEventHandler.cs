using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using TechFood.Payment.Application.Payments.Commands.CreatePayment;
using TechFood.Shared.Application.Events;
using TechFood.Shared.Domain.Enums;

namespace TechFood.Payment.Application.Payments.Events;

public record OrderCreatedIntegrationEvent(
    Guid OrderId,
    List<OrderItemCreatedDto> Items
) : IIntegrationEvent;

public record OrderItemCreatedDto(
    Guid ProductId,
    string Name,
    decimal UnitPrice,
    int Quantity
);

public class OrderCreatedEventHandler(
    IMediator mediator,
    ILogger<OrderCreatedEventHandler> logger)
    : INotificationHandler<OrderCreatedIntegrationEvent>
{
    public async Task Handle(OrderCreatedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Order created event received for OrderId: {OrderId}", notification.OrderId);

        try
        {
            var command = new CreatePaymentCommand(
                notification.OrderId,
                PaymentType.MercadoPago);

            var result = await mediator.Send(command, cancellationToken);

            logger.LogInformation("Payment created successfully for OrderId: {OrderId}, PaymentId: {PaymentId}", 
                notification.OrderId, result.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating payment for OrderId: {OrderId}", notification.OrderId);
            throw;
        }
    }
}
