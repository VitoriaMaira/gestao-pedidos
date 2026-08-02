using LojaPedidos.Application.Produtos.Criar;
using LojaPedidos.Application.Produtos.Listar;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace LojaPedidos.Api.Controllers;

[ApiController]
[Route("api/produtos")]
public sealed class ProdutosController : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(
        Summary = "Lista os produtos",
        Description = "Retorna os produtos de forma paginada, ordenados por nome.",
        OperationId = "ListarProdutos",
        Tags = ["Produtos"])]
    [SwaggerResponse(
        StatusCodes.Status200OK,
        "Produtos listados com sucesso.",
        typeof(ListarProdutosResponse))]
    [SwaggerResponse(
        StatusCodes.Status400BadRequest,
        "Parâmetros de paginação inválidos.",
        typeof(ValidationProblemDetails))]
    public async Task<IActionResult> ListarAsync(
        [FromServices] IListarProdutosUseCase useCase,
        [FromQuery] ListarProdutosQuery request,
        CancellationToken cancellationToken)
    {
        var response = await useCase.ExecutarAsync(request, cancellationToken);
        return Ok(response);
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Cadastra um novo produto",
        Description = "O nome deve ser único e possuir no máximo 150 caracteres. " +
                      "O preço deve ser maior que zero.",
        OperationId = "CriarProduto",
        Tags = ["Produtos"])]
    [SwaggerResponse(
        StatusCodes.Status201Created,
        "Produto criado com sucesso.",
        typeof(CriarProdutoResponse))]
    [SwaggerResponse(
        StatusCodes.Status400BadRequest,
        "Dados inválidos ou produto com nome já cadastrado.",
        typeof(ValidationProblemDetails))]
    public async Task<IActionResult> CriarAsync(
        [FromServices] ICriarProdutoUseCase useCase,
        [FromBody] CriarProdutoRequest request,
        CancellationToken cancellationToken)
    {
        var response = await useCase.Execute(request, cancellationToken);
        return Created($"/api/produtos/{response.Id}", response);
    }
}
