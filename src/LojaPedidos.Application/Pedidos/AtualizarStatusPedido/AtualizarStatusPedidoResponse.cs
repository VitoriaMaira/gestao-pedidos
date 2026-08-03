using LojaPedidos.Application.Pedidos.ConsultarPedido;

namespace LojaPedidos.Application.Pedidos.AtualizarStatusPedido;

public sealed record AtualizarStatusPedidoResponse(
    string Mensagem,
    ConsultarPedidoResponse Pedido);
