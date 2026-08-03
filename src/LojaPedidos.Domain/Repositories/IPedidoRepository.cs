using LojaPedidos.Domain.Entities;
using LojaPedidos.Domain.Enums;

namespace LojaPedidos.Domain.Repositories;

public interface IPedidoRepository : IRepositoryBase<Pedido>
{
    Task<Pedido?> ObterParaExclusaoAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<(IReadOnlyCollection<Pedido> Itens, int Total)> ListarAsync(
        int pagina,
        int tamanhoPagina,
        StatusPedido? status = null,
        string? cpf = null,
        CancellationToken cancellationToken = default);
}
