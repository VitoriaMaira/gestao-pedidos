using FluentValidation;

namespace LojaPedidos.Application.Produtos.Criar;

public sealed record CriarProdutoRequest(
    string Nome,
    decimal Preco,
    string? ImagemUrl = null);

public sealed class CriarProdutoRequestValidator : AbstractValidator<CriarProdutoRequest>
{
    public CriarProdutoRequestValidator()
    {
        RuleFor(produto => produto.Nome)
            .NotEmpty()
            .WithMessage("O nome do produto é obrigatório.")
            .MaximumLength(150)
            .WithMessage("O nome do produto deve possuir no máximo 150 caracteres.");

        RuleFor(produto => produto.Preco)
            .GreaterThan(0)
            .WithMessage("O preço do produto deve ser maior que zero.");

        RuleFor(produto => produto.ImagemUrl)
            .MaximumLength(2048)
            .WithMessage("A URL da imagem deve possuir no máximo 2048 caracteres.")
            .Must(url => string.IsNullOrWhiteSpace(url)
                || Uri.TryCreate(url, UriKind.Absolute, out var uri)
                && (uri.Scheme == Uri.UriSchemeHttp
                    || uri.Scheme == Uri.UriSchemeHttps))
            .WithMessage("A URL da imagem deve ser um endereço HTTP ou HTTPS válido.");
    }
}
