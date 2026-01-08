using System;
using MediatR;

namespace TechFood.Payment.Application.Webhooks.Commands.ProcessMercadoPagoWebhook;

public record ProcessMercadoPagoWebhookCommand(
    string Action,
    string Type,
    string DataId,
    long UserId
) : IRequest<Unit>;
