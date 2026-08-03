using LojaPedidos.Application.Produtos.Listar;

namespace LojaPedidos.UnitTests.Application.Produtos;

public sealed class ListarProdutosQueryValidatorTests
{
    private readonly ListarProdutosQueryValidator _validator = new();

    [Theory]
    [InlineData(0, 10, nameof(ListarProdutosQuery.Pagina))]
    [InlineData(1, 0, nameof(ListarProdutosQuery.TamanhoPagina))]
    [InlineData(1, 101, nameof(ListarProdutosQuery.TamanhoPagina))]
    public async Task Validar_DeveFalhar_QuandoPaginacaoForInvalida(
        int pagina,
        int tamanhoPagina,
        string propriedadeInvalida)
    {
        var request = new ListarProdutosQuery(pagina, tamanhoPagina);

        var resultado = await _validator.ValidateAsync(request);

        Assert.Contains(
            resultado.Errors,
            erro => erro.PropertyName == propriedadeInvalida);
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(1, 100)]
    public async Task Validar_DeveSerValido_QuandoPaginacaoForValida(
        int pagina,
        int tamanhoPagina)
    {
        var request = new ListarProdutosQuery(pagina, tamanhoPagina);

        var resultado = await _validator.ValidateAsync(request);

        Assert.True(resultado.IsValid);
    }
}
