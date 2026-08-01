namespace LojaPedidos.Web.Contracts.Pedidos;

public sealed record AtualizarStatusPedidoRequest(StatusPedido Status);

public sealed record AtualizarStatusPedidoResponse(
    string Mensagem,
    PedidoResponse Pedido);
