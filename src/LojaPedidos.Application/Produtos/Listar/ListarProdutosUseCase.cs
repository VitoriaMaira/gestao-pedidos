using FluentValidation;
using LojaPedidos.Domain.Repositories;

namespace LojaPedidos.Application.Produtos.Listar;

public interface IListarProdutosUseCase
{
    Task<ListarProdutosResponse> ExecutarAsync(
        ListarProdutosQuery request,
        CancellationToken cancellationToken = default);
}

public sealed class ListarProdutosUseCase(
    IProdutoRepository produtoRepository,
    IValidator<ListarProdutosQuery> validator) : IListarProdutosUseCase
{
    public async Task<ListarProdutosResponse> ExecutarAsync(
        ListarProdutosQuery request,
        CancellationToken cancellationToken = default)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var (produtos, total) = await produtoRepository.ListarAsync(
            request.Pagina,
            request.TamanhoPagina,
            cancellationToken);

        var itens = produtos
            .Select(produto => new ProdutoResponse(
                produto.Id,
                produto.Nome,
                produto.Preco))
            .ToArray();

        return new ListarProdutosResponse(
            itens,
            request.Pagina,
            request.TamanhoPagina,
            total);
    }
}
