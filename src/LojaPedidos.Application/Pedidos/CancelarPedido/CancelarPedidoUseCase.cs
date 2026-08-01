using LojaPedidos.Application.Pedidos.ConsultarPedido;
using LojaPedidos.Domain.Repositories;

namespace LojaPedidos.Application.Pedidos.CancelarPedido;

public sealed class CancelarPedidoUseCase(
    IPedidoRepository pedidoRepository,
    IUnitOfWork unitOfWork) : ICancelarPedidoUseCase
{
    public async Task<CancelarPedidoResponse?> ExecutarAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var pedido = await pedidoRepository.ObterPorIdAsync(id, cancellationToken);

        if (pedido is null)
        {
            return null;
        }

        pedido.Cancelar();
        await unitOfWork.CommitAsync(cancellationToken);

        return new CancelarPedidoResponse(
            "Pedido cancelado com sucesso.",
            PedidoResponse.Criar(pedido));
    }
}
