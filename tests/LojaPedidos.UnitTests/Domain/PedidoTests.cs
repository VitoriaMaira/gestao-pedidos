using LojaPedidos.Domain.Entities;
using LojaPedidos.Domain.Enums;
using LojaPedidos.Domain.Exceptions;

namespace LojaPedidos.UnitTests.Domain;

public sealed class PedidoTests
{
    [Fact]
    public void Criar_DeveIniciarPedido_QuandoDadosForemValidos()
    {
        var comprador = CriarComprador();
        var item = CriarItem();

        var pedido = new Pedido(comprador, [item]);

        Assert.NotEqual(Guid.Empty, pedido.Id);
        Assert.Equal(comprador.Id, pedido.CompradorId);
        Assert.Equal(StatusPedido.Iniciado, pedido.Status);
        Assert.Single(pedido.Itens);
        Assert.Equal(399.80m, pedido.Total);
    }

    [Fact]
    public void Criar_DeveFalhar_QuandoCompradorNaoForInformado()
    {
        var excecao = Assert.Throws<DomainException>(() => new Pedido(null!, [CriarItem()]));

        Assert.Equal("O comprador é obrigatório.", excecao.Message);
    }

    [Fact]
    public void Criar_DeveFalhar_QuandoNaoExistiremItens()
    {
        var excecao = Assert.Throws<DomainException>(() => new Pedido(CriarComprador(), []));

        Assert.Equal("O pedido deve possuir pelo menos um item.", excecao.Message);
    }

    [Fact]
    public void AlterarQuantidadeItem_DeveAlterarQuantidadeERecalcularTotal()
    {
        var pedido = CriarPedido();
        var item = Assert.Single(pedido.Itens);

        pedido.AlterarQuantidadeItem(item.Id, 3);

        Assert.Equal(3, item.Quantidade);
        Assert.Equal(599.70m, pedido.Total);
        Assert.NotNull(pedido.AtualizadoEm);
    }

    [Fact]
    public void AlterarQuantidadeItem_DeveFalhar_QuandoItemNaoPertencerAoPedido()
    {
        var pedido = CriarPedido();

        var excecao = Assert.Throws<DomainException>(
            () => pedido.AlterarQuantidadeItem(Guid.CreateVersion7(), 3));

        Assert.Equal("O item informado não pertence ao pedido.", excecao.Message);
    }

    [Fact]
    public void AlterarQuantidadeItem_DeveFalhar_QuandoPedidoJaEstiverProcessado()
    {
        var pedido = CriarPedido();
        var item = Assert.Single(pedido.Itens);
        pedido.Processar();

        var excecao = Assert.Throws<DomainException>(
            () => pedido.AlterarQuantidadeItem(item.Id, 3));

        Assert.Equal("Apenas pedidos não processados podem ser alterados.", excecao.Message);
    }

    [Fact]
    public void Cancelar_DeveCancelar_QuandoPedidoEstiverProcessado()
    {
        var pedido = CriarPedido();
        pedido.Processar();

        pedido.Cancelar();

        Assert.Equal(StatusPedido.Cancelado, pedido.Status);
    }

    [Fact]
    public void Cancelar_DeveCancelar_QuandoPedidoEstiverIniciado()
    {
        var pedido = CriarPedido();

        pedido.Cancelar();

        Assert.Equal(StatusPedido.Cancelado, pedido.Status);
        Assert.NotNull(pedido.AtualizadoEm);
    }

    [Fact]
    public void Cancelar_DeveFalhar_QuandoPedidoEstiverEnviado()
    {
        var pedido = CriarPedido();
        pedido.Processar();
        pedido.Enviar();

        var excecao = Assert.Throws<DomainException>(() => pedido.Cancelar());

        Assert.Equal(
            "Apenas pedidos iniciados ou processados podem ser cancelados.",
            excecao.Message);
    }

    [Fact]
    public void Enviar_DeveEnviar_QuandoPedidoEstiverProcessado()
    {
        var pedido = CriarPedido();
        pedido.Processar();

        pedido.Enviar();

        Assert.Equal(StatusPedido.Enviado, pedido.Status);
    }

    [Fact]
    public void Enviar_DeveFalhar_QuandoPedidoNaoEstiverProcessado()
    {
        var pedido = CriarPedido();

        var excecao = Assert.Throws<DomainException>(() => pedido.Enviar());

        Assert.Equal("Apenas pedidos processados podem ser enviados.", excecao.Message);
    }

    private static Pedido CriarPedido()
    {
        return new Pedido(CriarComprador(), [CriarItem()]);
    }

    private static Comprador CriarComprador()
    {
        return new Comprador("Maíra", "12345678909");
    }

    private static Produto CriarProduto()
    {
        return new Produto("Teclado", 199.90m);
    }

    private static ItemPedido CriarItem()
    {
        return new ItemPedido(CriarProduto(), 2);
    }
}
