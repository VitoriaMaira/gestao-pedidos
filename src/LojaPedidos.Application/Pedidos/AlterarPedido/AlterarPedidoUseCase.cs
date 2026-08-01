using FluentValidation;
using LojaPedidos.Application.Common.Exceptions;
using LojaPedidos.Application.Pedidos.ConsultarPedido;
using LojaPedidos.Domain.Repositories;

namespace LojaPedidos.Application.Pedidos.AlterarPedido;

public sealed class AlterarPedidoUseCase(
    IPedidoRepository pedidoRepository,
    IUnitOfWork unitOfWork,
    IValidator<AlterarPedidoRequest> validator) : IAlterarPedidoUseCase
{
    public async Task<PedidoResponse> ExecutarAsync(
        Guid id,
        AlterarPedidoRequest request,
        CancellationToken cancellationToken = default)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var pedido = await pedidoRepository.ObterPorIdAsync(id, cancellationToken);

        if (pedido is null)
        {
            throw new NotFoundException("Não foi possível encontrar o pedido informado.");
        }

        foreach (var itemRequest in request.Itens!)
        {
            pedido.AlterarQuantidadeItem(
                itemRequest.ItemId,
                itemRequest.Quantidade);
        }

        await unitOfWork.CommitAsync(cancellationToken);

        return PedidoResponse.Criar(pedido);
    }
}
