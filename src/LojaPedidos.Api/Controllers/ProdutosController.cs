using LojaPedidos.Application.Produtos.Criar;
using Microsoft.AspNetCore.Mvc;

namespace LojaPedidos.Api.Controllers;

[ApiController]
[Route("api/produtos")]
public sealed class ProdutosController : ControllerBase
{
    /// <summary>
    /// Cadastra um novo produto.
    /// </summary>
    /// <remarks>
    /// O nome do produto deve ser único e possuir no máximo 150 caracteres.
    /// O preço deve ser maior que zero.
    /// </remarks>
    /// <param name="useCase">Caso de uso responsável pela criação do produto.</param>
    /// <param name="request">Nome e preço do produto que será cadastrado.</param>
    /// <param name="cancellationToken">Token de cancelamento da requisição.</param>
    /// <returns>O identificador do produto criado.</returns>
    [HttpPost]
    [ProducesResponseType<CriarProdutoResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CriarAsync(
        [FromServices] ICriarProdutoUseCase useCase,
        [FromBody] CriarProdutoRequest request,
        CancellationToken cancellationToken)
    {
        var response = await useCase.Execute(request, cancellationToken);
        return Created($"/api/produtos/{response.Id}", response);
    }
}
