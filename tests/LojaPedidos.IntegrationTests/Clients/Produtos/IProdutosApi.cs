using LojaPedidos.Application.Produtos.Criar;
using Refit;

namespace LojaPedidos.IntegrationTests.Clients.Produtos;

public interface IProdutosApi
{
    [Post("/api/produtos")]
    Task<ApiResponse<CriarProdutoResponse>> CriarAsync(
        CriarProdutoRequest request,
        CancellationToken cancellationToken = default);
}
