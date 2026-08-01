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

    public Task<ApiResult<CriarPedidoResponse>> CriarAsync(
        CriarPedidoRequest request,
        CancellationToken cancellationToken = default) =>
        EnviarAsync<CriarPedidoResponse>(
            HttpMethod.Post,
            "api/pedidos",
            request,
            "Não foi possível criar o pedido.",
            cancellationToken);

    public Task<ApiResult<ListarPedidosResponse>> ListarAsync(
        ListarPedidosQuery query,
        CancellationToken cancellationToken = default) =>
        EnviarAsync<ListarPedidosResponse>(
            HttpMethod.Get,
            CriarUrlDeListagem(query),
            null,
            "Não foi possível consultar os pedidos.",
            cancellationToken);

    public Task<ApiResult<PedidoResponse>> ObterPorIdAsync(
        Guid id,
        CancellationToken cancellationToken = default) =>
        EnviarAsync<PedidoResponse>(
            HttpMethod.Get,
            $"api/pedidos/{id}",
            null,
            "Não foi possível consultar o pedido.",
            cancellationToken);

    private async Task<ApiResult<T>> EnviarAsync<T>(
        HttpMethod metodo,
        string url,
        object? conteudo,
        string mensagemPadrao,
        CancellationToken cancellationToken)
    {
        try
        {
            using var request = new HttpRequestMessage(metodo, url);

            if (conteudo is not null)
            {
                request.Content = JsonContent.Create(conteudo, options: JsonOptions);
            }

            using var response = await httpClient.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return await CriarResultadoDeErroAsync<T>(
                    response,
                    mensagemPadrao,
                    cancellationToken);
            }

            var dados = await response.Content.ReadFromJsonAsync<T>(
                JsonOptions,
                cancellationToken);

            return dados is null
                ? ApiResult<T>.Falha("A API retornou uma resposta vazia.", response.StatusCode)
                : ApiResult<T>.Ok(dados);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (TaskCanceledException)
        {
            return ApiResult<T>.Falha(
                "A API demorou mais que o esperado para responder. Tente novamente.");
        }
        catch (HttpRequestException)
        {
            return ApiResult<T>.Falha(
                "Não foi possível acessar a API. Verifique se o ambiente está em execução e tente novamente.");
        }
        catch (JsonException)
        {
            return ApiResult<T>.Falha(
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

    private static async Task<ApiResult<T>> CriarResultadoDeErroAsync<T>(
        HttpResponseMessage response,
        string mensagemPadrao,
        CancellationToken cancellationToken)
    {
        try
        {
            var problema = await response.Content.ReadFromJsonAsync<ApiProblem>(
                JsonOptions,
                cancellationToken);

            var mensagem = problema?.Detail ?? problema?.Title ?? mensagemPadrao;

            return ApiResult<T>.Falha(
                mensagem,
                response.StatusCode,
                problema?.Errors);
        }
        catch (JsonException)
        {
            return ApiResult<T>.Falha(mensagemPadrao, response.StatusCode);
        }
    }
}
