using LojaPedidos.Application.Common.Exceptions;
using LojaPedidos.Domain.Repositories;

namespace LojaPedidos.Application.Pedidos.ExcluirPedido;

public sealed class ExcluirPedidoUseCase(
    IPedidoRepository pedidoRepository,
    IUnitOfWork unitOfWork) : IExcluirPedidoUseCase
{
    public async Task<ExcluirPedidoResponse> ExecutarAsync(
        Guid id,
         CancellationToken cancellationToken = default)
    {
        var pedido = await pedidoRepository.ObterPorIdAsync(id, cancellationToken);

        if (pedido is null)
        {
            throw new NotFoundException(
                "Não foi possível excluir o pedido porque ele não foi encontrado.");
        }

        pedidoRepository.Remover(pedido);
        await unitOfWork.CommitAsync(cancellationToken);

        return new ExcluirPedidoResponse("Pedido excluído com sucesso.");
    }
}
