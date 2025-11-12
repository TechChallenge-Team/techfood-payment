using System;
using System.Collections.Generic;
using TechFood.Shared.Domain.Entities;
using TechFood.Shared.Domain.Enums;

namespace TechFood.Domain.Entities;

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

    private readonly List<OrderItem> _items = [];

    private readonly List<OrderHistory> _historical = [];

    public int Number { get; private set; }

    public Guid? CustomerId { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public OrderStatusType Status { get; private set; }

    public decimal Amount { get; private set; }

    public decimal Discount { get; private set; }

    public IReadOnlyCollection<OrderItem> Items;

    public IReadOnlyCollection<OrderHistory> Historical;
}
