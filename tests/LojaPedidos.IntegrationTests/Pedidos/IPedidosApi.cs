using LojaPedidos.Application.Pedidos.AlterarPedido;
using LojaPedidos.Application.Pedidos.AtualizarStatusPedido;
using LojaPedidos.Application.Pedidos.ConsultarPedido;
using LojaPedidos.Application.Pedidos.CriarPedido;
using LojaPedidos.Application.Pedidos.ExcluirPedido;
using Refit;

namespace LojaPedidos.IntegrationTests.Pedidos;

public interface IPedidosApi
{
    [Post("/api/pedidos")]
    Task<ApiResponse<CriarPedidoResponse>> CriarAsync(CriarPedidoRequest request);

    [Get("/api/pedidos/{id}")]
    Task<ApiResponse<PedidoResponse>> ObterPorIdAsync(Guid id);

    [Put("/api/pedidos/{id}")]
    Task<ApiResponse<PedidoResponse>> AlterarAsync(
        Guid id,
        AlterarPedidoRequest request);

    [Put("/api/pedidos/{id}/status")]
    Task<ApiResponse<AtualizarStatusPedidoResponse>> AtualizarStatusAsync(
        Guid id,
        AtualizarStatusPedidoRequest request);

    [Delete("/api/pedidos/{id}")]
    Task<ApiResponse<ExcluirPedidoResponse>> ExcluirAsync(Guid id);
}
