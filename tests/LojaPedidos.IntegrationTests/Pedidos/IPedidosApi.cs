using LojaPedidos.Application.Pedidos.CriarPedido;
using Refit;

namespace LojaPedidos.IntegrationTests.Pedidos;

public interface IPedidosApi
{
    [Post("/api/pedidos")]
    Task<ApiResponse<CriarPedidoResponse>> CriarAsync(CriarPedidoRequest request);
}
