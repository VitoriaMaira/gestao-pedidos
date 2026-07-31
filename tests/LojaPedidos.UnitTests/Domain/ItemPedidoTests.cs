using LojaPedidos.Domain.Entities;
using LojaPedidos.Domain.Exceptions;

namespace LojaPedidos.UnitTests.Domain;

public sealed class ItemPedidoTests
{
    [Fact]
    public void Criar_DeveCopiarPrecoECalcularSubtotal_QuandoDadosForemValidos()
    {
        var produto = new Produto("Teclado", 199.90m);

        var item = new ItemPedido(produto, 2);

        Assert.Equal(produto.Id, item.ProdutoId);
        Assert.Equal(2, item.Quantidade);
        Assert.Equal(199.90m, item.PrecoUnitario);
        Assert.Equal(399.80m, item.Subtotal);
    }

    [Fact]
    public void Criar_DeveManterPrecoOriginal_QuandoProdutoForAlterado()
    {
        var produto = new Produto("Teclado", 199.90m);
        var item = new ItemPedido(produto, 1);

        produto.Alterar("Teclado", 249.90m);

        Assert.Equal(199.90m, item.PrecoUnitario);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Criar_DeveFalhar_QuandoQuantidadeNaoForPositiva(int quantidade)
    {
        var produto = new Produto("Teclado", 199.90m);

        var excecao = Assert.Throws<DomainException>(
            () => new ItemPedido(produto, quantidade));

        Assert.Equal("A quantidade deve ser maior que zero.", excecao.Message);
    }
}
