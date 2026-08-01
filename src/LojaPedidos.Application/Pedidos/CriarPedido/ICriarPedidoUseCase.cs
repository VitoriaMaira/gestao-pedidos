namespace LojaPedidos.Application.Pedidos.CriarPedido;

public interface ICriarPedidoUseCase
{
    Task<CriarPedidoResponse> ExecutarAsync(
        CriarPedidoRequest request,
        CancellationToken cancellationToken = default);
}
