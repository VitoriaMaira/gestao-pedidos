using System.Net;
using LojaPedidos.Application.Common.Responses;
using LojaPedidos.Application.Pedidos.ConsultarPedido;
using LojaPedidos.Application.Pedidos.CriarPedido;
using LojaPedidos.Application.Produtos.Criar;
using LojaPedidos.Domain.Enums;
using LojaPedidos.IntegrationTests.Configurations;

namespace LojaPedidos.IntegrationTests.Features.Pedidos;

[Collection(IntegrationTestsCollection.Name)]
public sealed class ExcluirPedidoTests(AspireAppFixture fixture)
{
    [Fact]
    public async Task DeveCancelarPedidoSemRemoveLoDoBanco()
    {
        var pedidoId = await CriarPedidoAsync();

        var response = await fixture.Api.Pedidos.ExcluirAsync(pedidoId);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var apiResponse = Assert.IsType<ApiResponse<object>>(response.Content);
        Assert.True(apiResponse.Sucesso);
        Assert.Equal("Pedido cancelado com sucesso.", apiResponse.Mensagem);
        Assert.Null(apiResponse.Dados);

        var consulta = await fixture.Api.Pedidos.ConsultarAsync(pedidoId);

        Assert.Equal(HttpStatusCode.OK, consulta.StatusCode);
        var pedido = Assert.IsType<ConsultarPedidoResponse>(consulta.Content?.Dados);
        Assert.Equal(StatusPedido.Cancelado, pedido.Status);
    }

    [Fact]
    public async Task DeveManterCancelamentoQuandoPedidoJaEstiverCancelado()
    {
        var pedidoId = await CriarPedidoAsync();
        var primeiroCancelamento = await fixture.Api.Pedidos.ExcluirAsync(pedidoId);

        var segundoCancelamento = await fixture.Api.Pedidos.ExcluirAsync(pedidoId);

        Assert.Equal(HttpStatusCode.OK, primeiroCancelamento.StatusCode);
        Assert.Equal(HttpStatusCode.OK, segundoCancelamento.StatusCode);
        var apiResponse = Assert.IsType<ApiResponse<object>>(
            segundoCancelamento.Content);
        Assert.True(apiResponse.Sucesso);
        Assert.Null(apiResponse.Dados);
    }

    [Fact]
    public async Task DeveRetornarNotFoundQuandoPedidoNaoExistir()
    {
        var response = await fixture.Api.Pedidos.ExcluirAsync(
            Guid.CreateVersion7());

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.IsType<Refit.ApiException>(response.Error);
    }

    private async Task<Guid> CriarPedidoAsync()
    {
        var produtoResponse = await fixture.Api.Produtos.CriarAsync(
            new CriarProdutoRequest(
                $"Produto para cancelamento {Guid.CreateVersion7()}",
                99.90m));

        Assert.Equal(HttpStatusCode.Created, produtoResponse.StatusCode);
        var produto = Assert.IsType<CriarProdutoResponse>(
            produtoResponse.Content?.Dados);

        var pedidoResponse = await fixture.Api.Pedidos.CriarAsync(
            new CriarPedidoRequest(
                "Comprador para cancelamento",
                "93541134780",
                [new CriarPedidoRequest_ItemPedidoAux(produto.Id, 1)]));

        Assert.Equal(HttpStatusCode.Created, pedidoResponse.StatusCode);
        var pedido = Assert.IsType<CriarPedidoResponse>(
            pedidoResponse.Content?.Dados);

        return pedido.Id;
    }
}
