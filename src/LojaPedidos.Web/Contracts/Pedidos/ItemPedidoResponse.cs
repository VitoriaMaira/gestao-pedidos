namespace LojaPedidos.Web.Contracts.Pedidos;

public sealed record ItemPedidoResponse(
    Guid Id,
    Guid ProdutoId,
    string Produto,
    int Quantidade,
    decimal PrecoUnitario,
    decimal Subtotal);
