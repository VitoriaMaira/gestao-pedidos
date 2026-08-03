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
                "preco": 150.00,
                "imagemUrl": "https://commons.wikimedia.org/wiki/Special:FilePath/Mechanical%20Keyboard.jpg"
              },
              {
                "id": "019fbdbf-e925-7260-9976-f61483c85ad5",
                "nome": "Webcam Full HD",
                "preco": 249.90,
                "imagemUrl": "https://commons.wikimedia.org/wiki/Special:FilePath/USB%20webcam%20for%20PC.jpg"
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
