using LojaPedidos.Web.Contracts.Common;
using LojaPedidos.Web.Contracts.Pedidos;

namespace LojaPedidos.Web.Clients.Pedidos;

public interface IPedidosApiClient
{
    Task<ApiResult<ListarPedidosResponse>> ListarAsync(
        ListarPedidosQuery query,
        CancellationToken cancellationToken = default);
}
