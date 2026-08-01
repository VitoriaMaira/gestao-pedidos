namespace LojaPedidos.Web.Contracts.Pedidos;

public sealed record ListarPedidosResponse(
    IReadOnlyCollection<PedidoResponse> Itens,
    int Pagina,
    int TamanhoPagina,
    int Total);
