using System.Net.Http.Json;
using System.Text.Json;
using LojaPedidos.Web.Contracts.Common;
using LojaPedidos.Web.Contracts.Produtos;

namespace LojaPedidos.Web.Clients.Produtos;

public sealed class ProdutosApiClient(HttpClient httpClient) : IProdutosApiClient
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task<ApiResult<ListarProdutosResponse>> ListarAsync(
        ListarProdutosQuery query,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var url = $"api/produtos?pagina={query.Pagina}&tamanhoPagina={query.TamanhoPagina}";
            using var response = await httpClient.GetAsync(url, cancellationToken);
            var content = await response.Content.ReadFromJsonAsync<ApiResponse<ListarProdutosResponse>>(
                JsonOptions,
                cancellationToken);

            if (!response.IsSuccessStatusCode || content is null || !content.Sucesso)
            {
                return ApiResult<ListarProdutosResponse>.Falha(
                    content?.Mensagem ?? "Não foi possível carregar os produtos.",
                    response.StatusCode);
            }

            return content.Dados is null
                ? ApiResult<ListarProdutosResponse>.Falha("A API retornou uma resposta vazia.")
                : new ApiResult<ListarProdutosResponse>(
                    true,
                    content.Dados,
                    content.Mensagem,
                    response.StatusCode);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (HttpRequestException)
        {
            return ApiResult<ListarProdutosResponse>.Falha("Não foi possível acessar a API.");
        }
        catch (JsonException)
        {
            return ApiResult<ListarProdutosResponse>.Falha("A resposta de produtos não pôde ser interpretada.");
        }
    }
}
