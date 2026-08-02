using FluentValidation;

namespace LojaPedidos.Application.Produtos.Listar;

public sealed record ListarProdutosQuery(
    int Pagina = 1,
    int TamanhoPagina = 10);

public sealed class ListarProdutosQueryValidator
    : AbstractValidator<ListarProdutosQuery>
{
    public ListarProdutosQueryValidator()
    {
        RuleFor(request => request.Pagina)
            .GreaterThan(0)
            .WithMessage("A página deve ser maior que zero.");

        RuleFor(request => request.TamanhoPagina)
            .InclusiveBetween(1, 100)
            .WithMessage("O tamanho da página deve estar entre 1 e 100.");
    }
}
