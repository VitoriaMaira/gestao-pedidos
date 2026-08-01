using FluentValidation;
using LojaPedidos.Application.Pedidos.ConsultarPedido;
using LojaPedidos.Domain.Repositories;

namespace LojaPedidos.Application.Pedidos.AlterarPedido;

public sealed class AlterarPedidoUseCase(
    IPedidoRepository pedidoRepository,
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
