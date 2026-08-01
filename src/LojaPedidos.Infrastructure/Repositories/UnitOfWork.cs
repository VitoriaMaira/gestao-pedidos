using LojaPedidos.Domain.Repositories;

namespace LojaPedidos.Infrastructure.Repositories;

public sealed class UnitOfWork(LojaPedidosDbContext dbContext) : IUnitOfWork
{
    public Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}
