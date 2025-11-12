using System;
using TechFood.Shared.Domain.Enums;
using TechFood.Shared.Domain.Events;

namespace TechFood.Domain.Events.Payment;

public record class PaymentCreatedEvent(
    Guid Id,
    Guid OrderId,
    DateTime CreatedAt,
    PaymentType PaymentType,
    decimal Amount) : IDomainEvent;
