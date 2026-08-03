using LojaPedidos.Domain.Repositories;
using LojaPedidos.Infrastructure.DataAccess;

namespace LojaPedidos.Infrastructure.DataAccess.Repositories;

public sealed class UnitOfWork(LojaPedidosDbContext dbContext) : IUnitOfWork
{
    public Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}
