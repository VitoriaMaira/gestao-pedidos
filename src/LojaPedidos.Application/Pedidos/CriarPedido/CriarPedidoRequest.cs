using FluentValidation;

namespace LojaPedidos.Application.Pedidos.CriarPedido;

public sealed record CriarPedidoRequest(
    CriarCompradorRequest? Comprador,
    IReadOnlyCollection<CriarItemPedidoRequest>? Itens);

public sealed class CriarPedidoRequestValidator
    : AbstractValidator<CriarPedidoRequest>
{
    public CriarPedidoRequestValidator()
    {
        RuleFor(request => request.Comprador)
            .NotNull()
            .WithMessage("O comprador é obrigatório.");

        When(request => request.Comprador is not null, () =>
        {
            RuleFor(request => request.Comprador!)
                .SetValidator(new CriarCompradorRequestValidator());
        });

        RuleFor(request => request.Itens)
            .NotEmpty()
            .WithMessage("O pedido deve possuir pelo menos um item.");

        RuleForEach(request => request.Itens)
            .SetValidator(new CriarItemPedidoRequestValidator());
    }
}
