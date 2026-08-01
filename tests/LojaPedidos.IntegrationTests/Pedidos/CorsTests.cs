using System.Net;

namespace LojaPedidos.IntegrationTests.Pedidos;

public sealed class CorsTests
{
    [Fact]
    public async Task DevePermitirRequisicaoDoFrontendConfigurado()
    {
        using var client = new HttpClient
        {
            BaseAddress = new Uri(PedidosApiClient.ObterBaseUrl())
        };
        using var request = new HttpRequestMessage(HttpMethod.Options, "/api/pedidos");
        request.Headers.Add("Origin", "http://localhost:5056");
        request.Headers.Add("Access-Control-Request-Method", "GET");

        using var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.Equal(
            "http://localhost:5056",
            Assert.Single(response.Headers.GetValues("Access-Control-Allow-Origin")));
    }
}
