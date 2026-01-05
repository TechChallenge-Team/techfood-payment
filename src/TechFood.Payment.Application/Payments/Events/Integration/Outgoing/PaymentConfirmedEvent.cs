using System;
using TechFood.Shared.Application.Events;

namespace TechFood.Payment.Application.Payments.Events.Integration.Outgoing
{
    public class PaymentConfirmedEvent : IIntegrationEvent
    {
        public PaymentConfirmedEvent(
            Guid paymentId,
            Guid orderId)
        {
            PaymentId = paymentId;
            OrderId = orderId;
        }

        public Guid PaymentId { get; set; }

        public Guid OrderId { get; set; }
    }
}
