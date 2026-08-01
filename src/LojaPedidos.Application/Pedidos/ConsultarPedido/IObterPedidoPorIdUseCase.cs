namespace LojaPedidos.Application.Pedidos.ConsultarPedido;

public interface IObterPedidoPorIdUseCase
{
    Task<PedidoResponse> ExecutarAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}
