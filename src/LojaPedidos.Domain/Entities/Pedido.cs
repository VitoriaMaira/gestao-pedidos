using LojaPedidos.Domain.Common;
using LojaPedidos.Domain.Enums;

namespace LojaPedidos.Domain.Entities;

public sealed class Pedido : Entity
{
    private readonly List<ItemPedido> _itens = [];

    private Pedido()
    {
        Comprador = null!;
    }

    public Pedido(Comprador comprador, IEnumerable<ItemPedido> itens)
    {
        Comprador = comprador;
        CompradorId = comprador.Id;
        _itens.AddRange(itens);
        Status = StatusPedido.Iniciado;
        CriadoEm = DateTimeOffset.UtcNow;

        foreach (var item in _itens)
        {
            item.VincularAoPedido(Id);
        }
    }

    public Guid CompradorId { get; private set; }

    public Comprador Comprador { get; private set; }

    public IReadOnlyCollection<ItemPedido> Itens => _itens.AsReadOnly();

    public StatusPedido Status { get; private set; }

    public DateTimeOffset CriadoEm { get; private set; }

    public DateTimeOffset? AtualizadoEm { get; private set; }

    public decimal Total => _itens.Sum(item => item.Subtotal);

    public void DefinirStatus(StatusPedido status)
    {
        Status = status;
        RegistrarAtualizacao();
    }

    public void RegistrarAtualizacao()
    {
        AtualizadoEm = DateTimeOffset.UtcNow;
    }
}
