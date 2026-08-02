using LojaPedidos.Api.Swagger;
using Microsoft.OpenApi;

namespace LojaPedidos.Api.Configurations;

public static class SwaggerConfiguration
{
    public static IServiceCollection AddSwaggerConfiguration(
        this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Loja Pedidos API",
                Description = "API REST para criação, consulta e gerenciamento " +
                              "de pedidos de uma loja."
            });
            options.IncludeXmlComments(typeof(Program).Assembly);
            options.EnableAnnotations();
            options.OperationFilter<SwaggerRequestExamplesFilter>();
        });

        return services;
    }

    public static WebApplication UseSwaggerConfiguration(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint(
                "/swagger/v1/swagger.json",
                "Loja Pedidos API v1");
            options.DocumentTitle = "Loja Pedidos API";
        });

        return app;
    }
}
