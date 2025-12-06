using System;
using TechFood.Shared.Domain.Enums;
using TechFood.Shared.Domain.Events;

namespace TechFood.Payment.Domain.Events.Payment;

public record class PaymentConfirmedEvent(
    Guid Id,
    Guid OrderId,
    DateTime PaidAt,
    PaymentType PaymentType,
    decimal Amount) : IDomainEvent;
