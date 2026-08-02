using LojaPedidos.Application.Pedidos.AlterarPedido;
using LojaPedidos.Application.Pedidos.AtualizarStatusPedido;
using LojaPedidos.Application.Pedidos.ConsultarPedido;
using LojaPedidos.Application.Pedidos.CriarPedido;
using LojaPedidos.Application.Pedidos.ExcluirPedido;
using LojaPedidos.Application.Pedidos.ListarPedidos;
using Refit;

namespace LojaPedidos.IntegrationTests.Clients.Pedidos;

public interface IPedidosApi
{
    [Post("/api/pedidos")]
    Task<ApiResponse<CriarPedidoResponse>> CriarAsync(
        CriarPedidoRequest request,
        CancellationToken cancellationToken = default);

    [Get("/api/pedidos/{id}")]
    Task<ApiResponse<PedidoResponse>> ObterPorIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    [Get("/api/pedidos")]
    Task<ApiResponse<ListarPedidosResponse>> ListarAsync(
        [Query] ListarPedidosRequest request,
        CancellationToken cancellationToken = default);

    [Put("/api/pedidos/{id}")]
    Task<ApiResponse<PedidoResponse>> AlterarAsync(
        Guid id,
        AlterarPedidoRequest request,
        CancellationToken cancellationToken = default);

    [Put("/api/pedidos/{id}/status")]
    Task<ApiResponse<AtualizarStatusPedidoResponse>> AtualizarStatusAsync(
        Guid id,
        AtualizarStatusPedidoRequest request,
        CancellationToken cancellationToken = default);

    [Delete("/api/pedidos/{id}")]
    Task<ApiResponse<ExcluirPedidoResponse>> ExcluirAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}
