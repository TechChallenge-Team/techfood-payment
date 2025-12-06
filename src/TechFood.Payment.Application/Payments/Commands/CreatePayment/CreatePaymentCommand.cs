using System;
using MediatR;
using TechFood.Payment.Application.Payments.Dto;
using TechFood.Shared.Domain.Enums;

namespace TechFood.Payment.Application.Payments.Commands.CreatePayment;

public record CreatePaymentCommand(Guid OrderId, PaymentType Type) : IRequest<PaymentDto>;
