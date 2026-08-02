using System.Text.Json;
using System.Text.Json.Serialization;
using LojaPedidos.IntegrationTests.Clients.Pedidos;
using LojaPedidos.IntegrationTests.Clients.Produtos;
using Refit;

namespace LojaPedidos.IntegrationTests.Clients;

public sealed class ApiClients
{
    private ApiClients(HttpClient httpClient, RefitSettings settings)
    {
        Produtos = RestService.For<IProdutosApi>(httpClient, settings);
        Pedidos = RestService.For<IPedidosApi>(httpClient, settings);
    }

    public IProdutosApi Produtos { get; }

    public IPedidosApi Pedidos { get; }

    public static ApiClients Criar(HttpClient httpClient)
    {
        var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        jsonOptions.Converters.Add(new JsonStringEnumConverter());

        var settings = new RefitSettings
        {
            ContentSerializer = new SystemTextJsonContentSerializer(jsonOptions)
        };

        return new ApiClients(httpClient, settings);
    }
}
