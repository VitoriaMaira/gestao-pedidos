using FluentValidation;

namespace LojaPedidos.Application.Pedidos.CriarPedido;

public sealed record CriarPedidoRequest(string NomeComprador, string CpfComprador, List<CriarPedidoRequest_ItemPedidoAux> Itens);

public sealed record CriarPedidoRequest_ItemPedidoAux(Guid Id, int Quantidade);

public sealed class CriarPedidoRequestValidator
    : AbstractValidator<CriarPedidoRequest>
{
    public CriarPedidoRequestValidator()
    {
        RuleFor(request => request.NomeComprador)
            .NotEmpty()
            .WithMessage("O nome do comprador é obrigatório.");

        RuleFor(request => request.CpfComprador)
            .NotEmpty()
            .WithMessage("O CPF do comprador é obrigatório.");

        RuleFor(request => request.Itens)
            .NotEmpty()
            .WithMessage("O pedido deve possuir pelo menos um item.");

        RuleFor(request => request.Itens)
            .Must(itens => itens
                .Select(item => item.Id)
                .Distinct()
                .Count() == itens.Count)
            .When(request => request.Itens is { Count: > 0 })
            .WithMessage("O mesmo produto não pode ser adicionado mais de uma vez.");

        RuleForEach(request => request.Itens)
            .ChildRules(item =>
            {
                item.RuleFor(itemPedido => itemPedido.Id)
                    .NotEmpty()
                    .WithMessage("O identificador do produto é obrigatório.");

                item.RuleFor(itemPedido => itemPedido.Quantidade)
                    .GreaterThan(0)
                    .WithMessage("A quantidade do item deve ser maior que zero.");
            });
    }
}
