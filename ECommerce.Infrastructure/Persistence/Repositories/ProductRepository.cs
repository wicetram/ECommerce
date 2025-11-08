using ECommerce.Application.Abstractions;
using ECommerce.Domain.Entities;

namespace ECommerce.Infrastructure.Persistence.Repositories;

// Şimdilik ürünleri DB’de persist etmiyoruz. Balance API’den okunuyor.
// Gerekirse cache/local persist ekleriz.
public sealed class ProductRepository : IProductRepository
{
    public Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken ct) => Task.FromResult<IReadOnlyList<Product>>([]);
}