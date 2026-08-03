using LojaPedidos.Application.Common.Responses;
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
    IConsultarPedidoUseCase consultarPedidoUseCase,
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
    [ProducesResponseType<ApiResponse<CriarPedidoResponse>>(StatusCodes.Status201Created)]
    [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CriarPedidoAsync(
        [FromBody] CriarPedidoRequest request,
        CancellationToken cancellationToken)
    {
        var response = await criarPedidoUseCase.ExecutarAsync(request, cancellationToken);

        return CreatedAtAction(
            nameof(ConsultarPorId),
            new { id = response.Id },
            ApiResponse<CriarPedidoResponse>.Ok(
                "Pedido criado com sucesso.",
                response));
    }

    /// <summary>
    /// Busca um pedido pelo identificador.
    /// </summary>
    /// <param name="id">Identificador do pedido.</param>
    /// <param name="cancellationToken">Token de cancelamento da requisição.</param>
    [HttpGet("{id:guid}")]
    [ProducesResponseType<ApiResponse<ConsultarPedidoResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ConsultarPorId(
        Guid id,
        CancellationToken cancellationToken)
    {
        var response = await consultarPedidoUseCase.Execute(id, cancellationToken);

        return Ok(ApiResponse<ConsultarPedidoResponse>.Ok(
            "Pedido consultado com sucesso.",
            response));
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
    [ProducesResponseType<ApiResponse<ListarPedidosResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ListarAsync(
        [FromQuery] ListarPedidosRequest request,
        CancellationToken cancellationToken)
    {
        var response = await listarPedidosUseCase.Execute(request, cancellationToken);

        return Ok(ApiResponse<ListarPedidosResponse>.Ok(
            "Pedidos listados com sucesso.",
            response));
    }

    /// <summary>
    /// Altera as quantidades dos itens de um pedido iniciado.
    /// </summary>
    /// <param name="id">Identificador do pedido.</param>
    /// <param name="request">Itens e novas quantidades.</param>
    /// <param name="cancellationToken">Token de cancelamento da requisição.</param>
    [HttpPut("{id:guid}")]
    [ProducesResponseType<ApiResponse<ConsultarPedidoResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AlterarAsync(
        Guid id,
        [FromBody] AlterarPedidoRequest request,
        CancellationToken cancellationToken)
    {
        var response = await alterarPedidoUseCase.ExecutarAsync(
            id,
            request,
            cancellationToken);

        return Ok(ApiResponse<ConsultarPedidoResponse>.Ok(
            "Pedido atualizado com sucesso.",
            response));
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
    [ProducesResponseType<ApiResponse<AtualizarStatusPedidoResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AtualizarStatusAsync(
        Guid id,
        [FromBody] AtualizarStatusPedidoRequest request,
        CancellationToken cancellationToken)
    {
        var response = await atualizarStatusPedidoUseCase.ExecutarAsync(
            id,
            request,
            cancellationToken);

        return Ok(ApiResponse<AtualizarStatusPedidoResponse>.Ok(
            response.Mensagem,
            response));
    }

    /// <summary>
    /// Realiza a exclusão lógica de um pedido.
    /// </summary>
    /// <remarks>
    /// O pedido não é removido do banco de dados; seu status é alterado para Cancelado.
    /// </remarks>
    /// <param name="id">Identificador do pedido.</param>
    /// <param name="cancellationToken">Token de cancelamento da requisição.</param>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ExcluirAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        await excluirPedidoUseCase.ExecutarAsync(id, cancellationToken);

        return Ok(ApiResponse<object>.Ok("Pedido cancelado com sucesso."));
    }

}
