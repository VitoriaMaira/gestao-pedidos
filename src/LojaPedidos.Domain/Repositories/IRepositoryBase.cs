using LojaPedidos.Domain.Common;

namespace LojaPedidos.Domain.Repositories;

public interface IRepositoryBase<TEntity>
    where TEntity : Entity
{
    Task<TEntity?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task AdicionarAsync(TEntity entity, CancellationToken cancellationToken = default);

    void Atualizar(TEntity entity);

    void Remover(TEntity entity);
}
