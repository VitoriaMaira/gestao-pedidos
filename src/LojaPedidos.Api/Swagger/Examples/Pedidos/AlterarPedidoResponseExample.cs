namespace LojaPedidos.Api.Swagger.Examples.Pedidos;

internal static class AlterarPedidoResponseExample
{
    public const string Value = """
        {
          "mensagem": "Pedido atualizado com sucesso.",
          "dados": {
            "id": "019fbdbf-e925-7260-9976-f61483c85ad6",
            "compradorId": "019fbdbf-e925-7260-9976-f61483c85ad7",
            "comprador": "João da Silva",
            "status": "Iniciado",
            "total": 300.00,
            "criadoEm": "2026-08-02T18:30:00-03:00",
            "itens": [
              {
                "id": "019fbdbf-e925-7260-9976-f61483c85ad8",
                "produtoId": "019fbdbf-e925-7260-9976-f61483c85ad4",
                "produto": "Teclado mecânico",
                "imagemUrl": "https://commons.wikimedia.org/wiki/Special:FilePath/Mechanical%20Keyboard.jpg",
                "quantidade": 2,
                "precoUnitario": 150.00,
                "subtotal": 300.00
              }
            ]
          },
          "sucesso": true
        }
        """;
}
