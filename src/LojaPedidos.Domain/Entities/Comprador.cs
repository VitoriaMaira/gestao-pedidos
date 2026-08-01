using LojaPedidos.Domain.Common;
using LojaPedidos.Domain.Exceptions;

namespace LojaPedidos.Domain.Entities;

public sealed class Comprador : Entity
{
    private Comprador()
    {
        Nome = string.Empty;
        Cpf = string.Empty;
    }

    public Comprador(string nome, string cpf)
    {
        DefinirNome(nome);
        DefinirCpf(cpf);
    }

    public string Nome { get; private set; } = string.Empty;

    public string Cpf { get; private set; } = string.Empty;

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

    private void DefinirCpf(string cpf)
    {
        if (!LojaPedidos.Domain.ValueObjects.Cpf.EhValido(cpf))
        {
            throw new DomainException("O CPF do comprador é inválido.");
        }

        Cpf = LojaPedidos.Domain.ValueObjects.Cpf.Normalizar(cpf);
    }
}
