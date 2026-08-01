using LojaPedidos.Web.Contracts.Common;
using LojaPedidos.Web.Contracts.Pedidos;

namespace LojaPedidos.Web.Clients.Pedidos;

public interface IPedidosApiClient
{
    Task<ApiResult<CriarPedidoResponse>> CriarAsync(
        CriarPedidoRequest request,
        CancellationToken cancellationToken = default);

    Task<ApiResult<ListarPedidosResponse>> ListarAsync(
        ListarPedidosQuery query,
        CancellationToken cancellationToken = default);

    Task<ApiResult<PedidoResponse>> ObterPorIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}
