using LojaPedidos.Domain.Repositories;

namespace LojaPedidos.Application.Pedidos.ConsultarPedido;

public sealed class ObterPedidoPorIdUseCase(IPedidoRepository pedidoRepository)
    : IObterPedidoPorIdUseCase
{
    public async Task<PedidoResponse?> ExecutarAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var pedido = await pedidoRepository.ObterPorIdAsync(id, cancellationToken);

        return pedido is null ? null : PedidoResponse.Criar(pedido);
    }
}
