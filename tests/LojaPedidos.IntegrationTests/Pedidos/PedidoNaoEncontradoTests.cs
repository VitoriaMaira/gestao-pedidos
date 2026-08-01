using System.Net;
using LojaPedidos.Application.Pedidos.AlterarPedido;
using LojaPedidos.Application.Pedidos.AtualizarStatusPedido;
using LojaPedidos.Domain.Enums;

namespace LojaPedidos.IntegrationTests.Pedidos;

public sealed class PedidoNaoEncontradoTests
{
    private readonly IPedidosApi _api = PedidosApiClient.Criar();

    [Fact]
    public async Task ObterPorId_DeveRetornarNotFound_QuandoPedidoNaoExistir()
    {
        var response = await _api.ObterPorIdAsync(Guid.CreateVersion7());

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Alterar_DeveRetornarNotFound_QuandoPedidoNaoExistir()
    {
        var request = new AlterarPedidoRequest(
            [new AlterarItemPedidoRequest(Guid.CreateVersion7(), 1)]);

        var response = await _api.AlterarAsync(Guid.CreateVersion7(), request);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task AtualizarStatus_DeveRetornarNotFound_QuandoPedidoNaoExistir()
    {
        var request = new AtualizarStatusPedidoRequest(StatusPedido.Processado);

        var response = await _api.AtualizarStatusAsync(Guid.CreateVersion7(), request);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Excluir_DeveRetornarNotFound_QuandoPedidoNaoExistir()
    {
        var response = await _api.ExcluirAsync(Guid.CreateVersion7());

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
