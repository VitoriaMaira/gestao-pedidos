using LojaPedidos.Domain.Enums;

namespace LojaPedidos.Application.Pedidos.CriarPedido;

public sealed record CriarPedidoResponse(Guid Id, StatusPedido Status, decimal Total, DateTimeOffset CriadoEm);
