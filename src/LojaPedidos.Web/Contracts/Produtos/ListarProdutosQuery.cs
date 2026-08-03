namespace LojaPedidos.Web.Contracts.Produtos;

public sealed record ListarProdutosQuery(
    int Pagina = 1,
    int TamanhoPagina = 100);
