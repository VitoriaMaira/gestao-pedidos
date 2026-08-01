using FluentValidation;

namespace LojaPedidos.Application.Pedidos.AlterarPedido;

public sealed record AlterarPedidoRequest(
    Guid CompradorId,
    IReadOnlyCollection<AlterarItemPedidoRequest>? Itens);

public sealed class AlterarPedidoRequestValidator : AbstractValidator<AlterarPedidoRequest>
{
    public AlterarPedidoRequestValidator()
    {
        RuleFor(request => request.CompradorId)
            .NotEmpty()
            .WithMessage("O comprador é obrigatório.");

        RuleFor(request => request.Itens)
            .NotEmpty()
            .WithMessage("O pedido deve possuir pelo menos um item.");

        RuleForEach(request => request.Itens)
            .SetValidator(new AlterarItemPedidoRequestValidator());
    }
}
