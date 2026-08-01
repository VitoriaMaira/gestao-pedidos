using LojaPedidos.Domain.Common;
using LojaPedidos.Domain.Enums;
using LojaPedidos.Domain.Exceptions;

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
        Comprador = ValidarComprador(comprador);
        AdicionarItens(itens);
        Status = StatusPedido.Iniciado;
        CriadoEm = DateTimeOffset.UtcNow;
    }

    public Guid CompradorId { get; private set; }

    public Comprador Comprador { get; private set; }

    public IReadOnlyCollection<ItemPedido> Itens => _itens.AsReadOnly();

    public StatusPedido Status { get; private set; }

    public DateTimeOffset CriadoEm { get; private set; }

    public DateTimeOffset? AtualizadoEm { get; private set; }

    public decimal Total => _itens.Sum(item => item.Subtotal);

    public void AlterarQuantidadeItem(Guid itemId, int quantidade)
    {
        if (Status != StatusPedido.Iniciado)
        {
            throw new DomainException("Apenas pedidos não processados podem ser alterados.");
        }

        var item = _itens.SingleOrDefault(item => item.Id == itemId);

        if (item is null)
        {
            throw new DomainException("O item informado não pertence ao pedido.");
        }

        item.AlterarQuantidade(quantidade);
        RegistrarAtualizacao();
    }

    public void Processar()
    {
        if (Status != StatusPedido.Iniciado)
        {
            throw new DomainException("Apenas pedidos iniciados podem ser processados.");
        }

        Status = StatusPedido.Processado;
        RegistrarAtualizacao();
    }

    public void Cancelar()
    {
        if (Status is not (StatusPedido.Iniciado or StatusPedido.Processado))
        {
            throw new DomainException("Apenas pedidos iniciados ou processados podem ser cancelados.");
        }

        Status = StatusPedido.Cancelado;
        RegistrarAtualizacao();
    }

    public void Enviar()
    {
        if (Status != StatusPedido.Processado)
        {
            throw new DomainException("Apenas pedidos processados podem ser enviados.");
        }

        Status = StatusPedido.Enviado;
        RegistrarAtualizacao();
    }

    private Comprador ValidarComprador(Comprador? comprador)
    {
        if (comprador is null)
        {
            throw new DomainException("O comprador é obrigatório.");
        }

        CompradorId = comprador.Id;
        return comprador;
    }

    private void AdicionarItens(IEnumerable<ItemPedido>? itens)
    {
        if (itens is null)
        {
            throw new DomainException("O pedido deve possuir pelo menos um item.");
        }

        var itensInformados = itens.ToList();

        if (itensInformados.Count == 0)
        {
            throw new DomainException("O pedido deve possuir pelo menos um item.");
        }

        if (itensInformados.Any(item => item is null))
        {
            throw new DomainException("O pedido não pode possuir um item inválido.");
        }

        if (itensInformados.GroupBy(item => item.ProdutoId).Any(grupo => grupo.Count() > 1))
        {
            throw new DomainException("O mesmo produto não pode ser adicionado mais de uma vez.");
        }

        foreach (var item in itensInformados)
        {
            item.VincularAoPedido(Id);
        }

        _itens.AddRange(itensInformados);
    }

    private void RegistrarAtualizacao()
    {
        AtualizadoEm = DateTimeOffset.UtcNow;
    }
}
