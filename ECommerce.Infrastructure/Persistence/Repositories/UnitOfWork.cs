using ECommerce.Application.Abstractions;

namespace ECommerce.Infrastructure.Persistence.Repositories;

public sealed class UnitOfWork(AppDbContext db) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken ct) => db.SaveChangesAsync(ct);
}