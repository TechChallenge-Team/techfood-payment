using System;
using MediatR;

namespace TechFood.Payment.Application.Payments.Commands.ConfirmPayment;

public record ConfirmPaymentCommand(Guid Id) : IRequest<Unit>;
