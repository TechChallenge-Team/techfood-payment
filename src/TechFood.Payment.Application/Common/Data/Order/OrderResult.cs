using System;
using System.Collections.Generic;

namespace TechFood.Payment.Application.Common.Data.Order
{
    public class OrderResult
    {
        public Guid Id { get; private set; }
        public decimal Amount { get; private set; }
        public int Number { get; private set; }
        public List<OrderItemResult> Items { get; private set; }
    }

    public class OrderItemResult
    {
        public Guid Id { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
    }
}
