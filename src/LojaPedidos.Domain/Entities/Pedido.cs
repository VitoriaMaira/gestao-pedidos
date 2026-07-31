using LojaPedidos.Domain.Common;
using LojaPedidos.Domain.Enums;
using LojaPedidos.Domain.Exceptions;

namespace LojaPedidos.Domain.Entities;

public sealed class Pedido : Entity
{
    private readonly List<Produto> _produtos = [];

    private Pedido()
    {
        Comprador = null!;
    }

    public Pedido(Comprador comprador, IEnumerable<Produto> produtos)
    {
        Comprador = ValidarComprador(comprador);
        AdicionarProdutos(produtos);
        Status = StatusPedido.Iniciado;
        CriadoEm = DateTimeOffset.UtcNow;
    }

    public Guid CompradorId { get; private set; }

    public Comprador Comprador { get; private set; }

    public IReadOnlyCollection<Produto> Produtos => _produtos.AsReadOnly();

    public StatusPedido Status { get; private set; }

    public DateTimeOffset CriadoEm { get; private set; }

    public DateTimeOffset? AtualizadoEm { get; private set; }

    public void Alterar(Comprador comprador, IEnumerable<Produto> produtos)
    {
        if (Status != StatusPedido.Iniciado)
        {
            throw new DomainException("Apenas pedidos não processados podem ser alterados.");
        }

        Comprador = ValidarComprador(comprador);
        _produtos.Clear();
        AdicionarProdutos(produtos);
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

    private void AdicionarProdutos(IEnumerable<Produto>? produtos)
    {
        if (produtos is null)
        {
            throw new DomainException("O pedido deve possuir pelo menos um produto.");
        }

        var produtosInformados = produtos.ToList();

        if (produtosInformados.Count == 0)
        {
            throw new DomainException("O pedido deve possuir pelo menos um produto.");
        }

        if (produtosInformados.Any(produto => produto is null))
        {
            throw new DomainException("O pedido não pode possuir um produto inválido.");
        }

        _produtos.AddRange(produtosInformados);
    }

    private void RegistrarAtualizacao()
    {
        AtualizadoEm = DateTimeOffset.UtcNow;
    }
}
