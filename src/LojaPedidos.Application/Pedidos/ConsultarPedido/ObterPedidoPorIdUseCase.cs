using LojaPedidos.Application.Common.Exceptions;
using LojaPedidos.Domain.Repositories;

namespace LojaPedidos.Application.Pedidos.ConsultarPedido;

public sealed class ObterPedidoPorIdUseCase(IPedidoRepository pedidoRepository)
    : IObterPedidoPorIdUseCase
{
    public async Task<PedidoResponse> ExecutarAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var pedido = await pedidoRepository.ObterPorIdAsync(id, cancellationToken);

        if (pedido is null)
        {
            throw new NotFoundException("Não foi possível encontrar o pedido informado.");
        }

        return PedidoResponse.Criar(pedido);
    }
}
