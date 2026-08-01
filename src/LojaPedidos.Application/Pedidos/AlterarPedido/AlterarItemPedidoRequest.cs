using FluentValidation;

namespace LojaPedidos.Application.Pedidos.AlterarPedido;

public sealed record AlterarItemPedidoRequest(
    Guid ProdutoId,
    int Quantidade);

public sealed class AlterarItemPedidoRequestValidator : AbstractValidator<AlterarItemPedidoRequest>
{
    public AlterarItemPedidoRequestValidator()
    {
        RuleFor(item => item.ProdutoId)
            .NotEmpty()
            .WithMessage("O produto é obrigatório.");

        RuleFor(item => item.Quantidade)
            .GreaterThan(0)
            .WithMessage("A quantidade deve ser maior que zero.");
    }
}
