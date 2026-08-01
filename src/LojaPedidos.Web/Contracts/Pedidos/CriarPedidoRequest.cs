namespace LojaPedidos.Web.Contracts.Pedidos;

public sealed record CriarPedidoRequest(
    CriarCompradorRequest Comprador,
    IReadOnlyCollection<CriarItemPedidoRequest> Itens);

public sealed record CriarCompradorRequest(string Nome, string Cpf);

public sealed record CriarItemPedidoRequest(
    CriarProdutoRequest Produto,
    int Quantidade);

public sealed record CriarProdutoRequest(string Nome, decimal Preco);
