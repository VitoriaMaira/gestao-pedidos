using System.Net;
using LojaPedidos.Application.Pedidos.CriarPedido;
using LojaPedidos.Application.Produtos.Criar;
using LojaPedidos.IntegrationTests.Configurations;

namespace LojaPedidos.IntegrationTests.Features.Pedidos;

[Collection(IntegrationTestsCollection.Name)]
public sealed class CriarPedidoTests(AspireAppFixture fixture)
{
    [Fact]
    public async Task DeveCriarPedidoComDadosValidos()
    {
        var produto = await CriarProdutoAsync();
        var request = NovoPedido(
            [new CriarPedidoRequest_ItemPedidoAux(produto.Id, 2)]);

        var response = await fixture.Api.Pedidos.CriarAsync(request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var pedido = Assert.IsType<CriarPedidoResponse>(response.Content);
        Assert.NotEqual(Guid.Empty, pedido.Id);

        await ExcluirPedidoAsync(pedido.Id);
    }

    [Fact]
    public async Task DeveReutilizarCompradorExistente()
    {
        var primeiroProduto = await CriarProdutoAsync();
        var segundoProduto = await CriarProdutoAsync();
        var cpf = CpfValido;

        var primeiroPedido = await CriarPedidoAsync(
            NovoPedido(
                [new CriarPedidoRequest_ItemPedidoAux(primeiroProduto.Id, 1)],
                cpf));
        var segundoPedido = await CriarPedidoAsync(
            NovoPedido(
                [new CriarPedidoRequest_ItemPedidoAux(segundoProduto.Id, 1)],
                cpf));

        try
        {
            var primeiraConsulta = await fixture.Api.Pedidos.ObterPorIdAsync(
                primeiroPedido.Id);
            var segundaConsulta = await fixture.Api.Pedidos.ObterPorIdAsync(
                segundoPedido.Id);

            Assert.Equal(HttpStatusCode.OK, primeiraConsulta.StatusCode);
            Assert.Equal(HttpStatusCode.OK, segundaConsulta.StatusCode);
            Assert.NotNull(primeiraConsulta.Content);
            Assert.NotNull(segundaConsulta.Content);
            Assert.Equal(
                primeiraConsulta.Content.CompradorId,
                segundaConsulta.Content.CompradorId);
        }
        finally
        {
            await ExcluirPedidoAsync(primeiroPedido.Id);
            await ExcluirPedidoAsync(segundoPedido.Id);
        }
    }

    [Theory]
    [InlineData("", "12345678909")]
    [InlineData("Comprador dos testes", "")]
    public async Task DeveRejeitarPedidoComCompradorInvalido(
        string nomeComprador,
        string cpfComprador)
    {
        var produto = await CriarProdutoAsync();
        var request = new CriarPedidoRequest(
            nomeComprador,
            cpfComprador,
            [new CriarPedidoRequest_ItemPedidoAux(produto.Id, 1)]);

        var response = await fixture.Api.Pedidos.CriarAsync(request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task DeveRejeitarPedidoComQuantidadeInvalida(int quantidade)
    {
        var produto = await CriarProdutoAsync();
        var request = NovoPedido(
            [new CriarPedidoRequest_ItemPedidoAux(produto.Id, quantidade)]);

        var response = await fixture.Api.Pedidos.CriarAsync(request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task DeveRetornarNotFoundQuandoProdutoNaoExistir()
    {
        var request = NovoPedido(
            [new CriarPedidoRequest_ItemPedidoAux(Guid.CreateVersion7(), 1)]);

        var response = await fixture.Api.Pedidos.CriarAsync(request);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    private async Task<CriarProdutoResponse> CriarProdutoAsync()
    {
        var response = await fixture.Api.Produtos.CriarAsync(
            new CriarProdutoRequest(
                $"Produto de pedido {Guid.CreateVersion7()}",
                79.90m));

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        return Assert.IsType<CriarProdutoResponse>(response.Content);
    }

    private async Task<CriarPedidoResponse> CriarPedidoAsync(
        CriarPedidoRequest request)
    {
        var response = await fixture.Api.Pedidos.CriarAsync(request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        return Assert.IsType<CriarPedidoResponse>(response.Content);
    }

    private async Task ExcluirPedidoAsync(Guid id)
    {
        var response = await fixture.Api.Pedidos.ExcluirAsync(id);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    private static CriarPedidoRequest NovoPedido(
        List<CriarPedidoRequest_ItemPedidoAux> itens,
        string cpf = CpfValido) =>
        new("Comprador dos testes", cpf, itens);

    private const string CpfValido = "12345678909";
}
