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
        var apiResponse = Assert.IsType<LojaPedidos.Application.Common.Responses.ApiResponse<CriarProdutoResponse>>(
            response.Content);
        Assert.True(apiResponse.Sucesso);
        var produto = Assert.IsType<CriarProdutoResponse>(apiResponse.Dados);
        Assert.NotEqual(Guid.Empty, produto.Id);
    }

    [Theory]
    [InlineData("", 10, "nome")]
    [InlineData("Produto inválido", 0, "preço")]
    [InlineData("Produto inválido", -1, "preço")]
    public async Task DeveRejeitarProdutoComDadosInvalidos(
        string nome,
        decimal preco,
        string propriedadeInvalida)
    {
        var response = await fixture.Api.Produtos.CriarAsync(
            new CriarProdutoRequest(nome, preco));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var erro = Assert.IsType<Refit.ApiException>(response.Error);
        Assert.Contains(propriedadeInvalida, erro.Content, StringComparison.OrdinalIgnoreCase);
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
