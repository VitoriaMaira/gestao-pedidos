using LojaPedidos.Application.Pedidos.CriarPedido;

namespace LojaPedidos.UnitTests.Application.Pedidos;

public sealed class CriarPedidoRequestValidatorTests
{
    private readonly CriarPedidoRequestValidator _validator = new();

    [Fact]
    public async Task Validar_DeveSerValido_QuandoRequestEstiverCorreto()
    {
        var request = CriarRequestValido();

        var resultado = await _validator.ValidateAsync(request);

        Assert.True(resultado.IsValid);
    }

    [Fact]
    public async Task Validar_DeveFalhar_QuandoNomeDoCompradorNaoForInformado()
    {
        var request = CriarRequestValido() with { NomeComprador = string.Empty };

        var resultado = await _validator.ValidateAsync(request);

        Assert.Contains(
            resultado.Errors,
            erro => erro.PropertyName == nameof(CriarPedidoRequest.NomeComprador));
    }

    [Fact]
    public async Task Validar_DeveFalhar_QuandoCpfDoCompradorNaoForInformado()
    {
        var request = CriarRequestValido() with { CpfComprador = string.Empty };

        var resultado = await _validator.ValidateAsync(request);

        Assert.Contains(
            resultado.Errors,
            erro => erro.PropertyName == nameof(CriarPedidoRequest.CpfComprador));
    }

    [Fact]
    public async Task Validar_DeveFalhar_QuandoNaoExistiremItens()
    {
        var request = CriarRequestValido() with { Itens = [] };

        var resultado = await _validator.ValidateAsync(request);

        Assert.Contains(
            resultado.Errors,
            erro => erro.PropertyName == nameof(CriarPedidoRequest.Itens));
    }

    [Fact]
    public async Task Validar_DeveFalhar_QuandoIdDoProdutoForVazio()
    {
        var request = CriarRequestValido() with
        {
            Itens = [new CriarPedidoRequest_ItemPedidoAux(Guid.Empty, 1)]
        };

        var resultado = await _validator.ValidateAsync(request);

        Assert.Contains(
            resultado.Errors,
            erro => erro.PropertyName == "Itens[0].Id");
    }

    [Fact]
    public async Task Validar_DeveFalhar_QuandoProdutoForRepetido()
    {
        var produtoId = Guid.CreateVersion7();
        var request = CriarRequestValido() with
        {
            Itens =
            [
                new CriarPedidoRequest_ItemPedidoAux(produtoId, 1),
                new CriarPedidoRequest_ItemPedidoAux(produtoId, 2)
            ]
        };

        var resultado = await _validator.ValidateAsync(request);

        Assert.Contains(
            resultado.Errors,
            erro => erro.PropertyName == nameof(CriarPedidoRequest.Itens));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task Validar_DeveFalhar_QuandoQuantidadeNaoForPositiva(
        int quantidade)
    {
        var request = CriarRequestValido() with
        {
            Itens = [new CriarPedidoRequest_ItemPedidoAux(Guid.CreateVersion7(), quantidade)]
        };

        var resultado = await _validator.ValidateAsync(request);

        Assert.Contains(
            resultado.Errors,
            erro => erro.PropertyName == "Itens[0].Quantidade");
    }

    [Fact]
    public async Task Validar_DeveIdentificarIndiceDeCadaItemInvalido()
    {
        var request = CriarRequestValido() with
        {
            Itens =
            [
                new CriarPedidoRequest_ItemPedidoAux(Guid.Empty, 1),
                new CriarPedidoRequest_ItemPedidoAux(Guid.CreateVersion7(), 0)
            ]
        };

        var resultado = await _validator.ValidateAsync(request);

        Assert.Contains(resultado.Errors, erro => erro.PropertyName == "Itens[0].Id");
        Assert.Contains(
            resultado.Errors,
            erro => erro.PropertyName == "Itens[1].Quantidade");
    }

    private static CriarPedidoRequest CriarRequestValido() =>
        new(
            "João da Silva",
            "12345678909",
            [new CriarPedidoRequest_ItemPedidoAux(Guid.CreateVersion7(), 1)]);
}
