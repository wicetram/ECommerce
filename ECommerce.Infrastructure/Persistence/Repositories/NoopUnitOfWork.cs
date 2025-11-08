using ECommerce.Application.Abstractions;

namespace ECommerce.Infrastructure.Persistence.Repositories;

public sealed class NoopUnitOfWork : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken ct) => Task.FromResult(0);
}