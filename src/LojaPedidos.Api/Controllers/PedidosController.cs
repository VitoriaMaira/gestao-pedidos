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
[Produces("application/json")]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
public sealed class PedidosController(
    ICriarPedidoUseCase criarPedidoUseCase,
    IObterPedidoPorIdUseCase obterPedidoPorIdUseCase,
    IListarPedidosUseCase listarPedidosUseCase,
    IAlterarPedidoUseCase alterarPedidoUseCase,
    IAtualizarStatusPedidoUseCase atualizarStatusPedidoUseCase,
    IExcluirPedidoUseCase excluirPedidoUseCase) : ControllerBase
{
    /// <summary>
    /// Cria um pedido com os dados do comprador e dos produtos.
    /// </summary>
    /// <remarks>
    /// Se o CPF já estiver cadastrado, o comprador existente será reutilizado.
    /// O pedido é criado com o status Iniciado.
    /// </remarks>
    /// <param name="request">Dados do comprador e dos itens do pedido.</param>
    /// <param name="cancellationToken">Token de cancelamento da requisição.</param>
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

    /// <summary>
    /// Busca um pedido pelo identificador.
    /// </summary>
    /// <param name="id">Identificador do pedido.</param>
    /// <param name="cancellationToken">Token de cancelamento da requisição.</param>
    [HttpGet("{id:guid}")]
    [ProducesResponseType<PedidoResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(
        Guid id,
        CancellationToken cancellationToken)
    {
        var response = await obterPedidoPorIdUseCase.ExecutarAsync(id, cancellationToken);

        return Ok(response);
    }

    /// <summary>
    /// Lista os pedidos de forma paginada.
    /// </summary>
    /// <remarks>
    /// Os filtros de status e CPF são opcionais. O tamanho máximo da página é 100.
    /// </remarks>
    /// <param name="request">Paginação e filtros opcionais da consulta.</param>
    /// <param name="cancellationToken">Token de cancelamento da requisição.</param>
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

    /// <summary>
    /// Altera as quantidades dos itens de um pedido iniciado.
    /// </summary>
    /// <param name="id">Identificador do pedido.</param>
    /// <param name="request">Itens e novas quantidades.</param>
    /// <param name="cancellationToken">Token de cancelamento da requisição.</param>
    [HttpPut("{id:guid}")]
    [ProducesResponseType<PedidoResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
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

    /// <summary>
    /// Atualiza o status do pedido respeitando as transições permitidas.
    /// </summary>
    /// <remarks>
    /// Um pedido iniciado pode ser processado ou cancelado. Um pedido processado
    /// pode ser enviado ou cancelado.
    /// </remarks>
    /// <param name="id">Identificador do pedido.</param>
    /// <param name="request">Novo status do pedido.</param>
    /// <param name="cancellationToken">Token de cancelamento da requisição.</param>
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

    /// <summary>
    /// Exclui definitivamente um pedido.
    /// </summary>
    /// <remarks>
    /// A exclusão é diferente do cancelamento, que deve ser feito pela atualização de status.
    /// </remarks>
    /// <param name="id">Identificador do pedido.</param>
    /// <param name="cancellationToken">Token de cancelamento da requisição.</param>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType<ExcluirPedidoResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ExcluirAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var response = await excluirPedidoUseCase.ExecutarAsync(id, cancellationToken);

        return Ok(response);
    }

}
