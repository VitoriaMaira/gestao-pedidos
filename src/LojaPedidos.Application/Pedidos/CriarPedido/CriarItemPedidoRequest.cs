using FluentValidation;

namespace LojaPedidos.Application.Pedidos.CriarPedido;

public sealed record CriarItemPedidoRequest(
    CriarProdutoRequest? Produto,
    int Quantidade);

public sealed class CriarItemPedidoRequestValidator : AbstractValidator<CriarItemPedidoRequest>
{
    public CriarItemPedidoRequestValidator()
    {
        RuleFor(item => item.Produto)
            .NotNull()
            .WithMessage("O produto é obrigatório.");

        When(item => item.Produto is not null, () =>
        {
            RuleFor(item => item.Produto!)
                .SetValidator(new CriarProdutoRequestValidator());
        });

        RuleFor(item => item.Quantidade)
            .GreaterThan(0)
            .WithMessage("A quantidade deve ser maior que zero.");
    }
}
