using System;
using TechFood.Domain.Events.Payment;
using TechFood.Shared.Domain.Entities;
using TechFood.Shared.Domain.Enums;
using TechFood.Shared.Domain.Exceptions;

namespace TechFood.Domain.Entities;

public class Payment : Entity, IAggregateRoot
{
    private Payment() { }

    public Payment(
        Guid orderId,
        PaymentType type,
        decimal amount)
    {
        OrderId = orderId;
        Type = type;
        Amount = amount;
        CreatedAt = DateTime.Now;
        Status = PaymentStatusType.Pending;

        _events.Add(new PaymentCreatedEvent(
            Id,
            OrderId,
            CreatedAt,
            Type,
            Amount));
    }

    public Guid OrderId { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? PaidAt { get; private set; }

    public PaymentType Type { get; private set; }

    public PaymentStatusType Status { get; private set; }

    public decimal Amount { get; private set; }

    public void Confirm()
    {
        if (PaidAt.HasValue)
        {
            throw new DomainException(Resources.Exceptions.Payment_AlreadyPaid);
        }

        PaidAt = DateTime.Now;
        Status = PaymentStatusType.Approved;

        _events.Add(new PaymentConfirmedEvent(
            Id,
            OrderId,
            CreatedAt,
            Type,
            Amount));
    }

    public void Refused()
    {
        if (PaidAt.HasValue)
        {
            throw new DomainException(Resources.Exceptions.Payment_AlreadyPaid);
        }

        Status = PaymentStatusType.Refused;

        _events.Add(new PaymentRefusedEvent(
            Id,
            OrderId,
            DateTime.Now,
            Type,
            Amount));
    }
}
