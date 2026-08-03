using LojaPedidos.Application.Pedidos.AlterarPedido;
using LojaPedidos.Application.Pedidos.AtualizarStatusPedido;
using LojaPedidos.Application.Pedidos.ConsultarPedido;
using LojaPedidos.Application.Pedidos.CriarPedido;
using LojaPedidos.Application.Pedidos.ExcluirPedido;
using LojaPedidos.Application.Pedidos.ListarPedidos;
using LojaPedidos.Application.Produtos.Criar;
using Refit;

namespace LojaPedidos.IntegrationTests.Pedidos;

public interface IPedidosApi
{
    [Post("/api/produtos")]
    Task<ApiResponse<CriarProdutoResponse>> CriarProdutoAsync(
        CriarProdutoRequest request);

    [Post("/api/pedidos")]
    Task<ApiResponse<CriarPedidoResponse>> CriarAsync(CriarPedidoRequest request);

    [Get("/api/pedidos/{id}")]
    Task<ApiResponse<PedidoResponse>> ObterPorIdAsync(Guid id);

    [Get("/api/pedidos")]
    Task<ApiResponse<ListarPedidosResponse>> ListarAsync(
        [Query] ListarPedidosRequest request);

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
