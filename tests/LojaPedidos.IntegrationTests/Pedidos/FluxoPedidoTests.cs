using System.Net;
using LojaPedidos.Application.Pedidos.AlterarPedido;
using LojaPedidos.Application.Pedidos.AtualizarStatusPedido;
using LojaPedidos.Application.Pedidos.CriarPedido;
using LojaPedidos.Domain.Enums;

namespace LojaPedidos.IntegrationTests.Pedidos;

public sealed class FluxoPedidoTests
{
    private readonly IPedidosApi _api = PedidosApiClient.Criar();

    [Fact]
    public async Task DeveCriarEConsultarPedido()
    {
        var pedido = await CriarPedidoAsync();

        try
        {
            var response = await _api.ObterPorIdAsync(pedido.Id);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response.Content);
            Assert.Equal(pedido.Id, response.Content.Id);
            Assert.Equal(StatusPedido.Iniciado, response.Content.Status);
        }
        finally
        {
            await _api.ExcluirAsync(pedido.Id);
        }
    }

    [Fact]
    public async Task DeveAlterarQuantidadeERecalcularTotal()
    {
        var pedido = await CriarPedidoAsync(preco: 100m);
        var item = Assert.Single(pedido.Itens);

        try
        {
            var request = new AlterarPedidoRequest(
                [new AlterarItemPedidoRequest(item.Id, 3)]);

            var response = await _api.AlterarAsync(pedido.Id, request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response.Content);
            Assert.Equal(300m, response.Content.Total);
            Assert.Equal(3, Assert.Single(response.Content.Itens).Quantidade);
        }
        finally
        {
            await _api.ExcluirAsync(pedido.Id);
        }
    }

    [Fact]
    public async Task DeveProcessarEEnviarPedido()
    {
        var pedido = await CriarPedidoAsync();

        try
        {
            var processado = await _api.AtualizarStatusAsync(
                pedido.Id,
                new AtualizarStatusPedidoRequest(StatusPedido.Processado));
            var enviado = await _api.AtualizarStatusAsync(
                pedido.Id,
                new AtualizarStatusPedidoRequest(StatusPedido.Enviado));

            Assert.Equal(HttpStatusCode.OK, processado.StatusCode);
            Assert.Equal(StatusPedido.Processado, processado.Content?.Pedido.Status);
            Assert.Equal(HttpStatusCode.OK, enviado.StatusCode);
            Assert.Equal(StatusPedido.Enviado, enviado.Content?.Pedido.Status);
        }
        finally
        {
            await _api.ExcluirAsync(pedido.Id);
        }
    }

    [Fact]
    public async Task DeveCancelarPedidoIniciado()
    {
        var pedido = await CriarPedidoAsync();

        try
        {
            var response = await _api.AtualizarStatusAsync(
                pedido.Id,
                new AtualizarStatusPedidoRequest(StatusPedido.Cancelado));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(StatusPedido.Cancelado, response.Content?.Pedido.Status);
        }
        finally
        {
            await _api.ExcluirAsync(pedido.Id);
        }
    }

    [Fact]
    public async Task DeveRejeitarEnvioDePedidoIniciado()
    {
        var pedido = await CriarPedidoAsync();

        try
        {
            var response = await _api.AtualizarStatusAsync(
                pedido.Id,
                new AtualizarStatusPedidoRequest(StatusPedido.Enviado));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        finally
        {
            await _api.ExcluirAsync(pedido.Id);
        }
    }

    private async Task<CriarPedidoResponse> CriarPedidoAsync(decimal preco = 150m)
    {
        var request = new CriarPedidoRequest(
            new CriarCompradorRequest("Comprador dos testes", "12345678909"),
            [
                new CriarItemPedidoRequest(
                    new CriarProdutoRequest($"Produto {Guid.CreateVersion7()}", preco),
                    1)
            ]);
        var response = await _api.CriarAsync(request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(response.Content);

        return response.Content;
    }
}
