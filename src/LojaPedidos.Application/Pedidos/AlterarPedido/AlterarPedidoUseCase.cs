using FluentValidation;
using LojaPedidos.Application.Common.Exceptions;
using LojaPedidos.Application.Pedidos.ConsultarPedido;
using LojaPedidos.Domain.Enums;
using LojaPedidos.Domain.Repositories;

namespace LojaPedidos.Application.Pedidos.AlterarPedido;

public interface IAlterarPedidoUseCase
{
    Task<ConsultarPedidoResponse> Execute(Guid id, AlterarPedidoRequest request, CancellationToken cancellationToken = default);
}

public sealed class AlterarPedidoUseCase(
    IPedidoRepository pedidoRepository,
    IUnitOfWork unitOfWork,
    IValidator<AlterarPedidoRequest> validator) : IAlterarPedidoUseCase
{
    public async Task<ConsultarPedidoResponse> Execute(
        Guid id,
        AlterarPedidoRequest request,
        CancellationToken cancellationToken = default)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var pedido = await pedidoRepository.ObterPorId(id, cancellationToken);

        if (pedido is null)
            throw new NotFoundException("Não foi possível encontrar o pedido informado.");

        if (pedido.Status != StatusPedido.Iniciado)
            throw new ErrorOnValidationException(["Apenas pedidos não processados podem ser alterados."]);

        foreach (var itemRequest in request.Itens!)
        {
            var item = pedido.Itens.SingleOrDefault(item => item.Id == itemRequest.ItemId);

            if (item is null)
                throw new ErrorOnValidationException(["O item informado não pertence ao pedido."]);

            item.AlterarQuantidade(itemRequest.Quantidade);
        }

        pedido.RegistrarAtualizacao();

        await unitOfWork.CommitAsync(cancellationToken);

        return ConsultarPedidoResponse.Map(pedido);
    }
}
