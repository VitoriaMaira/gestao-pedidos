using LojaPedidos.Domain.Entities;
using LojaPedidos.Domain.Exceptions;

namespace LojaPedidos.UnitTests.Domain;

public sealed class ProdutoTests
{
    [Fact]
    public void Criar_DeveCriarProduto_QuandoDadosForemValidos()
    {
        var produto = new Produto("Teclado", 199.90m);

        Assert.NotEqual(Guid.Empty, produto.Id);
        Assert.Equal("Teclado", produto.Nome);
        Assert.Equal(199.90m, produto.Preco);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Criar_DeveFalhar_QuandoPrecoNaoForPositivo(decimal preco)
    {
        var excecao = Assert.Throws<DomainException>(() => new Produto("Teclado", preco));

        Assert.Equal("O preço do produto deve ser maior que zero.", excecao.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Criar_DeveFalhar_QuandoNomeForVazio(string nome)
    {
        var excecao = Assert.Throws<DomainException>(() => new Produto(nome, 10m));

        Assert.Equal("O nome do produto é obrigatório.", excecao.Message);
    }
}
