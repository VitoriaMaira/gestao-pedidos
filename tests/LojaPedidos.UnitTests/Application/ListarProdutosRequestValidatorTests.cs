using LojaPedidos.Application.Produtos.Listar;

namespace LojaPedidos.UnitTests.Application;

public sealed class ListarProdutosRequestValidatorTests
{
    private readonly ListarProdutosQueryValidator _validator = new();

    [Theory]
    [InlineData(0, 10)]
    [InlineData(1, 0)]
    [InlineData(1, 101)]
    public async Task Validar_DeveFalhar_QuandoPaginacaoForInvalida(
        int pagina,
        int tamanhoPagina)
    {
        var request = new ListarProdutosQuery(pagina, tamanhoPagina);

        var resultado = await _validator.ValidateAsync(request);

        Assert.False(resultado.IsValid);
    }

    [Fact]
    public async Task Validar_DeveSerValido_QuandoPaginacaoForValida()
    {
        var request = new ListarProdutosQuery(1, 100);

        var resultado = await _validator.ValidateAsync(request);

        Assert.True(resultado.IsValid);
    }
}
