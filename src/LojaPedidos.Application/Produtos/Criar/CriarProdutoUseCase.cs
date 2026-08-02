using FluentValidation;
using LojaPedidos.Application.Common.Exceptions;
using LojaPedidos.Domain.Entities;
using LojaPedidos.Domain.Repositories;

namespace LojaPedidos.Application.Produtos.Criar;

public interface ICriarProdutoUseCase
{
    Task<CriarProdutoResponse> Execute(CriarProdutoRequest request, CancellationToken cancellationToken = default);
}

public class CriarProdutoUseCase(
    IValidator<CriarProdutoRequest> validator,
    IProdutoRepository produtoRepository,
    IUnitOfWork unitOfWork) : ICriarProdutoUseCase
{
    public async Task<CriarProdutoResponse> Execute(CriarProdutoRequest request, CancellationToken cancellationToken = default)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var produtoExiste = await produtoRepository.ObterPorNomeAsync(request.Nome);

        if (produtoExiste is not null)
            throw new ErrorOnValidationException(["O produto já existe."]);

        var produto = new Produto
        {
            Nome = request.Nome,
            Preco = request.Preco
        };

        await produtoRepository.AdicionarAsync(produto, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return new CriarProdutoResponse(produto.Id);
    }
}
