using System;
using TechFood.Shared.Application.Events;

namespace TechFood.Payment.Application.Payments.Events
{
    public class PaymentConfirmedIntegrationEvent : IIntegrationEvent
    {
        public PaymentConfirmedIntegrationEvent(
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
