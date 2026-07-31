using LojaPedidos.Domain.Common;
using LojaPedidos.Domain.Exceptions;

namespace LojaPedidos.Domain.Entities;

public sealed class Comprador : Entity
{
    private Comprador()
    {
        Nome = string.Empty;
    }

    public Comprador(string nome)
    {
        DefinirNome(nome);
    }

    public string Nome { get; private set; } = string.Empty;

    public void AlterarNome(string nome)
    {
        DefinirNome(nome);
    }

    private void DefinirNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
        {
            throw new DomainException("O nome do comprador é obrigatório.");
        }

        Nome = nome.Trim();
    }
}
