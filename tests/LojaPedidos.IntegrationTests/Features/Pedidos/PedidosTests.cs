using System.Net;
using LojaPedidos.Application.Pedidos.CriarPedido;
using LojaPedidos.Application.Pedidos.ListarPedidos;
using LojaPedidos.IntegrationTests.Configurations;

namespace LojaPedidos.IntegrationTests.Features.Pedidos;

[Collection(IntegrationTestsCollection.Name)]
public sealed class PedidosTests(AspireAppFixture fixture)
{
    [Fact]
    public async Task DeveCriarEConsultarPedido()
    {
        var pedidoCriado = await CriarPedidoAsync();

        try
        {
            var response = await fixture.Api.Pedidos.ObterPorIdAsync(pedidoCriado.Id);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response.Content);
            Assert.Equal(pedidoCriado.Id, response.Content.Id);
            Assert.Single(response.Content.Itens);
        }
        finally
        {
            await ExcluirPedidoAsync(pedidoCriado.Id);
        }
    }

    [Fact]
    public async Task DeveListarPedidoCriado()
    {
        var pedidoCriado = await CriarPedidoAsync();

        try
        {
            var response = await fixture.Api.Pedidos.ListarAsync(
                new ListarPedidosRequest(1, 20, null, null));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response.Content);
            Assert.Contains(response.Content.Itens, pedido => pedido.Id == pedidoCriado.Id);
        }
        finally
        {
            await ExcluirPedidoAsync(pedidoCriado.Id);
        }
    }

    [Fact]
    public async Task DeveRetornarNotFoundParaPedidoInexistente()
    {
        var response = await fixture.Api.Pedidos.ObterPorIdAsync(Guid.CreateVersion7());

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeveRejeitarPedidoSemItens()
    {
        var request = new CriarPedidoRequest(
            new CriarCompradorRequest("Comprador dos testes", CpfValido),
            []);

        var response = await fixture.Api.Pedidos.CriarAsync(request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.IsType<Refit.ValidationApiException>(response.Error);
    }

    private async Task<CriarPedidoResponse> CriarPedidoAsync()
    {
        var request = new CriarPedidoRequest(
            new CriarCompradorRequest("Comprador dos testes", CpfValido),
            [
                new CriarItemPedidoRequest(
                    new CriarProdutoRequest(
                        $"Produto de pedido {Guid.CreateVersion7()}",
                        79.90m),
                    2)
            ]);

        var response = await fixture.Api.Pedidos.CriarAsync(request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        return Assert.IsType<CriarPedidoResponse>(response.Content);
    }

    private async Task ExcluirPedidoAsync(Guid id)
    {
        var response = await fixture.Api.Pedidos.ExcluirAsync(id);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    private const string CpfValido = "12345678909";
}
