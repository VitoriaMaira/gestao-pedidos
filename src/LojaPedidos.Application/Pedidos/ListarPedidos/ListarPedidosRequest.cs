using FluentValidation;
using LojaPedidos.Domain.Enums;

namespace LojaPedidos.Application.Pedidos.ListarPedidos;

public sealed record ListarPedidosRequest(
    int Pagina = 1,
    int TamanhoPagina = 10,
    StatusPedido? Status = null,
    Guid? CompradorId = null);

public sealed class ListarPedidosRequestValidator : AbstractValidator<ListarPedidosRequest>
{
    public ListarPedidosRequestValidator()
    {
        RuleFor(request => request.Pagina)
            .GreaterThan(0)
            .WithMessage("A página deve ser maior que zero.");

        RuleFor(request => request.TamanhoPagina)
            .InclusiveBetween(1, 100)
            .WithMessage("O tamanho da página deve estar entre 1 e 100.");
    }
}
