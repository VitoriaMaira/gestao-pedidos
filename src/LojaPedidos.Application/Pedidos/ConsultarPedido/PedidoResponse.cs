using LojaPedidos.Domain.Entities;
using LojaPedidos.Domain.Enums;

namespace LojaPedidos.Application.Pedidos.ConsultarPedido;

public sealed record PedidoResponse(
    Guid Id,
    Guid CompradorId,
    string Comprador,
    StatusPedido Status,
    decimal Total,
    DateTimeOffset CriadoEm,
    IReadOnlyCollection<ItemPedidoResponse> Itens)
{
    public static PedidoResponse Criar(Pedido pedido)
    {
        var itens = pedido.Itens
            .Select(item => new ItemPedidoResponse(
                item.ProdutoId,
                item.Produto.Nome,
                item.Quantidade,
                item.PrecoUnitario,
                item.Subtotal))
            .ToArray();

        return new PedidoResponse(
            pedido.Id,
            pedido.CompradorId,
            pedido.Comprador.Nome,
            pedido.Status,
            pedido.Total,
            pedido.CriadoEm,
            itens);
    }
}

public sealed record ItemPedidoResponse(
    Guid ProdutoId,
    string Produto,
    int Quantidade,
    decimal PrecoUnitario,
    decimal Subtotal);
