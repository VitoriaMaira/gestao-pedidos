using LojaPedidos.Application.Pedidos.CriarPedido;

namespace LojaPedidos.UnitTests.Application;

public sealed class CriarPedidoRequestValidatorTests
{
    private readonly CriarPedidoRequestValidator _validator = new();

    [Fact]
    public async Task Validar_DeveSerValido_QuandoRequestEstiverCorreto()
    {
        var request = new CriarPedidoRequest(
            Guid.CreateVersion7(),
            [new CriarItemPedidoRequest(Guid.CreateVersion7(), 2)]);

        var resultado = await _validator.ValidateAsync(request);

        Assert.True(resultado.IsValid);
    }

    [Fact]
    public async Task Validar_DeveFalhar_QuandoCompradorNaoForInformado()
    {
        var request = new CriarPedidoRequest(
            Guid.Empty,
            [new CriarItemPedidoRequest(Guid.CreateVersion7(), 1)]);

        var resultado = await _validator.ValidateAsync(request);

        Assert.Contains(
            resultado.Errors,
            erro => erro.PropertyName == nameof(CriarPedidoRequest.CompradorId));
    }

    [Fact]
    public async Task Validar_DeveFalhar_QuandoNaoExistiremItens()
    {
        var request = new CriarPedidoRequest(Guid.CreateVersion7(), []);

        var resultado = await _validator.ValidateAsync(request);

        Assert.Contains(
            resultado.Errors,
            erro => erro.PropertyName == nameof(CriarPedidoRequest.Itens));
    }

    [Fact]
    public async Task Validar_DeveFalhar_QuandoItemForInvalido()
    {
        var request = new CriarPedidoRequest(
            Guid.CreateVersion7(),
            [new CriarItemPedidoRequest(Guid.Empty, 0)]);

        var resultado = await _validator.ValidateAsync(request);

        Assert.Contains(resultado.Errors, erro => erro.PropertyName == "Itens[0].ProdutoId");
        Assert.Contains(resultado.Errors, erro => erro.PropertyName == "Itens[0].Quantidade");
    }
}
