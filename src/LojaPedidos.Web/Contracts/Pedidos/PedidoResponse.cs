namespace LojaPedidos.Web.Contracts.Pedidos;

public sealed record PedidoResponse(
    Guid Id,
    Guid CompradorId,
    string Comprador,
    StatusPedido Status,
    decimal Total,
    DateTimeOffset CriadoEm,
    IReadOnlyCollection<ItemPedidoResponse> Itens);
