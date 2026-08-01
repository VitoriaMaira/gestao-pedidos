using FluentValidation;
using LojaPedidos.Domain.Entities;
using LojaPedidos.Domain.Exceptions;
using LojaPedidos.Domain.Repositories;

namespace LojaPedidos.Application.Pedidos.CriarPedido;

public sealed class CriarPedidoUseCase(
    ICompradorRepository compradorRepository,
    IProdutoRepository produtoRepository,
    IPedidoRepository pedidoRepository,
    IUnitOfWork unitOfWork,
    IValidator<CriarPedidoRequest> validator)
{
    public async Task<CriarPedidoResponse> ExecutarAsync(
        CriarPedidoRequest request,
        CancellationToken cancellationToken = default)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var comprador = await compradorRepository.ObterPorIdAsync(
            request.CompradorId,
            cancellationToken);

        if (comprador is null)
        {
            throw new DomainException("Comprador não encontrado.");
        }

        var itens = new List<ItemPedido>();

        foreach (var itemRequest in request.Itens!)
        {
            var produto = await produtoRepository.ObterPorIdAsync(
                itemRequest.ProdutoId,
                cancellationToken);

            if (produto is null)
            {
                throw new DomainException("Produto não encontrado.");
            }

            itens.Add(new ItemPedido(produto, itemRequest.Quantidade));
        }

        var pedido = new Pedido(comprador, itens);

        await pedidoRepository.AdicionarAsync(pedido, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return new CriarPedidoResponse(
            pedido.Id,
            pedido.Status,
            pedido.Total,
            pedido.CriadoEm);
    }
}
