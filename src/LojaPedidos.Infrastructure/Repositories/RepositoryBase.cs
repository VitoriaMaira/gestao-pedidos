using LojaPedidos.Domain.Common;
using LojaPedidos.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LojaPedidos.Infrastructure.Repositories;

public class RepositoryBase<TEntity>(LojaPedidosDbContext dbContext)
    : IRepositoryBase<TEntity>
   where TEntity : Entity
{
    protected DbSet<TEntity> DbSet { get; } = dbContext.Set<TEntity>();

    public virtual async Task<TEntity?> ObterPorIdAsync(
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

    public void Remover(TEntity entity)
    {
        DbSet.Remove(entity);
    }
}
