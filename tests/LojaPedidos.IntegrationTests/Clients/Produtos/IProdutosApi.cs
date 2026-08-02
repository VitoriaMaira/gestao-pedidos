using LojaPedidos.Application.Produtos.Criar;
using LojaPedidos.Application.Produtos.Listar;
using Refit;

namespace LojaPedidos.IntegrationTests.Clients.Produtos;

public interface IProdutosApi
{
    [Get("/api/produtos")]
    Task<ApiResponse<ListarProdutosResponse>> ListarAsync(
        [Query] ListarProdutosQuery query,
        CancellationToken cancellationToken = default);

    [Post("/api/produtos")]
    Task<ApiResponse<CriarProdutoResponse>> CriarAsync(
        CriarProdutoRequest request,
        CancellationToken cancellationToken = default);
}
