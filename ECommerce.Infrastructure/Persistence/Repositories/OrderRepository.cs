using ECommerce.Application.Abstractions;
using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Persistence.Repositories;

public sealed class OrderRepository(AppDbContext db) : IOrderRepository
{
    public async Task<Order?> GetByIdAsync(Guid id, CancellationToken ct) =>
        await db.Orders
                .Include("_items")
                .FirstOrDefaultAsync(o => o.Id == id, ct);

    public async Task<bool> AddAsync(Order order, CancellationToken ct)
    {
        var result = await db.Orders.AddAsync(order, ct);
        return result != null;
    }

    public Task<bool> UpdateAsync(Order order, CancellationToken ct)
    {
        var result = db.Orders.Update(order);
        return Task.FromResult(result != null);
    }
}
