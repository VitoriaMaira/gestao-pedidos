using FluentValidation;
using LojaPedidos.Application.Pedidos.ConsultarPedido;
using LojaPedidos.Domain.Entities;
using LojaPedidos.Domain.Exceptions;
using LojaPedidos.Domain.Repositories;

namespace LojaPedidos.Application.Pedidos.AlterarPedido;

public sealed class AlterarPedidoUseCase(
    IPedidoRepository pedidoRepository,
    ICompradorRepository compradorRepository,
    IProdutoRepository produtoRepository,
    IUnitOfWork unitOfWork,
    IValidator<AlterarPedidoRequest> validator) : IAlterarPedidoUseCase
{
    public async Task<PedidoResponse?> ExecutarAsync(
        Guid id,
        AlterarPedidoRequest request,
        CancellationToken cancellationToken = default)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var pedido = await pedidoRepository.ObterPorIdAsync(id, cancellationToken);

        if (pedido is null)
        {
            return null;
        }

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

        pedido.Alterar(comprador, itens);
        await unitOfWork.CommitAsync(cancellationToken);

        return PedidoResponse.Criar(pedido);
    }
}
