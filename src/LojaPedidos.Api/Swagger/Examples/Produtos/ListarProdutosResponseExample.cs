namespace LojaPedidos.Api.Swagger.Examples.Produtos;

internal static class ListarProdutosResponseExample
{
    public const string Value = """
        {
          "mensagem": "Produtos listados com sucesso.",
          "dados": {
            "itens": [
              {
                "id": "019fbdbf-e925-7260-9976-f61483c85ad4",
                "nome": "Teclado mecânico",
                "preco": 150.00
              },
              {
                "id": "019fbdbf-e925-7260-9976-f61483c85ad5",
                "nome": "Webcam Full HD",
                "preco": 249.90
              }
            ],
            "pagina": 1,
            "tamanhoPagina": 10,
            "total": 2
          },
          "sucesso": true
        }
        """;
}
