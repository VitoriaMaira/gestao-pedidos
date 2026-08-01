using FluentValidation;

namespace LojaPedidos.Application.Pedidos.AlterarPedido;

public sealed record AlterarPedidoRequest(
    IReadOnlyCollection<AlterarItemPedidoRequest>? Itens);

public sealed class AlterarPedidoRequestValidator : AbstractValidator<AlterarPedidoRequest>
{
    public AlterarPedidoRequestValidator()
    {
        RuleFor(request => request.Itens)
            .NotEmpty()
            .WithMessage("O pedido deve possuir pelo menos um item.");

        RuleFor(request => request.Itens)
            .Must(itens => itens is null
                || itens.Select(item => item.ItemId).Distinct().Count() == itens.Count)
            .WithMessage("O mesmo item não pode ser informado mais de uma vez.");

        RuleForEach(request => request.Itens)
            .SetValidator(new AlterarItemPedidoRequestValidator());
    }
}
