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
        var produto = CriarProduto();

        var pedido = new Pedido(comprador, [produto]);

        Assert.NotEqual(Guid.Empty, pedido.Id);
        Assert.Equal(comprador.Id, pedido.CompradorId);
        Assert.Equal(StatusPedido.Iniciado, pedido.Status);
        Assert.Single(pedido.Produtos);
    }

    [Fact]
    public void Criar_DeveFalhar_QuandoCompradorNaoForInformado()
    {
        var excecao = Assert.Throws<DomainException>(() => new Pedido(null!, [CriarProduto()]));

        Assert.Equal("O comprador é obrigatório.", excecao.Message);
    }

    [Fact]
    public void Criar_DeveFalhar_QuandoNaoExistiremProdutos()
    {
        var excecao = Assert.Throws<DomainException>(() => new Pedido(CriarComprador(), []));

        Assert.Equal("O pedido deve possuir pelo menos um produto.", excecao.Message);
    }

    [Fact]
    public void Alterar_DeveFalhar_QuandoPedidoJaEstiverProcessado()
    {
        var pedido = CriarPedido();
        pedido.Processar();

        var excecao = Assert.Throws<DomainException>(
            () => pedido.Alterar(CriarComprador(), [CriarProduto()]));

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
        return new Pedido(CriarComprador(), [CriarProduto()]);
    }

    private static Comprador CriarComprador()
    {
        return new Comprador("Maíra");
    }

    private static Produto CriarProduto()
    {
        return new Produto("Teclado", 199.90m);
    }
}
