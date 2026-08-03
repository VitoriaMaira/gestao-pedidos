using LojaPedidos.Domain.Entities;
using LojaPedidos.Domain.Enums;

namespace LojaPedidos.Application.Pedidos.ConsultarPedido;

public sealed record ConsultarPedidoResponse(
    Guid Id,
    Guid CompradorId,
    string Comprador,
    StatusPedido Status,
    decimal Total,
    DateTimeOffset CriadoEm,
    IReadOnlyCollection<ConsultarPedidoResponse_ItemPedido> Itens)
{
    public static ConsultarPedidoResponse Map(Pedido pedido)
    {
        var itens = pedido.Itens
            .Select(item => new ConsultarPedidoResponse_ItemPedido(
                item.Id,
                item.ProdutoId,
                item.Produto.Nome,
                item.Quantidade,
                item.PrecoUnitario,
                item.Subtotal))
            .ToArray();

        return new ConsultarPedidoResponse(
            pedido.Id,
            pedido.CompradorId,
            pedido.Comprador.Nome,
            pedido.Status,
            pedido.Total,
            pedido.CriadoEm,
            itens);
    }
}

public sealed record ConsultarPedidoResponse_ItemPedido(
    Guid Id,
    Guid ProdutoId,
    string Produto,
    int Quantidade,
    decimal PrecoUnitario,
    decimal Subtotal);
