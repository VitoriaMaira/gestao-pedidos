using LojaPedidos.Application.Produtos.Criar;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace LojaPedidos.Api.Controllers;

[ApiController]
[Route("api/produtos")]
public sealed class ProdutosController : ControllerBase
{
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
