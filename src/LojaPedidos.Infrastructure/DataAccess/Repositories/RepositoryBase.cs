using LojaPedidos.Domain.Common;
using LojaPedidos.Domain.Repositories;
using LojaPedidos.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace LojaPedidos.Infrastructure.DataAccess.Repositories;

public class RepositoryBase<TEntity>(LojaPedidosDbContext dbContext)
    : IRepositoryBase<TEntity>
   where TEntity : Entity
{
    protected LojaPedidosDbContext DbContext { get; } = dbContext;

    protected DbSet<TEntity> DbSet { get; } = dbContext.Set<TEntity>();

    public virtual async Task<TEntity?> ObterPorId(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await DbSet.FindAsync([id], cancellationToken);
    }

    public async Task AdicionarAsync(
        TEntity entity,
        CancellationToken cancellationToken = default)
    {
        await DbSet.AddAsync(entity, cancellationToken);
    }

    public void Atualizar(TEntity entity)
    {
        DbSet.Update(entity);
    }

    public virtual void Remover(TEntity entity)
    {
        DbSet.Remove(entity);
    }
}
