using LojaPedidos.Application.Pedidos.ConsultarPedido;

namespace LojaPedidos.Application.Pedidos.ListarPedidos;

public sealed record ListarPedidosResponse(
    IReadOnlyCollection<PedidoResponse> Itens,
    int Pagina,
    int TamanhoPagina,
    int Total);
