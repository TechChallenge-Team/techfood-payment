using System;
using MediatR;

namespace TechFood.Application.Payments.Commands.ConfirmPayment;

public record ConfirmPaymentCommand(Guid Id) : IRequest<Unit>;
