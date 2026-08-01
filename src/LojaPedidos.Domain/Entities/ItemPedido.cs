using LojaPedidos.Domain.Common;
using LojaPedidos.Domain.Exceptions;

namespace LojaPedidos.Domain.Entities;

public sealed class ItemPedido : Entity
{
    private ItemPedido()
    {
        Produto = null!;
    }

    public ItemPedido(Produto produto, int quantidade)
    {
        if (produto is null)
        {
            throw new DomainException("O produto é obrigatório.");
        }

        if (quantidade <= 0)
        {
            throw new DomainException("A quantidade deve ser maior que zero.");
        }

        Produto = produto;
        ProdutoId = produto.Id;
        Quantidade = quantidade;
        PrecoUnitario = produto.Preco;
    }

    public Guid PedidoId { get; private set; }

    public Guid ProdutoId { get; private set; }

    public Produto Produto { get; private set; }

    public int Quantidade { get; private set; }

    public decimal PrecoUnitario { get; private set; }

    public decimal Subtotal => PrecoUnitario * Quantidade;

    internal void AlterarQuantidade(int quantidade)
    {
        if (quantidade <= 0)
        {
            throw new DomainException("A quantidade deve ser maior que zero.");
        }

        Quantidade = quantidade;
    }

    internal void VincularAoPedido(Guid pedidoId)
    {
        if (pedidoId == Guid.Empty)
        {
            throw new DomainException("O pedido do item é inválido.");
        }

        if (PedidoId != Guid.Empty && PedidoId != pedidoId)
        {
            throw new DomainException("O item já pertence a outro pedido.");
        }

        PedidoId = pedidoId;
    }
}
