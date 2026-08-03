namespace LojaPedidos.Api.Swagger.Examples.Pedidos;

internal static class CriarPedidoRequestExample
{
    public const string Value = """
        {
          "nomeComprador": "João da Silva",
          "cpfComprador": "52998224725",
          "itens": [
            {
              "id": "019fbdbf-e925-7260-9976-f61483c85ad4",
              "quantidade": 1
            }
          ]
        }
        """;
}
