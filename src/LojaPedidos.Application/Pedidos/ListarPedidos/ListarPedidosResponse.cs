using LojaPedidos.Domain.Entities;
using LojaPedidos.Domain.Enums;

namespace LojaPedidos.Application.Pedidos.ListarPedidos;

public sealed record ListarPedidosResponse(
    IReadOnlyCollection<ListarPedidosResponse_Pedido> Itens,
    int Pagina,
    int TamanhoPagina,
    int Total)
{
    public static ListarPedidosResponse Map(
        IEnumerable<Pedido> pedidos,
        int pagina,
        int tamanhoPagina,
        int total)
    {
        var itens = pedidos
            .Select(ListarPedidosResponse_Pedido.Map)
            .ToArray();

        return new ListarPedidosResponse(
            itens,
            pagina,
            tamanhoPagina,
            total);
    }
}

public sealed record ListarPedidosResponse_Pedido(
    Guid Id,
    Guid CompradorId,
    string Comprador,
    StatusPedido Status,
    decimal Total,
    DateTimeOffset CriadoEm,
    IReadOnlyCollection<ListarPedidosResponse_ItemPedido> Itens)
{
    public static ListarPedidosResponse_Pedido Map(Pedido pedido)
    {
        var itens = pedido.Itens
            .Select(ListarPedidosResponse_ItemPedido.Map)
            .ToArray();

        return new ListarPedidosResponse_Pedido(
            pedido.Id,
            pedido.CompradorId,
            pedido.Comprador.Nome,
            pedido.Status,
            pedido.Total,
            pedido.CriadoEm,
            itens);
    }
}

public sealed record ListarPedidosResponse_ItemPedido(
    Guid Id,
    Guid ProdutoId,
    string Produto,
    string? ImagemUrl,
    int Quantidade,
    decimal PrecoUnitario,
    decimal Subtotal)
{
    public static ListarPedidosResponse_ItemPedido Map(ItemPedido item) =>
        new(
            item.Id,
            item.ProdutoId,
            item.Produto.Nome,
            item.Produto.ImagemUrl,
            item.Quantidade,
            item.PrecoUnitario,
            item.Subtotal);
}
