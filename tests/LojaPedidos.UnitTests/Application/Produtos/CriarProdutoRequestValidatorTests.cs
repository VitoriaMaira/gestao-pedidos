using LojaPedidos.Application.Produtos.Criar;

namespace LojaPedidos.UnitTests.Application.Produtos;

public sealed class CriarProdutoRequestValidatorTests
{
    private readonly CriarProdutoRequestValidator _validator = new();

    [Fact]
    public async Task Validar_DeveSerValido_QuandoProdutoEstiverCorreto()
    {
        var request = new CriarProdutoRequest("Teclado mecânico", 150m);

        var resultado = await _validator.ValidateAsync(request);

        Assert.True(resultado.IsValid);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task Validar_DeveFalhar_QuandoNomeNaoForInformado(string nome)
    {
        var request = new CriarProdutoRequest(nome, 150m);

        var resultado = await _validator.ValidateAsync(request);

        Assert.Contains(
            resultado.Errors,
            erro => erro.PropertyName == nameof(CriarProdutoRequest.Nome));
    }

    [Fact]
    public async Task Validar_DeveFalhar_QuandoNomeExcederLimite()
    {
        var request = new CriarProdutoRequest(new string('a', 151), 150m);

        var resultado = await _validator.ValidateAsync(request);

        Assert.Contains(
            resultado.Errors,
            erro => erro.PropertyName == nameof(CriarProdutoRequest.Nome));
    }

    [Fact]
    public async Task Validar_DeveSerValido_QuandoNomeEstiverNoLimite()
    {
        var request = new CriarProdutoRequest(new string('a', 150), 150m);

        var resultado = await _validator.ValidateAsync(request);

        Assert.True(resultado.IsValid);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task Validar_DeveFalhar_QuandoPrecoNaoForPositivo(decimal preco)
    {
        var request = new CriarProdutoRequest("Teclado mecânico", preco);

        var resultado = await _validator.ValidateAsync(request);

        Assert.Contains(
            resultado.Errors,
            erro => erro.PropertyName == nameof(CriarProdutoRequest.Preco));
    }
}
