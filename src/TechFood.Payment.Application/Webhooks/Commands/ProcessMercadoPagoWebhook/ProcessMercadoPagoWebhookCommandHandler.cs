using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using TechFood.Payment.Application.Payments.Events.Integration.Outgoing;
using TechFood.Payment.Domain.Repositories;

namespace TechFood.Payment.Application.Webhooks.Commands.ProcessMercadoPagoWebhook;

public class ProcessMercadoPagoWebhookCommandHandler : IRequestHandler<ProcessMercadoPagoWebhookCommand, Unit>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IMediator _mediator;
    private readonly ILogger<ProcessMercadoPagoWebhookCommandHandler> _logger;

    public ProcessMercadoPagoWebhookCommandHandler(
        IPaymentRepository paymentRepository,
        IMediator mediator,
        ILogger<ProcessMercadoPagoWebhookCommandHandler> logger)
    {
        _paymentRepository = paymentRepository;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<Unit> Handle(ProcessMercadoPagoWebhookCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Processing MercadoPago webhook. Action: {Action}, Type: {Type}, DataId: {DataId}",
            request.Action, request.Type, request.DataId);

        if (!request.Type.Equals("payment", StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogWarning("Ignoring webhook with type: {Type}", request.Type);
            return Unit.Value;
        }

        if (!Guid.TryParse(request.DataId, out var orderId))
        {
            _logger.LogWarning("Invalid GUID format for DataId: {DataId}", request.DataId);
            return Unit.Value;
        }

        var payment = await _paymentRepository.GetByOrderIdAsync(orderId);

        if (payment == null)
        {
            _logger.LogWarning("Payment not found for OrderId: {OrderId}", orderId);
            return Unit.Value;
        }

        if (payment.IsConfirmed)
        {
            _logger.LogInformation("Payment {PaymentId} already confirmed", payment.Id);
            return Unit.Value;
        }

        payment.Confirm();

        _logger.LogInformation("Payment {PaymentId} confirmed for OrderId: {OrderId}", payment.Id, orderId);

        await _mediator.Publish(new PaymentConfirmedEvent(payment.Id, payment.OrderId), cancellationToken);

        return Unit.Value;
    }
}
