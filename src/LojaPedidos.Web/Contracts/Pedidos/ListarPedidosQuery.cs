namespace LojaPedidos.Web.Contracts.Pedidos;

public sealed record ListarPedidosQuery(
    int Pagina = 1,
    int TamanhoPagina = 10,
    StatusPedido? Status = null,
    string? Cpf = null);
