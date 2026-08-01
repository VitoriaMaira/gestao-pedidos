namespace LojaPedidos.Application.Pedidos.AtualizarStatusPedido;

public interface IAtualizarStatusPedidoUseCase
{
    Task<AtualizarStatusPedidoResponse?> ExecutarAsync(
        Guid id,
        AtualizarStatusPedidoRequest request,
        CancellationToken cancellationToken = default);
}
