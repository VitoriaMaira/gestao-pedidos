using LojaPedidos.Application.Pedidos.CriarPedido;
using Microsoft.AspNetCore.Mvc;

namespace LojaPedidos.Api.Controllers;

[ApiController]
[Route("api/pedidos")]
public sealed class PedidosController(CriarPedidoUseCase criarPedidoUseCase) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<CriarPedidoResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CriarAsync(
        [FromBody] CriarPedidoRequest request,
        CancellationToken cancellationToken)
    {
        var response = await criarPedidoUseCase.ExecutarAsync(request, cancellationToken);

        return Created($"/api/pedidos/{response.Id}", response);
    }
}
