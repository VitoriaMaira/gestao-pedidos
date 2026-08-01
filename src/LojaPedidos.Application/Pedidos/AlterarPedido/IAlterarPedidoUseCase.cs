using LojaPedidos.Application.Pedidos.ConsultarPedido;

namespace LojaPedidos.Application.Pedidos.AlterarPedido;

public interface IAlterarPedidoUseCase
{
    Task<PedidoResponse?> ExecutarAsync(
        Guid id,
        AlterarPedidoRequest request,
        CancellationToken cancellationToken = default);
}
