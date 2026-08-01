using FluentValidation;

namespace LojaPedidos.Application.Pedidos.CriarPedido;

public sealed record CriarPedidoRequest(
    Guid CompradorId,
    IReadOnlyCollection<CriarItemPedidoRequest>? Itens);

public sealed record CriarItemPedidoRequest(
    Guid ProdutoId,
    int Quantidade);

public sealed class CriarPedidoRequestValidator
    : AbstractValidator<CriarPedidoRequest>
{
    public CriarPedidoRequestValidator()
    {
        RuleFor(request => request.CompradorId)
            .NotEmpty()
            .WithMessage("O comprador é obrigatório.");

        RuleFor(request => request.Itens)
            .NotEmpty()
            .WithMessage("O pedido deve possuir pelo menos um item.");

        RuleForEach(request => request.Itens)
            .SetValidator(new CriarItemPedidoRequestValidator());
    }
}

public sealed class CriarItemPedidoRequestValidator
    : AbstractValidator<CriarItemPedidoRequest>
{
    public CriarItemPedidoRequestValidator()
    {
        RuleFor(item => item.ProdutoId)
            .NotEmpty()
            .WithMessage("O produto é obrigatório.");

        RuleFor(item => item.Quantidade)
            .GreaterThan(0)
            .WithMessage("A quantidade deve ser maior que zero.");
    }
}
