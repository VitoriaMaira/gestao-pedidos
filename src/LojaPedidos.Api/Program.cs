using LojaPedidos.Api.Configurations;
using LojaPedidos.Api.Filters;
using LojaPedidos.Application;
using LojaPedidos.Application.Common.Responses;
using LojaPedidos.Infrastructure;
using LojaPedidos.Infrastructure.DataAccess;
using LojaPedidos.Infrastructure.DataAccess.Seeds;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerConfiguration();
builder.Services.AddCorsConfiguration(builder.Configuration);
builder.Services
    .AddControllers(options => options.Filters.Add<ApiExceptionFilter>())
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var mensagem = string.Join(
                " ",
                context.ModelState.Values
                    .SelectMany(value => value.Errors)
                    .Select(error => error.ErrorMessage)
                    .Where(error => !string.IsNullOrWhiteSpace(error))
                    .Distinct());

            return new BadRequestObjectResult(
                ApiResponse<object>.Erro(
                    string.IsNullOrWhiteSpace(mensagem)
                        ? "A requisição possui dados inválidos."
                        : mensagem));
        };
    });

builder.AddServiceDefaults();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseSwaggerConfiguration();

app.UseCorsConfiguration();

await using var scope = app.Services.CreateAsyncScope();
var dbContext = scope.ServiceProvider.GetRequiredService<LojaPedidosDbContext>();

await dbContext.Database.MigrateAsync();
await DatabaseSeeder.SeedAsync(dbContext);

app.MapDefaultEndpoints();
app.MapControllers();

app.Run();
