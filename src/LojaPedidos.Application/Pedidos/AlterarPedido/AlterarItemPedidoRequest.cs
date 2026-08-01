using FluentValidation;

namespace LojaPedidos.Application.Pedidos.AlterarPedido;

public sealed record AlterarItemPedidoRequest(
    Guid ItemId,
    int Quantidade);

public sealed class AlterarItemPedidoRequestValidator : AbstractValidator<AlterarItemPedidoRequest>
{
    public AlterarItemPedidoRequestValidator()
    {
        RuleFor(item => item.ItemId)
            .NotEmpty()
            .WithMessage("O item é obrigatório.");

        RuleFor(item => item.Quantidade)
            .GreaterThan(0)
            .WithMessage("A quantidade deve ser maior que zero.");
    }
}
