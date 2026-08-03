using System.Net;
using LojaPedidos.Application.Common.Responses;
using LojaPedidos.Application.Pedidos.AlterarPedido;
using LojaPedidos.Application.Pedidos.ConsultarPedido;
using LojaPedidos.Application.Pedidos.CriarPedido;
using LojaPedidos.Application.Produtos.Criar;
using LojaPedidos.IntegrationTests.Configurations;

namespace LojaPedidos.IntegrationTests.Features.Pedidos;

[Collection(IntegrationTestsCollection.Name)]
public sealed class AlterarPedidoTests(AspireAppFixture fixture)
{
    [Fact]
    public async Task DeveAlterarQuantidadeDoItemDoPedido()
    {
        var pedido = await CriarPedidoAsync();

        try
        {
            var item = Assert.Single(pedido.Itens);
            var response = await fixture.Api.Pedidos.AlterarAsync(
                pedido.Id,
                new AlterarPedidoRequest([new AlterarItemPedidoRequest(item.Id, 3)]));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var apiResponse = Assert.IsType<ApiResponse<ConsultarPedidoResponse>>(
                response.Content);
            Assert.True(apiResponse.Sucesso);
            Assert.Equal("Pedido atualizado com sucesso.", apiResponse.Mensagem);

            var pedidoAlterado = Assert.IsType<ConsultarPedidoResponse>(apiResponse.Dados);
            var itemAlterado = Assert.Single(pedidoAlterado.Itens);
            Assert.Equal(3, itemAlterado.Quantidade);
            Assert.Equal(itemAlterado.PrecoUnitario * 3, itemAlterado.Subtotal);
            Assert.Equal(itemAlterado.Subtotal, pedidoAlterado.Total);
        }
        finally
        {
            await fixture.Api.Pedidos.ExcluirAsync(pedido.Id);
        }
    }

    [Fact]
    public async Task DeveRejeitarQuantidadeInvalida()
    {
        var pedido = await CriarPedidoAsync();

        try
        {
            var item = Assert.Single(pedido.Itens);
            var response = await fixture.Api.Pedidos.AlterarAsync(
                pedido.Id,
                new AlterarPedidoRequest([new AlterarItemPedidoRequest(item.Id, 0)]));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.IsType<Refit.ApiException>(response.Error);
        }
        finally
        {
            await fixture.Api.Pedidos.ExcluirAsync(pedido.Id);
        }
    }

    [Fact]
    public async Task DeveRetornarNotFoundQuandoPedidoNaoExistir()
    {
        var response = await fixture.Api.Pedidos.AlterarAsync(
            Guid.CreateVersion7(),
            new AlterarPedidoRequest([new AlterarItemPedidoRequest(Guid.CreateVersion7(), 1)]));

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.IsType<Refit.ApiException>(response.Error);
    }

    private async Task<ConsultarPedidoResponse> CriarPedidoAsync()
    {
        var produtoResponse = await fixture.Api.Produtos.CriarAsync(
            new CriarProdutoRequest($"Produto para alteração {Guid.CreateVersion7()}", 25m));
        Assert.Equal(HttpStatusCode.Created, produtoResponse.StatusCode);
        var produto = Assert.IsType<CriarProdutoResponse>(produtoResponse.Content?.Dados);

        var pedidoResponse = await fixture.Api.Pedidos.CriarAsync(
            new CriarPedidoRequest(
                "Comprador para alteração",
                "93541134780",
                [new CriarPedidoRequest_ItemPedidoAux(produto.Id, 1)]));
        Assert.Equal(HttpStatusCode.Created, pedidoResponse.StatusCode);
        var pedidoCriado = Assert.IsType<CriarPedidoResponse>(pedidoResponse.Content?.Dados);

        var consulta = await fixture.Api.Pedidos.ConsultarAsync(pedidoCriado.Id);
        Assert.Equal(HttpStatusCode.OK, consulta.StatusCode);
        return Assert.IsType<ConsultarPedidoResponse>(consulta.Content?.Dados);
    }
}
