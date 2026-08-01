namespace LojaPedidos.Application.Pedidos.ExcluirPedido;

public interface IExcluirPedidoUseCase
{
    Task<bool> ExecutarAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}
