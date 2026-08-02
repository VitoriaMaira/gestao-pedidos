using FluentValidation;

namespace LojaPedidos.Application.Produtos.Criar;

public sealed record CriarProdutoRequest(string Nome, decimal Preco);

public sealed class CriarProdutoRequestValidator : AbstractValidator<CriarProdutoRequest>
{
    public CriarProdutoRequestValidator()
    {
        RuleFor(produto => produto.Nome)
            .NotEmpty()
            .WithMessage("O nome do produto é obrigatório.")
            .MaximumLength(150)
            .WithMessage("O nome do produto deve possuir no máximo 150 caracteres.");

        RuleFor(produto => produto.Preco)
            .GreaterThan(0)
            .WithMessage("O preço do produto deve ser maior que zero.");
    }
}