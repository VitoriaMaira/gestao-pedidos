using System.Net;
using LojaPedidos.Application.Common.Responses;
using LojaPedidos.Application.Pedidos.AtualizarStatusPedido;
using LojaPedidos.Application.Pedidos.CriarPedido;
using LojaPedidos.Application.Produtos.Criar;
using LojaPedidos.Domain.Enums;
using LojaPedidos.IntegrationTests.Configurations;

namespace LojaPedidos.IntegrationTests.Features.Pedidos;

[Collection(IntegrationTestsCollection.Name)]
public sealed class AtualizarStatusPedidoTests(AspireAppFixture fixture)
{
    [Fact]
    public async Task DeveAtualizarStatusDoPedidoParaProcessado()
    {
        var pedidoId = await CriarPedidoAsync();

        try
        {
            var response = await fixture.Api.Pedidos.AtualizarStatusAsync(
                pedidoId,
                new AtualizarStatusPedidoRequest(StatusPedido.Processado));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var apiResponse = Assert.IsType<ApiResponse<AtualizarStatusPedidoResponse>>(
                response.Content);
            Assert.True(apiResponse.Sucesso);
            Assert.Equal("Status do pedido atualizado com sucesso.", apiResponse.Mensagem);

            var resultado = Assert.IsType<AtualizarStatusPedidoResponse>(apiResponse.Dados);
            Assert.Equal(StatusPedido.Processado, resultado.Pedido.Status);
        }
        finally
        {
            await fixture.Api.Pedidos.ExcluirAsync(pedidoId);
        }
    }

    [Fact]
    public async Task DeveRejeitarTransicaoDeIniciadoParaEnviado()
    {
        var pedidoId = await CriarPedidoAsync();

        try
        {
            var response = await fixture.Api.Pedidos.AtualizarStatusAsync(
                pedidoId,
                new AtualizarStatusPedidoRequest(StatusPedido.Enviado));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.IsType<Refit.ApiException>(response.Error);
        }
        finally
        {
            await fixture.Api.Pedidos.ExcluirAsync(pedidoId);
        }
    }

    [Fact]
    public async Task DeveRetornarNotFoundQuandoPedidoNaoExistir()
    {
        var response = await fixture.Api.Pedidos.AtualizarStatusAsync(
            Guid.CreateVersion7(),
            new AtualizarStatusPedidoRequest(StatusPedido.Processado));

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.IsType<Refit.ApiException>(response.Error);
    }

    private async Task<Guid> CriarPedidoAsync()
    {
        var produtoResponse = await fixture.Api.Produtos.CriarAsync(
            new CriarProdutoRequest($"Produto para status {Guid.CreateVersion7()}", 40m));
        Assert.Equal(HttpStatusCode.Created, produtoResponse.StatusCode);
        var produto = Assert.IsType<CriarProdutoResponse>(produtoResponse.Content?.Dados);

        var pedidoResponse = await fixture.Api.Pedidos.CriarAsync(
            new CriarPedidoRequest(
                "Comprador para status",
                "93541134780",
                [new CriarPedidoRequest_ItemPedidoAux(produto.Id, 1)]));
        Assert.Equal(HttpStatusCode.Created, pedidoResponse.StatusCode);
        var pedido = Assert.IsType<CriarPedidoResponse>(pedidoResponse.Content?.Dados);

        return pedido.Id;
    }
}
