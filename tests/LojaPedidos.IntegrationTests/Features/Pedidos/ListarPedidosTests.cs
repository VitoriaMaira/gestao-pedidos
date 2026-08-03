using System.Net;
using LojaPedidos.Application.Common.Responses;
using LojaPedidos.Application.Pedidos.CriarPedido;
using LojaPedidos.Application.Pedidos.ListarPedidos;
using LojaPedidos.Application.Produtos.Criar;
using LojaPedidos.IntegrationTests.Configurations;

namespace LojaPedidos.IntegrationTests.Features.Pedidos;

[Collection(IntegrationTestsCollection.Name)]
public sealed class ListarPedidosTests(AspireAppFixture fixture)
{
    [Fact]
    public async Task DeveListarPedidosDeFormaPaginada()
    {
        var pedidoId = await CriarPedidoAsync();

        try
        {
            var response = await fixture.Api.Pedidos.ListarAsync(
                new ListarPedidosRequest(Pagina: 1, TamanhoPagina: 20));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var apiResponse = Assert.IsType<ApiResponse<ListarPedidosResponse>>(
                response.Content);
            Assert.True(apiResponse.Sucesso);
            Assert.Equal("Pedidos listados com sucesso.", apiResponse.Mensagem);

            var resultado = Assert.IsType<ListarPedidosResponse>(apiResponse.Dados);
            Assert.Equal(1, resultado.Pagina);
            Assert.Equal(20, resultado.TamanhoPagina);
            Assert.True(resultado.Total > 0);
            Assert.Contains(resultado.Itens, pedido => pedido.Id == pedidoId);
        }
        finally
        {
            await fixture.Api.Pedidos.ExcluirAsync(pedidoId);
        }
    }

    [Fact]
    public async Task DeveRetornarBadRequestQuandoPaginacaoForInvalida()
    {
        var response = await fixture.Api.Pedidos.ListarAsync(
            new ListarPedidosRequest(Pagina: 0));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.IsType<Refit.ApiException>(response.Error);
    }

    private async Task<Guid> CriarPedidoAsync()
    {
        var produtoResponse = await fixture.Api.Produtos.CriarAsync(
            new CriarProdutoRequest(
                $"Produto para listagem {Guid.CreateVersion7()}",
                89.90m));
        var produto = Assert.IsType<CriarProdutoResponse>(
            produtoResponse.Content?.Dados);

        var pedidoResponse = await fixture.Api.Pedidos.CriarAsync(
            new CriarPedidoRequest(
                "Comprador da listagem",
                "52998224725",
                [new CriarPedidoRequest_ItemPedidoAux(produto.Id, 1)]));
        var pedido = Assert.IsType<CriarPedidoResponse>(
            pedidoResponse.Content?.Dados);

        return pedido.Id;
    }
}
