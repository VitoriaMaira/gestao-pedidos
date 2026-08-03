using System.Net;
using LojaPedidos.Application.Pedidos.AlterarPedido;
using LojaPedidos.Application.Pedidos.AtualizarStatusPedido;
using LojaPedidos.Application.Pedidos.CriarPedido;
using LojaPedidos.Application.Pedidos.ConsultarPedido;
using LojaPedidos.Application.Produtos.Criar;
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
            Assert.Equal(
                Assert.Single(pedido.Itens).Id,
                Assert.Single(response.Content.Itens).Id);
        }
        finally
        {
            await ExcluirPedidoAsync(pedido.Id);
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
            await ExcluirPedidoAsync(pedido.Id);
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
            await ExcluirPedidoAsync(pedido.Id);
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
            await ExcluirPedidoAsync(pedido.Id);
        }
    }

    [Fact]
    public async Task DeveCancelarPedidoProcessado()
    {
        var pedido = await CriarPedidoAsync();

        try
        {
            await _api.AtualizarStatusAsync(
                pedido.Id,
                new AtualizarStatusPedidoRequest(StatusPedido.Processado));

            var response = await _api.AtualizarStatusAsync(
                pedido.Id,
                new AtualizarStatusPedidoRequest(StatusPedido.Cancelado));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(StatusPedido.Cancelado, response.Content?.Pedido.Status);
        }
        finally
        {
            await ExcluirPedidoAsync(pedido.Id);
        }
    }

    [Fact]
    public async Task DeveRejeitarAlteracaoDePedidoProcessado()
    {
        var pedido = await CriarPedidoAsync();
        var item = Assert.Single(pedido.Itens);

        try
        {
            await _api.AtualizarStatusAsync(
                pedido.Id,
                new AtualizarStatusPedidoRequest(StatusPedido.Processado));

            var request = new AlterarPedidoRequest(
                [new AlterarItemPedidoRequest(item.Id, 2)]);
            var response = await _api.AlterarAsync(pedido.Id, request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        finally
        {
            await ExcluirPedidoAsync(pedido.Id);
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
            await ExcluirPedidoAsync(pedido.Id);
        }
    }

    [Fact]
    public async Task DeveRejeitarEnvioDePedidoCancelado()
    {
        var pedido = await CriarPedidoAsync();

        try
        {
            await _api.AtualizarStatusAsync(
                pedido.Id,
                new AtualizarStatusPedidoRequest(StatusPedido.Cancelado));

            var response = await _api.AtualizarStatusAsync(
                pedido.Id,
                new AtualizarStatusPedidoRequest(StatusPedido.Enviado));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        finally
        {
            await ExcluirPedidoAsync(pedido.Id);
        }
    }

    [Fact]
    public async Task DeveRetornarNotFoundAoConsultarPedidoExcluido()
    {
        var pedido = await CriarPedidoAsync();

        await ExcluirPedidoAsync(pedido.Id);
        var response = await _api.ObterPorIdAsync(pedido.Id);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    private async Task<PedidoResponse> CriarPedidoAsync(decimal preco = 150m)
    {
        var produtoResponse = await _api.CriarProdutoAsync(
            new CriarProdutoRequest($"Produto {Guid.CreateVersion7()}", preco));
        Assert.Equal(HttpStatusCode.Created, produtoResponse.StatusCode);
        var produto = Assert.IsType<CriarProdutoResponse>(produtoResponse.Content);

        var request = new CriarPedidoRequest(
            "Comprador dos testes",
            "12345678909",
            [new CriarPedidoRequest_ItemPedidoAux(produto.Id, 1)]);
        var response = await _api.CriarAsync(request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(response.Content);
        var headers = Assert.IsType<System.Net.Http.Headers.HttpResponseHeaders>(
            response.Headers);
        var location = Assert.IsType<Uri>(headers.Location);
        Assert.EndsWith(
            $"/api/pedidos/{response.Content.Id}",
            location.OriginalString);

        var consulta = await _api.ObterPorIdAsync(response.Content.Id);
        Assert.Equal(HttpStatusCode.OK, consulta.StatusCode);
        return Assert.IsType<PedidoResponse>(consulta.Content);
    }

    private async Task ExcluirPedidoAsync(Guid id)
    {
        var response = await _api.ExcluirAsync(id);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Pedido excluído com sucesso.", response.Content?.Mensagem);
    }
}
