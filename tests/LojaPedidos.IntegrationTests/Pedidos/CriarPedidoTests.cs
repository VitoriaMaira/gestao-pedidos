using System.Net;
using LojaPedidos.Application.Pedidos.CriarPedido;

namespace LojaPedidos.IntegrationTests.Pedidos;

public sealed class CriarPedidoTests
{
    private readonly IPedidosApi _api = PedidosApiClient.Criar();

    [Fact]
    public async Task DeveRetornarBadRequestQuandoPedidoForInvalido()
    {
        var request = new CriarPedidoRequest(null, []);

        var response = await _api.CriarAsync(request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task DeveReutilizarCompradorQuandoCpfJaEstiverCadastrado()
    {
        const string cpf = "11144477735";
        var primeiroPedido = await CriarPedidoAsync("Primeiro nome", cpf);
        var segundoPedido = await CriarPedidoAsync("Nome informado novamente", cpf);

        try
        {
            Assert.Equal(primeiroPedido.Comprador.Id, segundoPedido.Comprador.Id);
            Assert.Equal("Primeiro nome", segundoPedido.Comprador.Nome);
            Assert.Contains("reutilizado", segundoPedido.Mensagem);
        }
        finally
        {
            await _api.ExcluirAsync(primeiroPedido.Id);
            await _api.ExcluirAsync(segundoPedido.Id);
        }
    }

    private async Task<CriarPedidoResponse> CriarPedidoAsync(string comprador, string cpf)
    {
        var request = new CriarPedidoRequest(
            new CriarCompradorRequest(comprador, cpf),
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
}
