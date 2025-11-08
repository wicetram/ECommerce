using ECommerce.Domain.Exceptions;

namespace ECommerce.Domain.Entities;

public sealed class Order
{
    public Guid Id { get; }
    public DateTime CreatedAt { get; private set; }
    public int Status { get; private set; } = 0;

    private readonly List<OrderItem> _items = [];

    public IReadOnlyCollection<OrderItem> Items => _items;

    private Order() { } // EF

    public Order(Guid id)
    {
        Id = id;
        CreatedAt = DateTime.UtcNow;
    }

    public void AddItem(OrderItem item)
    {
        _items.Add(item);
    }

    public void Complete()
    {
        if (Status != 0)
            throw new DomainException("Only pending orders can be completed");

        Status = 1;
    }
}