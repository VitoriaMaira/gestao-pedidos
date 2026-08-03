using System.Net;
using LojaPedidos.Application.Common.Responses;
using LojaPedidos.Application.Pedidos.ConsultarPedido;
using LojaPedidos.Application.Pedidos.CriarPedido;
using LojaPedidos.Application.Produtos.Criar;
using LojaPedidos.Domain.Enums;
using LojaPedidos.IntegrationTests.Configurations;

namespace LojaPedidos.IntegrationTests.Features.Pedidos;

[Collection(IntegrationTestsCollection.Name)]
public sealed class ConsultarPedidoTests(AspireAppFixture fixture)
{
    [Fact]
    public async Task DeveConsultarPedidoPorId()
    {
        var pedidoId = await CriarPedidoAsync();

        try
        {
            var response = await fixture.Api.Pedidos.ConsultarAsync(pedidoId);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var apiResponse = Assert.IsType<ApiResponse<ConsultarPedidoResponse>>(
                response.Content);
            Assert.True(apiResponse.Sucesso);
            Assert.Equal("Pedido consultado com sucesso.", apiResponse.Mensagem);

            var pedido = Assert.IsType<ConsultarPedidoResponse>(apiResponse.Dados);
            Assert.Equal(pedidoId, pedido.Id);
            Assert.Equal(StatusPedido.Iniciado, pedido.Status);
            Assert.Single(pedido.Itens);
        }
        finally
        {
            await fixture.Api.Pedidos.ExcluirAsync(pedidoId);
        }
    }

    [Fact]
    public async Task DeveRetornarNotFoundQuandoPedidoNaoExistir()
    {
        var response = await fixture.Api.Pedidos.ConsultarAsync(
            Guid.CreateVersion7());

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.IsType<Refit.ApiException>(response.Error);
    }

    private async Task<Guid> CriarPedidoAsync()
    {
        var produtoResponse = await fixture.Api.Produtos.CriarAsync(
            new CriarProdutoRequest(
                $"Produto para consulta {Guid.CreateVersion7()}",
                120m));
        var produto = Assert.IsType<CriarProdutoResponse>(
            produtoResponse.Content?.Dados);

        var pedidoResponse = await fixture.Api.Pedidos.CriarAsync(
            new CriarPedidoRequest(
                "Comprador da consulta",
                "12345678909",
                [new CriarPedidoRequest_ItemPedidoAux(produto.Id, 1)]));
        var pedido = Assert.IsType<CriarPedidoResponse>(
            pedidoResponse.Content?.Dados);

        return pedido.Id;
    }
}
