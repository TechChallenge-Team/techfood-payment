using System;
using System.Collections.Generic;

namespace TechFood.Payment.Application.Common.Dto.QrCode;

public class QrCodePaymentRequest
{
    public string PosId { get; set; }

    public Guid OrderId { get; set; }

    public string Title { get; set; }

    public decimal Amount { get; set; }

    public List<PaymentItem> Items { get; set; } = [];

    public QrCodePaymentRequest(string posId, Guid orderId, string title, decimal amount, List<PaymentItem> items)
    {
        PosId = posId;
        OrderId = orderId;
        Title = title;
        Amount = amount;
        Items = items;
    }
}
