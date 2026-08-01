using LojaPedidos.Domain.Repositories;

namespace LojaPedidos.Application.Pedidos.ExcluirPedido;

public sealed class ExcluirPedidoUseCase(
    IPedidoRepository pedidoRepository,
    IUnitOfWork unitOfWork) : IExcluirPedidoUseCase
{
    public async Task<bool> ExecutarAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var pedido = await pedidoRepository.ObterPorIdAsync(id, cancellationToken);

        if (pedido is null)
        {
            return false;
        }

        pedidoRepository.Remover(pedido);
        await unitOfWork.CommitAsync(cancellationToken);

        return true;
    }
}
