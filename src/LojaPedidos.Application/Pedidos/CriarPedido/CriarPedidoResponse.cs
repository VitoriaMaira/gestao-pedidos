using LojaPedidos.Domain.Entities;
using LojaPedidos.Domain.Enums;

namespace LojaPedidos.Application.Pedidos.CriarPedido;

public sealed record CriarPedidoResponse(
    string Mensagem,
    Guid Id,
    CriarPedidoCompradorResponse Comprador,
    StatusPedido Status,
    decimal Total,
    DateTimeOffset CriadoEm,
    IReadOnlyCollection<CriarPedidoItemResponse> Itens)
{
    public static CriarPedidoResponse Criar(Pedido pedido, string mensagem)
    {
        var itens = pedido.Itens
            .Select(item => new CriarPedidoItemResponse(
                item.Id,
                new CriarPedidoProdutoResponse(
                    item.Produto.Id,
                    item.Produto.Nome,
                    item.Produto.Preco),
                item.Quantidade,
                item.PrecoUnitario,
                item.Subtotal))
            .ToArray();

        return new CriarPedidoResponse(
            mensagem,
            pedido.Id,
            new CriarPedidoCompradorResponse(
                pedido.Comprador.Id,
                pedido.Comprador.Nome,
                pedido.Comprador.Cpf),
            pedido.Status,
            pedido.Total,
            pedido.CriadoEm,
            itens);
    }
}

public sealed record CriarPedidoCompradorResponse(
    Guid Id,
    string Nome,
    string Cpf);

public sealed record CriarPedidoProdutoResponse(
    Guid Id,
    string Nome,
    decimal Preco);

public sealed record CriarPedidoItemResponse(
    Guid Id,
    CriarPedidoProdutoResponse Produto,
    int Quantidade,
    decimal PrecoUnitario,
    decimal Subtotal);
