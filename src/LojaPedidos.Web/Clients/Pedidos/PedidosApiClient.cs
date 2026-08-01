using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using LojaPedidos.Web.Contracts.Common;
using LojaPedidos.Web.Contracts.Pedidos;

namespace LojaPedidos.Web.Clients.Pedidos;

public sealed class PedidosApiClient(HttpClient httpClient) : IPedidosApiClient
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() }
    };

    public async Task<ApiResult<ListarPedidosResponse>> ListarAsync(
        ListarPedidosQuery query,
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var response = await httpClient.GetAsync(
                CriarUrlDeListagem(query),
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return await CriarResultadoDeErroAsync(response, cancellationToken);
            }

            var dados = await response.Content.ReadFromJsonAsync<ListarPedidosResponse>(
                JsonOptions,
                cancellationToken);

            return dados is null
                ? ApiResult<ListarPedidosResponse>.Falha(
                    "A API retornou uma resposta vazia.",
                    response.StatusCode)
                : ApiResult<ListarPedidosResponse>.Ok(dados);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (TaskCanceledException)
        {
            return ApiResult<ListarPedidosResponse>.Falha(
                "A API demorou mais que o esperado para responder. Tente novamente.");
        }
        catch (HttpRequestException)
        {
            return ApiResult<ListarPedidosResponse>.Falha(
                "Não foi possível acessar a API. Verifique se o ambiente está em execução e tente novamente.");
        }
        catch (JsonException)
        {
            return ApiResult<ListarPedidosResponse>.Falha(
                "A resposta recebida da API não pôde ser interpretada.");
        }
    }

    private static string CriarUrlDeListagem(ListarPedidosQuery query)
    {
        var parametros = new List<string>
        {
            $"pagina={query.Pagina}",
            $"tamanhoPagina={query.TamanhoPagina}"
        };

        if (query.Status is not null)
        {
            parametros.Add($"status={Uri.EscapeDataString(query.Status.Value.ToString())}");
        }

        if (!string.IsNullOrWhiteSpace(query.Cpf))
        {
            parametros.Add($"cpf={Uri.EscapeDataString(query.Cpf.Trim())}");
        }

        return $"api/pedidos?{string.Join('&', parametros)}";
    }

    private static async Task<ApiResult<ListarPedidosResponse>> CriarResultadoDeErroAsync(
        HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        try
        {
            var problema = await response.Content.ReadFromJsonAsync<ApiProblem>(
                JsonOptions,
                cancellationToken);

            var mensagem = problema?.Detail
                ?? problema?.Title
                ?? "Não foi possível consultar os pedidos.";

            return ApiResult<ListarPedidosResponse>.Falha(
                mensagem,
                response.StatusCode,
                problema?.Errors);
        }
        catch (JsonException)
        {
            return ApiResult<ListarPedidosResponse>.Falha(
                "Não foi possível consultar os pedidos.",
                response.StatusCode);
        }
    }
}
