using System.Net;
using LojaPedidos.Application.Pedidos.CriarPedido;
using Refit;

namespace LojaPedidos.IntegrationTests.Pedidos;

public sealed class CriarPedidoTests
{
    [Fact]
    public async Task DeveRetornarBadRequestQuandoPedidoForInvalido()
    {
        var baseUrl = Environment.GetEnvironmentVariable("LOJA_PEDIDOS_API_URL")
            ?? "http://localhost:5080";
        var api = RestService.For<IPedidosApi>(baseUrl);
        var request = new CriarPedidoRequest(null, []);

        var response = await api.CriarAsync(request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
