using LojaPedidos.Domain.Common;

namespace LojaPedidos.Domain.Entities;

public sealed class Comprador : Entity
{
    public string Nome { get; set; } = string.Empty;

    public string Cpf { get; set; } = string.Empty;

    public Comprador()
    {

    }

    public Comprador(string nome, string cpf)
    {
        Nome = nome;
        Cpf = cpf;
    }
}
