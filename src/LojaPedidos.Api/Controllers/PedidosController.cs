using LojaPedidos.Application.Pedidos.AlterarPedido;
using LojaPedidos.Application.Pedidos.AtualizarStatusPedido;
using LojaPedidos.Application.Pedidos.ConsultarPedido;
using LojaPedidos.Application.Pedidos.CriarPedido;
using LojaPedidos.Application.Pedidos.ExcluirPedido;
using LojaPedidos.Application.Pedidos.ListarPedidos;
using Microsoft.AspNetCore.Mvc;

namespace LojaPedidos.Api.Controllers;

[ApiController]
[Route("api/pedidos")]
public sealed class PedidosController(
    ICriarPedidoUseCase criarPedidoUseCase,
    IObterPedidoPorIdUseCase obterPedidoPorIdUseCase,
    IListarPedidosUseCase listarPedidosUseCase,
    IAlterarPedidoUseCase alterarPedidoUseCase,
    IAtualizarStatusPedidoUseCase atualizarStatusPedidoUseCase,
    IExcluirPedidoUseCase excluirPedidoUseCase) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<CriarPedidoResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CriarPedidoAsync(
        [FromBody] CriarPedidoRequest request,
        CancellationToken cancellationToken)
    {
        var response = await criarPedidoUseCase.ExecutarAsync(request, cancellationToken);

        return CreatedAtAction(
            nameof(ObterPorId),
            new { id = response.Id },
            response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType<PedidoResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(
        Guid id,
        CancellationToken cancellationToken)
    {
        var response = await obterPedidoPorIdUseCase.ExecutarAsync(id, cancellationToken);

        return Ok(response);
    }

    [HttpGet]
    [ProducesResponseType<ListarPedidosResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ListarAsync(
        [FromQuery] ListarPedidosRequest request,
        CancellationToken cancellationToken)
    {
        var response = await listarPedidosUseCase.ExecutarAsync(request, cancellationToken);

        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType<PedidoResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AlterarAsync(
        Guid id,
        [FromBody] AlterarPedidoRequest request,
        CancellationToken cancellationToken)
    {
        var response = await alterarPedidoUseCase.ExecutarAsync(
            id,
            request,
            cancellationToken);

        return Ok(response);
    }

    [HttpPut("{id:guid}/status")]
    [ProducesResponseType<AtualizarStatusPedidoResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AtualizarStatusAsync(
        Guid id,
        [FromBody] AtualizarStatusPedidoRequest request,
        CancellationToken cancellationToken)
    {
        var response = await atualizarStatusPedidoUseCase.ExecutarAsync(
            id,
            request,
            cancellationToken);

        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ExcluirAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        await excluirPedidoUseCase.ExecutarAsync(id, cancellationToken);

        return NoContent();
    }

}
