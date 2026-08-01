using FluentValidation;
using LojaPedidos.Application.Pedidos.ConsultarPedido;
using LojaPedidos.Domain.Repositories;

namespace LojaPedidos.Application.Pedidos.ListarPedidos;

public sealed class ListarPedidosUseCase(
    IPedidoRepository pedidoRepository,
    IValidator<ListarPedidosRequest> validator) : IListarPedidosUseCase
{
    public async Task<ListarPedidosResponse> ExecutarAsync(
        ListarPedidosRequest request,
        CancellationToken cancellationToken = default)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var (pedidos, total) = await pedidoRepository.ListarAsync(
            request.Pagina,
            request.TamanhoPagina,
            request.Status,
            request.CompradorId,
            cancellationToken);

        var itens = pedidos.Select(PedidoResponse.Criar).ToArray();

        return new ListarPedidosResponse(
            itens,
            request.Pagina,
            request.TamanhoPagina,
            total);
    }
}
