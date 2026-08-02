using LojaPedidos.Domain.Common;

namespace LojaPedidos.Domain.Entities;

public class Produto : Entity
{
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }
}
