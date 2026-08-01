namespace LojaPedidos.Application.Pedidos.CancelarPedido;

public interface ICancelarPedidoUseCase
{
    Task<CancelarPedidoResponse?> ExecutarAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}
