namespace LojaPedidos.Api.Swagger.Examples.Pedidos;

internal static class CriarPedidoRequestExample
{
    public const string Value = """
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
}
