using System.Text.Json.Serialization;
using LojaPedidos.Application;
using LojaPedidos.Api.Filters;
using LojaPedidos.Api.Swagger;
using LojaPedidos.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Loja Pedidos API",
        Version = "v1",
        Description = "API REST para criação, consulta e gerenciamento de pedidos de uma loja."
    });
    options.IncludeXmlComments(typeof(Program).Assembly);
    options.OperationFilter<SwaggerRequestExamplesFilter>();
});
builder.Services
    .AddControllers(options => options.Filters.Add<ApiExceptionFilter>())
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.AddServiceDefaults();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Loja Pedidos API v1");
    options.DocumentTitle = "Loja Pedidos API";
});

if (app.Environment.IsDevelopment())
{
    await using var scope = app.Services.CreateAsyncScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<LojaPedidosDbContext>();

    await dbContext.Database.MigrateAsync();
}

app.MapDefaultEndpoints();
app.MapControllers();

app.Run();
