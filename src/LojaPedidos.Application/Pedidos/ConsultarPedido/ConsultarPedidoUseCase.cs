using LojaPedidos.Application.Common.Exceptions;
using LojaPedidos.Domain.Repositories;

namespace LojaPedidos.Application.Pedidos.ConsultarPedido;

public interface IConsultarPedidoUseCase
{
    Task<ConsultarPedidoResponse> Execute(Guid id, CancellationToken cancellationToken = default);
}

public sealed class ConsultarPedidoUseCase(IPedidoRepository pedidoRepository) : IConsultarPedidoUseCase
{
    public async Task<ConsultarPedidoResponse> Execute(Guid id, CancellationToken cancellationToken = default)
    {
        var pedido = await pedidoRepository.ObterPorId(id, cancellationToken);

        if (pedido is null)
            throw new NotFoundException("Não foi possível encontrar o pedido informado.");

        return ConsultarPedidoResponse.Map(pedido);
    }
}
