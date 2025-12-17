using System;
using TechFood.Shared.Application.Events;

namespace TechFood.Payment.Application.Payments.Events
{
    public class ConfirmedPaymentEvent : IIntegrationEvent
    {
        public ConfirmedPaymentEvent(Guid orderId)
        {
            OrderId = orderId;
        }

        public Guid OrderId { get; set; }
    }
}
