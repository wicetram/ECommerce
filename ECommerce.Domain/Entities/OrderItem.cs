using ECommerce.Domain.Exceptions;
using ECommerce.Domain.ValueObjects;

namespace ECommerce.Domain.Entities;

public sealed class OrderItem
{
    public string ProductId { get; private set; } = default!;
    public int Quantity { get; private set; }
    public Money UnitPrice { get; private set; } = default!;

    private OrderItem() { }

    public OrderItem(string productId, int quantity, Money unitPrice)
    {
        if (string.IsNullOrWhiteSpace(productId))
            throw new DomainException("ProductId is required.");
        if (quantity <= 0)
            throw new DomainException("Quantity must be greater than zero.");
        if (unitPrice is null)
            throw new DomainException("Unit price is required.");

        ProductId = productId.Trim();
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    public Money Total() => Money.Create(UnitPrice.Amount * Quantity, UnitPrice.Currency);
}