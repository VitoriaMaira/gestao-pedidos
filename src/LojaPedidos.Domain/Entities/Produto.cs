using LojaPedidos.Domain.Common;
using LojaPedidos.Domain.Exceptions;

namespace LojaPedidos.Domain.Entities;

public sealed class Produto : Entity
{
    private Produto()
    {
        Nome = string.Empty;
    }

    public Produto(string nome, decimal preco)
    {
        DefinirNome(nome);
        DefinirPreco(preco);
    }

    public string Nome { get; private set; } = string.Empty;

    public decimal Preco { get; private set; }

    public void Alterar(string nome, decimal preco)
    {
        DefinirNome(nome);
        DefinirPreco(preco);
    }

    private void DefinirNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
        {
            throw new DomainException("O nome do produto é obrigatório.");
        }

        Nome = nome.Trim();
    }

    private void DefinirPreco(decimal preco)
    {
        if (preco <= 0)
        {
            throw new DomainException("O preço do produto deve ser maior que zero.");
        }

        Preco = preco;
    }
}
