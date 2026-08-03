using LojaPedidos.Application.Produtos.Criar;
using LojaPedidos.Application.Produtos.Listar;
using Refit;

namespace LojaPedidos.IntegrationTests.Clients.Produtos;

public interface IProdutosApi
{
    [Get("/api/produtos")]
    Task<Refit.ApiResponse<LojaPedidos.Application.Common.Responses.ApiResponse<ListarProdutosResponse>>> ListarAsync(
        [Query] ListarProdutosQuery query,
        CancellationToken cancellationToken = default);

    [Post("/api/produtos")]
    Task<Refit.ApiResponse<LojaPedidos.Application.Common.Responses.ApiResponse<CriarProdutoResponse>>> CriarAsync(
        CriarProdutoRequest request,
        CancellationToken cancellationToken = default);
}
