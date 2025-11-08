using ECommerce.Domain.Exceptions;
using ECommerce.Domain.ValueObjects;

namespace ECommerce.Domain.Entities;

public sealed class OrderItem
{
    public Guid ProductId { get; }
    public int Quantity { get; private set; }
    public Money UnitPrice { get; private set; }

    public Money Total => Money.Create(UnitPrice.Amount * Quantity, UnitPrice.Currency);

    private OrderItem() { }

    public OrderItem(Guid productId, int quantity, Money unitPrice)
    {
        if (quantity <= 0)
            throw new DomainException("Quantity must be greater than zero");

        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }
}