namespace LojaPedidos.Application.Pedidos.ExcluirPedido;

public interface IExcluirPedidoUseCase
{
    Task<ExcluirPedidoResponse> ExecutarAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}
