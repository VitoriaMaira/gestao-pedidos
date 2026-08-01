using FluentValidation;
using LojaPedidos.Application.Pedidos.AlterarPedido;
using LojaPedidos.Application.Pedidos.CancelarPedido;
using LojaPedidos.Application.Pedidos.ConsultarPedido;
using LojaPedidos.Application.Pedidos.CriarPedido;
using LojaPedidos.Application.Pedidos.ExcluirPedido;
using LojaPedidos.Application.Pedidos.ListarPedidos;
using Microsoft.Extensions.DependencyInjection;

namespace LojaPedidos.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<CriarPedidoRequestValidator>();
        services.AddScoped<ICriarPedidoUseCase, CriarPedidoUseCase>();
        services.AddScoped<IObterPedidoPorIdUseCase, ObterPedidoPorIdUseCase>();
        services.AddScoped<IListarPedidosUseCase, ListarPedidosUseCase>();
        services.AddScoped<IAlterarPedidoUseCase, AlterarPedidoUseCase>();
        services.AddScoped<IExcluirPedidoUseCase, ExcluirPedidoUseCase>();
        services.AddScoped<ICancelarPedidoUseCase, CancelarPedidoUseCase>();

        return services;
    }
}
