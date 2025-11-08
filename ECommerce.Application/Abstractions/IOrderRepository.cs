using ECommerce.Domain.Entities;

namespace ECommerce.Application.Abstractions;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<bool> AddAsync(Order order, CancellationToken ct);
    Task<bool> UpdateAsync(Order order, CancellationToken ct);
}
