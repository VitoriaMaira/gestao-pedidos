using System.Net;
using LojaPedidos.Application.Pedidos.CriarPedido;
using LojaPedidos.Application.Pedidos.ConsultarPedido;
using LojaPedidos.Application.Produtos.Criar;

namespace LojaPedidos.IntegrationTests.Pedidos;

public sealed class CriarPedidoTests
{
    private readonly IPedidosApi _api = PedidosApiClient.Criar();

    [Fact]
    public async Task DeveRetornarBadRequestQuandoPedidoForInvalido()
    {
        var request = new CriarPedidoRequest(string.Empty, string.Empty, []);

        var response = await _api.CriarAsync(request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task DeveReutilizarCompradorQuandoCpfJaEstiverCadastrado()
    {
        const string cpf = "11144477735";
        var primeiroPedido = await CriarPedidoAsync("Primeiro nome", cpf);
        var segundoPedido = await CriarPedidoAsync("Nome informado novamente", cpf);

        try
        {
            Assert.Equal(primeiroPedido.CompradorId, segundoPedido.CompradorId);
            Assert.Equal("Primeiro nome", segundoPedido.Comprador);
        }
        finally
        {
            await _api.ExcluirAsync(primeiroPedido.Id);
            await _api.ExcluirAsync(segundoPedido.Id);
        }
    }

    private async Task<PedidoResponse> CriarPedidoAsync(string comprador, string cpf)
    {
        var produtoResponse = await _api.CriarProdutoAsync(
            new CriarProdutoRequest($"Produto {Guid.CreateVersion7()}", 100m));
        Assert.Equal(HttpStatusCode.Created, produtoResponse.StatusCode);
        var produto = Assert.IsType<CriarProdutoResponse>(produtoResponse.Content);

        var request = new CriarPedidoRequest(
            comprador,
            cpf,
            [new CriarPedidoRequest_ItemPedidoAux(produto.Id, 1)]);
        var response = await _api.CriarAsync(request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(response.Content);

        var consulta = await _api.ObterPorIdAsync(response.Content.Id);
        Assert.Equal(HttpStatusCode.OK, consulta.StatusCode);
        return Assert.IsType<PedidoResponse>(consulta.Content);
    }
}
