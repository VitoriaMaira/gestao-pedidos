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
        var example = (context.MethodInfo.DeclaringType?.Name, context.MethodInfo.Name) switch
        {
            ("ProdutosController", "ListarAsync") => ListarProdutosResponseExample.Value,
            ("PedidosController", "ConsultarPorId") => ConsultarPedidoResponseExample.Value,
            ("PedidosController", "ListarAsync") => ListarPedidosResponseExample.Value,
            ("PedidosController", "ExcluirAsync") => ExcluirPedidoResponseExample.Value,
            _ => null
        };

        if (example is null ||
            operation.Responses is null ||
            !operation.Responses.TryGetValue("200", out var response) ||
            response?.Content is null)
        {
            return;
        }

        foreach (var mediaType in response.Content.Values)
        {
            mediaType.Example = JsonNode.Parse(example);
        }
    }
}
