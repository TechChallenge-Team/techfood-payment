using System;
using TechFood.Shared.Domain.Enums;
using TechFood.Shared.Domain.Events;

namespace TechFood.Domain.Events.Payment;

public record class PaymentRefusedEvent(
    Guid Id,
    Guid OrderId,
    DateTime RefusedAt,
    PaymentType Type,
    decimal Amount) : IDomainEvent;
