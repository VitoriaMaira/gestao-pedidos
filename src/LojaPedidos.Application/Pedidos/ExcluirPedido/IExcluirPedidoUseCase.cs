namespace LojaPedidos.Application.Pedidos.ExcluirPedido;

public interface IExcluirPedidoUseCase
{
    Task ExecutarAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}
