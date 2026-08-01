namespace LojaPedidos.Web.Contracts.Pedidos;

public sealed record AlterarPedidoRequest(
    IReadOnlyCollection<AlterarItemPedidoRequest> Itens);

public sealed record AlterarItemPedidoRequest(Guid ItemId, int Quantidade);
