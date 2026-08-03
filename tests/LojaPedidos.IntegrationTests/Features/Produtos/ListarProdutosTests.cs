using System.Net;
using LojaPedidos.Application.Produtos.Criar;
using LojaPedidos.Application.Produtos.Listar;
using LojaPedidos.IntegrationTests.Configurations;

namespace LojaPedidos.IntegrationTests.Features.Produtos;

[Collection(IntegrationTestsCollection.Name)]
public sealed class ListarProdutosTests(AspireAppFixture fixture)
{
    [Fact]
    public async Task DeveListarProdutosDeFormaPaginadaEOrdenadaPorNome()
    {
        var identificador = Guid.CreateVersion7();
        var nomeA = $"Produto A {identificador}";
        var nomeB = $"Produto B {identificador}";

        await CriarProdutoAsync(nomeB, 200m);
        await CriarProdutoAsync(nomeA, 100m);

        var response = await fixture.Api.Produtos.ListarAsync(
            new ListarProdutosQuery(Pagina: 1, TamanhoPagina: 100));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var apiResponse = Assert.IsType<LojaPedidos.Application.Common.Responses.ApiResponse<ListarProdutosResponse>>(
            response.Content);
        Assert.True(apiResponse.Sucesso);
        var resultado = Assert.IsType<ListarProdutosResponse>(apiResponse.Dados);
        Assert.Equal(1, resultado.Pagina);
        Assert.Equal(100, resultado.TamanhoPagina);
        Assert.True(resultado.Total >= 2);
        Assert.True(resultado.Itens.Count <= 100);

        var produtosCriados = resultado.Itens
            .Where(produto => produto.Nome == nomeA || produto.Nome == nomeB)
            .ToArray();

        Assert.Collection(
            produtosCriados,
            produto => Assert.Equal(nomeA, produto.Nome),
            produto => Assert.Equal(nomeB, produto.Nome));
    }

    [Theory]
    [InlineData(0, 10)]
    [InlineData(1, 0)]
    [InlineData(1, 101)]
    public async Task DeveRetornarBadRequestQuandoPaginacaoForInvalida(
        int pagina,
        int tamanhoPagina)
    {
        var response = await fixture.Api.Produtos.ListarAsync(
            new ListarProdutosQuery(pagina, tamanhoPagina));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.IsType<Refit.ApiException>(response.Error);
    }

    private async Task CriarProdutoAsync(string nome, decimal preco)
    {
        var response = await fixture.Api.Produtos.CriarAsync(
            new CriarProdutoRequest(nome, preco));

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
}
