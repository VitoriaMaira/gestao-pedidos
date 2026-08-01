using LojaPedidos.Domain.Entities;
using LojaPedidos.Domain.Enums;
using LojaPedidos.Domain.Exceptions;

namespace LojaPedidos.UnitTests.Domain;

public sealed class PedidoTransicaoStatusTests
{
    [Fact]
    public void AlterarStatus_DevePermitirProcessarPedidoIniciado()
    {
        var pedido = CriarPedido();

        pedido.AlterarStatus(StatusPedido.Processado);

        Assert.Equal(StatusPedido.Processado, pedido.Status);
    }

    [Fact]
    public void AlterarStatus_DevePermitirEnviarPedidoProcessado()
    {
        var pedido = CriarPedido();
        pedido.Processar();

        pedido.AlterarStatus(StatusPedido.Enviado);

        Assert.Equal(StatusPedido.Enviado, pedido.Status);
    }

    [Theory]
    [InlineData(StatusPedido.Iniciado)]
    [InlineData(StatusPedido.Processado)]
    public void AlterarStatus_DevePermitirCancelarPedido(
        StatusPedido statusInicial)
    {
        var pedido = CriarPedido();

        if (statusInicial == StatusPedido.Processado)
        {
            pedido.Processar();
        }

        pedido.AlterarStatus(StatusPedido.Cancelado);

        Assert.Equal(StatusPedido.Cancelado, pedido.Status);
    }

    [Fact]
    public void AlterarStatus_DeveRejeitarEnvioDePedidoIniciado()
    {
        var pedido = CriarPedido();

        Assert.Throws<DomainException>(
            () => pedido.AlterarStatus(StatusPedido.Enviado));
    }

    [Fact]
    public void AlterarStatus_DeveRejeitarCancelamentoDePedidoEnviado()
    {
        var pedido = CriarPedido();
        pedido.Processar();
        pedido.Enviar();

        Assert.Throws<DomainException>(
            () => pedido.AlterarStatus(StatusPedido.Cancelado));
    }

    private static Pedido CriarPedido()
    {
        var comprador = new Comprador("Comprador", "12345678909");
        var produto = new Produto("Produto", 100m);

        return new Pedido(comprador, [new ItemPedido(produto, 1)]);
    }
}
