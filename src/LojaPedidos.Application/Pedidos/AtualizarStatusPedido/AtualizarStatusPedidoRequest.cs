using FluentValidation;
using LojaPedidos.Domain.Enums;

namespace LojaPedidos.Application.Pedidos.AtualizarStatusPedido;

public sealed record AtualizarStatusPedidoRequest(StatusPedido Status);

public sealed class AtualizarStatusPedidoRequestValidator
    : AbstractValidator<AtualizarStatusPedidoRequest>
{
    public AtualizarStatusPedidoRequestValidator()
    {
        RuleFor(request => request.Status)
            .IsInEnum()
            .WithMessage("O status informado é inválido.");

        RuleFor(request => request.Status)
            .NotEqual(StatusPedido.Iniciado)
            .WithMessage("O status Iniciado é definido apenas na criação do pedido.");
    }
}
