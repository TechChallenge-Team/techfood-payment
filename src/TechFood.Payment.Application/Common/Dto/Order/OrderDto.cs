using System;
using System.Collections.Generic;

namespace TechFood.Payment.Application.Common.Dto.Order
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public int Number { get; set; }
        public List<OrderItemResult> Items { get; set; } = [];
    }

    public class OrderItemResult
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
