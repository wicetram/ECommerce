using ECommerce.Domain.ValueObjects;

namespace ECommerce.Domain.Entities;

public sealed class Product
{
    public string Id { get; }
    public string Name { get; private set; }
    public Money Price { get; private set; }

    private Product() { } // EF Core

    public Product(string id, string name, Money price)
    {
        Id = id;
        Name = name;
        Price = price;
    }
}
