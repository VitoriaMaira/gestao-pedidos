namespace LojaPedidos.Application.Produtos.Listar;

public sealed record ListarProdutosResponse(
    IReadOnlyCollection<ProdutoResponse> Itens,
    int Pagina,
    int TamanhoPagina,
    int Total);

public sealed record ProdutoResponse(Guid Id, string Nome, decimal Preco);
