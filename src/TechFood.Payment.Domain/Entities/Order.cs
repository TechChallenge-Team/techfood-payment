using System;
using System.Collections.Generic;
using TechFood.Shared.Domain.Entities;
using TechFood.Shared.Domain.Enums;

namespace TechFood.Payment.Domain.Entities;

public class Order : Entity, IAggregateRoot
{
    private Order() { }

    public Order(
        int number,
        Guid? customerId = null)
    {
        Number = number;
        CustomerId = customerId;
        CreatedAt = DateTime.Now;
        Status = OrderStatusType.Pending;
    }

    public int Number { get; private set; }

    public Guid? CustomerId { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public OrderStatusType Status { get; private set; }

    public decimal Amount { get; private set; }

    public decimal Discount { get; private set; }

    public IEnumerable<OrderItem> Items;

    public IEnumerable<OrderHistory> Historical;
}
