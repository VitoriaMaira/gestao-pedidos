namespace LojaPedidos.Api.Configurations;

public static class CorsConfiguration
{
    private const string FrontendPolicy = "Frontend";

    public static IServiceCollection AddCorsConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var allowedOrigins = configuration
            .GetSection("Cors:AllowedOrigins")
            .Get<string[]>() ?? [];

        services.AddCors(options =>
        {
            options.AddPolicy(FrontendPolicy, policy =>
            {
                if (allowedOrigins.Length > 0)
                {
                    policy
                        .WithOrigins(allowedOrigins)
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                }
            });
        });

        return services;
    }

    public static WebApplication UseCorsConfiguration(this WebApplication app)
    {
        app.UseCors(FrontendPolicy);
        return app;
    }
}
