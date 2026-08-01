namespace LojaPedidos.Application.Pedidos.ListarPedidos;

public interface IListarPedidosUseCase
{
    Task<ListarPedidosResponse> ExecutarAsync(
        ListarPedidosRequest request,
        CancellationToken cancellationToken = default);
}
