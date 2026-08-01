using LojaPedidos.Application.Pedidos.CriarPedido;

namespace LojaPedidos.UnitTests.Application;

public sealed class CriarPedidoRequestValidatorTests
{
    private readonly CriarPedidoRequestValidator _validator = new();

    [Fact]
    public async Task Validar_DeveSerValido_QuandoRequestEstiverCorreto()
    {
        var request = new CriarPedidoRequest(
            new CriarCompradorRequest("João da Silva", "12345678909"),
            [new CriarItemPedidoRequest(new CriarProdutoRequest("Teclado", 150m), 2)]);

        var resultado = await _validator.ValidateAsync(request);

        Assert.True(resultado.IsValid);
    }

    [Fact]
    public async Task Validar_DeveFalhar_QuandoCompradorNaoForInformado()
    {
        var request = new CriarPedidoRequest(
            null,
            [new CriarItemPedidoRequest(new CriarProdutoRequest("Teclado", 150m), 1)]);

        var resultado = await _validator.ValidateAsync(request);

        Assert.Contains(
            resultado.Errors,
            erro => erro.PropertyName == nameof(CriarPedidoRequest.Comprador));
    }

    [Fact]
    public async Task Validar_DeveFalhar_QuandoCpfForInvalido()
    {
        var request = new CriarPedidoRequest(
            new CriarCompradorRequest("João da Silva", "11111111111"),
            [new CriarItemPedidoRequest(new CriarProdutoRequest("Teclado", 150m), 1)]);

        var resultado = await _validator.ValidateAsync(request);

        Assert.Contains(resultado.Errors, erro => erro.PropertyName == "Comprador.Cpf");
    }

    [Fact]
    public async Task Validar_DeveFalhar_QuandoNaoExistiremItens()
    {
        var request = new CriarPedidoRequest(
            new CriarCompradorRequest("João da Silva", "12345678909"),
            []);

        var resultado = await _validator.ValidateAsync(request);

        Assert.Contains(
            resultado.Errors,
            erro => erro.PropertyName == nameof(CriarPedidoRequest.Itens));
    }

    [Fact]
    public async Task Validar_DeveFalhar_QuandoItemForInvalido()
    {
        var request = new CriarPedidoRequest(
            new CriarCompradorRequest("João da Silva", "12345678909"),
            [new CriarItemPedidoRequest(null, 0)]);

        var resultado = await _validator.ValidateAsync(request);

        Assert.Contains(resultado.Errors, erro => erro.PropertyName == "Itens[0].Produto");
        Assert.Contains(resultado.Errors, erro => erro.PropertyName == "Itens[0].Quantidade");
    }

    [Fact]
    public async Task Validar_DeveFalhar_QuandoProdutoForInvalido()
    {
        var request = new CriarPedidoRequest(
            new CriarCompradorRequest("João da Silva", "12345678909"),
            [new CriarItemPedidoRequest(new CriarProdutoRequest("", 0), 1)]);

        var resultado = await _validator.ValidateAsync(request);

        Assert.Contains(resultado.Errors, erro => erro.PropertyName == "Itens[0].Produto.Nome");
        Assert.Contains(resultado.Errors, erro => erro.PropertyName == "Itens[0].Produto.Preco");
    }
}
