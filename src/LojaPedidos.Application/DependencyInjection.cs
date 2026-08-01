using FluentValidation;
using LojaPedidos.Application.Pedidos.CriarPedido;
using Microsoft.Extensions.DependencyInjection;

namespace LojaPedidos.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<CriarPedidoRequestValidator>();
        services.AddScoped<CriarPedidoUseCase>();

        return services;
    }
}
