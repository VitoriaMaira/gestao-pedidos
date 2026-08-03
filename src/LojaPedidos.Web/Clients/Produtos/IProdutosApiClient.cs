using LojaPedidos.Web.Contracts.Common;
using LojaPedidos.Web.Contracts.Produtos;

namespace LojaPedidos.Web.Clients.Produtos;

public interface IProdutosApiClient
{
    Task<ApiResult<ListarProdutosResponse>> ListarAsync(
        ListarProdutosQuery query,
        CancellationToken cancellationToken = default);
}
