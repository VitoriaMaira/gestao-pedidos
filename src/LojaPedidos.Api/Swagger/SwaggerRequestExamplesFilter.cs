using System.Text.Json.Nodes;
using LojaPedidos.Api.Swagger.Examples.Pedidos;
using LojaPedidos.Api.Swagger.Examples.Produtos;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LojaPedidos.Api.Swagger;

public sealed class SwaggerRequestExamplesFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        ApplyRequestExample(operation, context);
        ApplyResponseExample(operation, context);
    }

    private static void ApplyRequestExample(
        OpenApiOperation operation,
        OperationFilterContext context)
    {
        var example = context.MethodInfo.Name switch
        {
            "CriarAsync" => CriarProdutoRequestExample.Value,
            "CriarPedidoAsync" => CriarPedidoRequestExample.Value,
            "AlterarAsync" => AlterarPedidoRequestExample.Value,
            "AtualizarStatusAsync" => AtualizarStatusPedidoRequestExample.Value,
            _ => null
        };

        var content = operation.RequestBody?.Content;

        if (example is null || content is null)
        {
            return;
        }

        foreach (var mediaType in content.Values)
        {
            mediaType.Example = JsonNode.Parse(example);
        }
    }

    private static void ApplyResponseExample(
        OpenApiOperation operation,
        OperationFilterContext context)
    {
        (string StatusCode, string Value)? responseExample =
            (context.MethodInfo.DeclaringType?.Name, context.MethodInfo.Name) switch
        {
            ("ProdutosController", "CriarAsync") => ("201", CriarProdutoResponseExample.Value),
            ("ProdutosController", "ListarAsync") => ("200", ListarProdutosResponseExample.Value),
            ("PedidosController", "CriarPedidoAsync") => ("201", CriarPedidoResponseExample.Value),
            ("PedidosController", "ConsultarPorId") => ("200", ConsultarPedidoResponseExample.Value),
            ("PedidosController", "ListarAsync") => ("200", ListarPedidosResponseExample.Value),
            ("PedidosController", "AlterarAsync") => ("200", AlterarPedidoResponseExample.Value),
            ("PedidosController", "AtualizarStatusAsync") => ("200", AtualizarStatusPedidoResponseExample.Value),
            ("PedidosController", "ExcluirAsync") => ("200", ExcluirPedidoResponseExample.Value),
            _ => ((string StatusCode, string Value)?)null
        };

        if (responseExample is null ||
            operation.Responses is null ||
            !operation.Responses.TryGetValue(responseExample.Value.StatusCode, out var response) ||
            response?.Content is null)
        {
            return;
        }

        foreach (var mediaType in response.Content.Values)
        {
            mediaType.Example = JsonNode.Parse(responseExample.Value.Value);
        }
    }
}
