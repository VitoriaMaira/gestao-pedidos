namespace LojaPedidos.Web.Contracts.Pedidos;

public sealed record CriarPedidoRequest(
    string NomeComprador,
    string CpfComprador,
    IReadOnlyCollection<CriarItemPedidoRequest> Itens);

public sealed record CriarItemPedidoRequest(Guid Id, int Quantidade);
