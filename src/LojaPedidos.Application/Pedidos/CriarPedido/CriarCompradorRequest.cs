using FluentValidation;
using LojaPedidos.Domain.ValueObjects;

namespace LojaPedidos.Application.Pedidos.CriarPedido;

public sealed record CriarCompradorRequest(
    string? Nome,
    string? Cpf);

public sealed class CriarCompradorRequestValidator : AbstractValidator<CriarCompradorRequest>
{
    public CriarCompradorRequestValidator()
    {
        RuleFor(comprador => comprador.Nome)
            .NotEmpty()
            .WithMessage("O nome do comprador é obrigatório.")
            .MaximumLength(150)
            .WithMessage("O nome do comprador deve possuir no máximo 150 caracteres.");

        RuleFor(comprador => comprador.Cpf)
            .Must(Cpf.EhValido)
            .WithMessage("O CPF do comprador é inválido.");
    }
}
