using ECommerce.Application.Abstractions;
using ECommerce.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace ECommerce.Infrastructure.Persistence.Repositories;

public sealed class OrderMemoryRepository(IMemoryCache cache) : IOrderRepository
{
    private static string Key(string id) => $"order:{id:N}";
    private static readonly MemoryCacheEntryOptions Entry = new() { SlidingExpiration = TimeSpan.FromHours(12) };

    public Task<Order?> GetByIdAsync(string id, CancellationToken ct)
    {
        cache.TryGetValue(Key(id), out Order? order);
        return Task.FromResult(order);
    }

    public Task<bool> AddAsync(Order order, CancellationToken ct)
    {
        cache.Set(Key(order.Id), order, Entry);
        return Task.FromResult(true);
    }

    public Task<bool> UpdateAsync(Order order, CancellationToken ct)
    {
        // upsert: cache'te varsa günceller, yoksa ekler
        cache.Set(Key(order.Id), order, Entry);
        return Task.FromResult(true);
    }
}