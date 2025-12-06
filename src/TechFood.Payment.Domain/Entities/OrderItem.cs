using System;
using TechFood.Domain.Resources;
using TechFood.Shared.Domain.Entities;
using TechFood.Shared.Domain.Validations;

namespace TechFood.Payment.Domain.Entities;

public class OrderItem : Entity
{
    private OrderItem() { }

    public OrderItem(
        Guid productId,
        decimal unitPrice,
        int quantity)
    {
        ProductId = productId;
        UnitPrice = unitPrice;
        Quantity = quantity;

        Validate();
    }

    public Guid ProductId { get; private set; }

    public decimal UnitPrice { get; private set; }

    public int Quantity { get; private set; }

    public void UpdateQuantity(int quantity)
    {
        Validations.ThrowIsGreaterThanZero(quantity, Exceptions.OrderItem_ThrowQuantityGreaterThanZero);

        Quantity = quantity;
    }

    private void Validate()
    {
        Validations.ThrowValidGuid(ProductId, Exceptions.OrderItem_ThrowProductIdIsInvalid);
        Validations.ThrowIsGreaterThanZero(UnitPrice, Exceptions.OrderItem_ThrowUnitPriceGreaterThanZero);
        Validations.ThrowIsGreaterThanZero(Quantity, Exceptions.OrderItem_ThrowQuantityGreaterThanZero);
    }
}
