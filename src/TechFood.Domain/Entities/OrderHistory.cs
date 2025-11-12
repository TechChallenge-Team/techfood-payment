using System;
using TechFood.Shared.Domain.Entities;
using TechFood.Shared.Domain.Enums;

namespace TechFood.Domain.Entities;

public class OrderHistory : Entity
{
    private OrderHistory() { }

    public OrderHistory(
        OrderStatusType status
        )
    {
        Status = status;
        CreatedAt = DateTime.Now;
    }

    public DateTime CreatedAt { get; private set; }

    public OrderStatusType Status { get; private set; }
}
