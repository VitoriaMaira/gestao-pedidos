namespace LojaPedidos.Web.Contracts.Pedidos;

public sealed record CriarPedidoResponse(
    string Mensagem,
    Guid Id,
    CriarPedidoCompradorResponse Comprador,
    StatusPedido Status,
    decimal Total,
    DateTimeOffset CriadoEm,
    IReadOnlyCollection<CriarPedidoItemResponse> Itens);

public sealed record CriarPedidoCompradorResponse(Guid Id, string Nome, string Cpf);

public sealed record CriarPedidoProdutoResponse(Guid Id, string Nome, decimal Preco);

public sealed record CriarPedidoItemResponse(
    Guid Id,
    CriarPedidoProdutoResponse Produto,
    int Quantidade,
    decimal PrecoUnitario,
    decimal Subtotal);
