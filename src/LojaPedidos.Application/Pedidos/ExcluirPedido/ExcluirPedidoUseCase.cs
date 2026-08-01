using LojaPedidos.Application.Common.Exceptions;
using LojaPedidos.Domain.Repositories;

namespace LojaPedidos.Application.Pedidos.ExcluirPedido;

public sealed class ExcluirPedidoUseCase(
    IPedidoRepository pedidoRepository,
    IUnitOfWork unitOfWork) : IExcluirPedidoUseCase
{
    public async Task ExecutarAsync(
        Guid id,
         CancellationToken cancellationToken = default)
    {
        var pedido = await pedidoRepository.ObterPorIdAsync(id, cancellationToken);

        if (pedido is null)
        {
            throw new NotFoundException("Não foi possível encontrar o pedido informado.");
        }

        pedidoRepository.Remover(pedido);
        await unitOfWork.CommitAsync(cancellationToken);

    }
}
