using LojaPedidos.Application.Pedidos.ConsultarPedido;

namespace LojaPedidos.Application.Pedidos.CancelarPedido;

public sealed record CancelarPedidoResponse(
    string Mensagem,
    PedidoResponse Pedido);
