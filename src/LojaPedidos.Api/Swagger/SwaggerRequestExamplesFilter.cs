using System.Text.Json.Nodes;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LojaPedidos.Api.Swagger;

public sealed class SwaggerRequestExamplesFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var example = context.MethodInfo.Name switch
        {
            "CriarPedidoAsync" => CriarPedidoExample,
            "AlterarAsync" => AlterarPedidoExample,
            "AtualizarStatusAsync" => AtualizarStatusExample,
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

    private const string CriarPedidoExample = """
        {
          "comprador": {
            "nome": "João da Silva",
            "cpf": "52998224725"
          },
          "itens": [
            {
              "produto": {
                "nome": "Teclado mecânico",
                "preco": 150.00
              },
              "quantidade": 1
            }
          ]
        }
        """;

    private const string AlterarPedidoExample = """
        {
          "itens": [
            {
              "itemId": "019fbdbf-e925-7260-9976-f61483c85ad4",
              "quantidade": 2
            }
          ]
        }
        """;

    private const string AtualizarStatusExample = """
        {
          "status": "Processado"
        }
        """;
}
