using System.Net;
using LojaPedidos.Application.Pedidos.CriarPedido;

namespace LojaPedidos.IntegrationTests.Pedidos;

public sealed class CriarPedidoTests
{
    [Fact]
    public async Task DeveRetornarBadRequestQuandoPedidoForInvalido()
    {
        var api = PedidosApiClient.Criar();
        var request = new CriarPedidoRequest(null, []);

        var response = await api.CriarAsync(request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
