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
}
