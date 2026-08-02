using LojaPedidos.IntegrationTests.Configurations;
using System.Net;

namespace LojaPedidos.IntegrationTests.Features.Produtos;

[Collection(IntegrationTestsCollection.Name)]
public sealed class CriarProdutoTests(AspireAppFixture fixture)
{
    [Fact]
    public async Task DeveCriarProdutoComDadosValidos()
    {
        var request = NovoProduto();
        var response = await fixture.Api.Produtos.CriarAsync(request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var produto = Assert.IsType<CriarProdutoResponse>(response.Content);
        Assert.NotEqual(Guid.Empty, produto.Id);
    }

    [Theory]
    [InlineData("", 10, "Nome")]
    [InlineData("Produto inválido", 0, "Preco")]
    [InlineData("Produto inválido", -1, "Preco")]
    public async Task DeveRejeitarProdutoComDadosInvalidos(
        string nome,
        decimal preco,
        string propriedadeInvalida)
    {
        var response = await fixture.Api.Produtos.CriarAsync(
            new CriarProdutoRequest(nome, preco));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var erro = Assert.IsType<Refit.ValidationApiException>(response.Error);
        var problema = erro.Content;
        Assert.NotNull(problema);
        Assert.Contains(propriedadeInvalida, problema.Errors.Keys);
    }

    [Fact]
    public async Task DeveRejeitarProdutoComNomeDuplicado()
    {
        var request = NovoProduto();
        var primeiraResposta = await fixture.Api.Produtos.CriarAsync(request);
        var segundaResposta = await fixture.Api.Produtos.CriarAsync(request);

        Assert.Equal(HttpStatusCode.Created, primeiraResposta.StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, segundaResposta.StatusCode);
    }

    private static CriarProdutoRequest NovoProduto() =>
        new($"Produto de integração {Guid.CreateVersion7()}", 149.90m);
}
