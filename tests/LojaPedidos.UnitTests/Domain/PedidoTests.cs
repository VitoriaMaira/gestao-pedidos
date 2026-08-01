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
    public void Criar_DeveFalhar_QuandoProdutoForRepetidoNosItens()
    {
        var produto = CriarProduto();
        var itens = new[]
        {
            new ItemPedido(produto, 1),
            new ItemPedido(produto, 2)
        };

        var excecao = Assert.Throws<DomainException>(
            () => new Pedido(CriarComprador(), itens));

        Assert.Equal(
            "O mesmo produto não pode ser adicionado mais de uma vez.",
            excecao.Message);
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

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void AlterarQuantidadeItem_DeveFalhar_QuandoQuantidadeNaoForPositiva(
        int quantidade)
    {
        var pedido = CriarPedido();
        var item = Assert.Single(pedido.Itens);

        var excecao = Assert.Throws<DomainException>(
            () => pedido.AlterarQuantidadeItem(item.Id, quantidade));

        Assert.Equal("A quantidade deve ser maior que zero.", excecao.Message);
    }

    [Fact]
    public void Processar_DeveFalhar_QuandoPedidoEstiverCancelado()
    {
        var pedido = CriarPedido();
        pedido.Cancelar();

        var excecao = Assert.Throws<DomainException>(() => pedido.Processar());

        Assert.Equal("Apenas pedidos iniciados podem ser processados.", excecao.Message);
    }

    [Theory]
    [InlineData(StatusPedido.Cancelado)]
    [InlineData(StatusPedido.Enviado)]
    public void AlterarQuantidadeItem_DeveFalhar_QuandoPedidoEstiverFinalizado(
        StatusPedido status)
    {
        var pedido = CriarPedido();
        var item = Assert.Single(pedido.Itens);

        if (status == StatusPedido.Enviado)
        {
            pedido.Processar();
            pedido.Enviar();
        }
        else
        {
            pedido.Cancelar();
        }

        Assert.Throws<DomainException>(
            () => pedido.AlterarQuantidadeItem(item.Id, 3));
    }

    [Fact]
    public void AlterarStatus_NaoDeveAtualizarData_QuandoStatusJaEstiverDefinido()
    {
        var pedido = CriarPedido();
        pedido.AlterarStatus(StatusPedido.Processado);
        var atualizadoEm = pedido.AtualizadoEm;

        pedido.AlterarStatus(StatusPedido.Processado);

        Assert.Equal(atualizadoEm, pedido.AtualizadoEm);
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

    [Fact]
    public void AlterarStatus_DeveProcessarEEnviar_QuandoTransicoesForemValidas()
    {
        var pedido = CriarPedido();

        pedido.AlterarStatus(StatusPedido.Processado);
        pedido.AlterarStatus(StatusPedido.Enviado);

        Assert.Equal(StatusPedido.Enviado, pedido.Status);
    }

    [Fact]
    public void AlterarStatus_DeveCancelar_QuandoPedidoEstiverIniciado()
    {
        var pedido = CriarPedido();

        pedido.AlterarStatus(StatusPedido.Cancelado);

        Assert.Equal(StatusPedido.Cancelado, pedido.Status);
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
