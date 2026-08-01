using System.Text.Json;
using System.Text.Json.Serialization;
using Refit;

namespace LojaPedidos.IntegrationTests.Pedidos;

internal static class PedidosApiClient
{
    public static IPedidosApi Criar()
    {
        var baseUrl = Environment.GetEnvironmentVariable("LOJA_PEDIDOS_API_URL")
            ?? "http://localhost:5080";
        var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        jsonOptions.Converters.Add(new JsonStringEnumConverter());

        var settings = new RefitSettings
        {
            ContentSerializer = new SystemTextJsonContentSerializer(jsonOptions)
        };

        return RestService.For<IPedidosApi>(baseUrl, settings);
    }
}
