using LojaPedidos.Domain.Entities;

namespace LojaPedidos.Domain.Repositories;

public interface ICompradorRepository : IRepositoryBase<Comprador>
{
    Task<Comprador?> ObterPorCpfAsync(
        string cpf,
        CancellationToken cancellationToken = default);
}
