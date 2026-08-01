using System.Net;
using LojaPedidos.Application.Pedidos.AtualizarStatusPedido;
using LojaPedidos.Application.Pedidos.CriarPedido;
using LojaPedidos.Application.Pedidos.ListarPedidos;
using LojaPedidos.Domain.Enums;

namespace LojaPedidos.IntegrationTests.Pedidos;

public sealed class ListarPedidosTests
{
    private const string CpfComprador = "52998224725";
    private readonly IPedidosApi _api = PedidosApiClient.Criar();

    [Fact]
    public async Task DeveListarPedidosDeFormaPaginadaEFiltrarPorCpf()
    {
        var primeiroPedido = await CriarPedidoAsync();
        var segundoPedido = await CriarPedidoAsync();

        try
        {
            var request = new ListarPedidosRequest(
                Pagina: 1,
                TamanhoPagina: 1,
                Cpf: CpfComprador);

            var response = await _api.ListarAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response.Content);
            Assert.Equal(1, response.Content.Pagina);
            Assert.Equal(1, response.Content.TamanhoPagina);
            Assert.Equal(2, response.Content.Total);
            Assert.Single(response.Content.Itens);
        }
        finally
        {
            await ExcluirPedidoAsync(primeiroPedido.Id);
            await ExcluirPedidoAsync(segundoPedido.Id);
        }
    }

    [Fact]
    public async Task DeveFiltrarPedidosPorStatus()
    {
        var pedido = await CriarPedidoAsync();

        try
        {
            await _api.AtualizarStatusAsync(
                pedido.Id,
                new AtualizarStatusPedidoRequest(StatusPedido.Cancelado));

            var request = new ListarPedidosRequest(
                Status: StatusPedido.Cancelado,
                Cpf: CpfComprador);

            var response = await _api.ListarAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response.Content);
            Assert.Equal(1, response.Content.Total);
            var pedidoListado = Assert.Single(response.Content.Itens);
            Assert.Equal(pedido.Id, pedidoListado.Id);
            Assert.Equal(StatusPedido.Cancelado, pedidoListado.Status);
        }
        finally
        {
            await ExcluirPedidoAsync(pedido.Id);
        }
    }

    [Fact]
    public async Task DeveRetornarBadRequestQuandoTamanhoDaPaginaForInvalido()
    {
        var request = new ListarPedidosRequest(TamanhoPagina: 101);

        var response = await _api.ListarAsync(request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    private async Task<CriarPedidoResponse> CriarPedidoAsync()
    {
        var request = new CriarPedidoRequest(
            new CriarCompradorRequest("Comprador da listagem", CpfComprador),
            [
                new CriarItemPedidoRequest(
                    new CriarProdutoRequest($"Produto {Guid.CreateVersion7()}", 100m),
                    1)
            ]);
        var response = await _api.CriarAsync(request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(response.Content);

        return response.Content;
    }

    private async Task ExcluirPedidoAsync(Guid id)
    {
        var response = await _api.ExcluirAsync(id);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
