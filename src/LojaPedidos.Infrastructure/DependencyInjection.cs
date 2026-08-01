using LojaPedidos.Domain.Repositories;
using LojaPedidos.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LojaPedidos.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("lojapedidos")
            ?? throw new InvalidOperationException(
                "A conexão 'lojapedidos' não foi configurada.");

        services.AddDbContext<LojaPedidosDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IPedidoRepository, PedidoRepository>();
        services.AddScoped<ICompradorRepository, CompradorRepository>();
        services.AddScoped<IProdutoRepository, ProdutoRepository>();

        return services;
    }
}
