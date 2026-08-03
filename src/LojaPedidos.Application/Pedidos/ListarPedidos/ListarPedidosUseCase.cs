using FluentValidation;
using LojaPedidos.Domain.Repositories;
using LojaPedidos.Domain.ValueObjects;

namespace LojaPedidos.Application.Pedidos.ListarPedidos;

public interface IListarPedidosUseCase
{
    Task<ListarPedidosResponse> Execute(ListarPedidosRequest request, CancellationToken cancellationToken = default);
}

public sealed class ListarPedidosUseCase(
    IPedidoRepository pedidoRepository,
    IValidator<ListarPedidosRequest> validator) : IListarPedidosUseCase
{
    public async Task<ListarPedidosResponse> Execute(
        ListarPedidosRequest request,
        CancellationToken cancellationToken = default)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var cpf = string.IsNullOrWhiteSpace(request.Cpf)
            ? null
            : Cpf.Normalizar(request.Cpf);

        var (pedidos, total) = await pedidoRepository.ListarAsync(
            request.Pagina,
            request.TamanhoPagina,
            request.Status,
            cpf,
            cancellationToken);

        return ListarPedidosResponse.Map(
            pedidos,
            request.Pagina,
            request.TamanhoPagina,
            total);
    }
}
