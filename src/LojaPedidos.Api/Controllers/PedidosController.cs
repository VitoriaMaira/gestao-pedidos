using LojaPedidos.Application.Common.Responses;
using LojaPedidos.Application.Pedidos.AlterarPedido;
using LojaPedidos.Application.Pedidos.AtualizarStatusPedido;
using LojaPedidos.Application.Pedidos.ConsultarPedido;
using LojaPedidos.Application.Pedidos.CriarPedido;
using LojaPedidos.Application.Pedidos.ExcluirPedido;
using LojaPedidos.Application.Pedidos.ListarPedidos;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
    [HttpPost]
    [SwaggerOperation(
        Summary = "Cria um novo pedido",
        Description = "Cria o pedido com status Iniciado. Se o CPF já estiver cadastrado, " +
                      "o comprador existente será reutilizado.",
        OperationId = "CriarPedido",
        Tags = ["Pedidos"])]
    [SwaggerResponse(
        StatusCodes.Status201Created,
        "Pedido criado com sucesso.",
        typeof(ApiResponse<CriarPedidoResponse>))]
    [SwaggerResponse(
        StatusCodes.Status400BadRequest,
        "Dados do comprador ou dos itens inválidos.",
        typeof(ApiResponse<object>))]
    [SwaggerResponse(
        StatusCodes.Status404NotFound,
        "Um dos produtos informados não foi encontrado.",
        typeof(ApiResponse<object>))]
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


    [HttpGet("{id:guid}")]
    [SwaggerOperation(
        Summary = "Consulta um pedido",
        Description = "Retorna os dados do pedido, do comprador e de seus itens.",
        OperationId = "ConsultarPedido",
        Tags = ["Pedidos"])]
    [SwaggerResponse(
        StatusCodes.Status200OK,
        "Pedido consultado com sucesso.",
        typeof(ApiResponse<ConsultarPedidoResponse>))]
    [SwaggerResponse(
        StatusCodes.Status404NotFound,
        "Pedido não encontrado.",
        typeof(ApiResponse<object>))]
    public async Task<IActionResult> ConsultarPorId(
        Guid id,
        CancellationToken cancellationToken)
    {
        var response = await consultarPedidoUseCase.Execute(id, cancellationToken);

        return Ok(ApiResponse<ConsultarPedidoResponse>.Ok(
            "Pedido consultado com sucesso.",
            response));
    }


    [HttpGet]
    [SwaggerOperation(
        Summary = "Lista os pedidos",
        Description = "Retorna os pedidos de forma paginada. Os filtros por status e CPF são opcionais.",
        OperationId = "ListarPedidos",
        Tags = ["Pedidos"])]
    [SwaggerResponse(
        StatusCodes.Status200OK,
        "Pedidos listados com sucesso.",
        typeof(ApiResponse<ListarPedidosResponse>))]
    [SwaggerResponse(
        StatusCodes.Status400BadRequest,
        "Parâmetros de paginação ou filtros inválidos.",
        typeof(ApiResponse<object>))]
    public async Task<IActionResult> ListarAsync(
        [FromQuery] ListarPedidosRequest request,
        CancellationToken cancellationToken)
    {
        var response = await listarPedidosUseCase.Execute(request, cancellationToken);

        return Ok(ApiResponse<ListarPedidosResponse>.Ok(
            "Pedidos listados com sucesso.",
            response));
    }

    [HttpPut("{id:guid}")]
    [SwaggerOperation(
        Summary = "Altera um pedido",
        Description = "Altera as quantidades dos itens de um pedido com status Iniciado.",
        OperationId = "AlterarPedido",
        Tags = ["Pedidos"])]
    [SwaggerResponse(
        StatusCodes.Status200OK,
        "Pedido atualizado com sucesso.",
        typeof(ApiResponse<ConsultarPedidoResponse>))]
    [SwaggerResponse(
        StatusCodes.Status400BadRequest,
        "Itens inválidos ou pedido em um status que não permite alteração.",
        typeof(ApiResponse<object>))]
    [SwaggerResponse(
        StatusCodes.Status404NotFound,
        "Pedido não encontrado.",
        typeof(ApiResponse<object>))]
    public async Task<IActionResult> AlterarAsync(
        Guid id,
        [FromBody] AlterarPedidoRequest request,
        CancellationToken cancellationToken)
    {
        var response = await alterarPedidoUseCase.Execute(
            id,
            request,
            cancellationToken);

        return Ok(ApiResponse<ConsultarPedidoResponse>.Ok(
            "Pedido atualizado com sucesso.",
            response));
    }

    [HttpPut("{id:guid}/status")]
    [SwaggerOperation(
        Summary = "Atualiza o status de um pedido",
        Description = "Atualiza o status respeitando as transições permitidas para o pedido.",
        OperationId = "AtualizarStatusPedido",
        Tags = ["Pedidos"])]
    [SwaggerResponse(
        StatusCodes.Status200OK,
        "Status do pedido atualizado com sucesso.",
        typeof(ApiResponse<AtualizarStatusPedidoResponse>))]
    [SwaggerResponse(
        StatusCodes.Status400BadRequest,
        "Status inválido ou transição de status não permitida.",
        typeof(ApiResponse<object>))]
    [SwaggerResponse(
        StatusCodes.Status404NotFound,
        "Pedido não encontrado.",
        typeof(ApiResponse<object>))]
    public async Task<IActionResult> AtualizarStatusAsync(
        Guid id,
        [FromBody] AtualizarStatusPedidoRequest request,
        CancellationToken cancellationToken)
    {
        var response = await atualizarStatusPedidoUseCase.Execute(
            id,
            request,
            cancellationToken);

        return Ok(ApiResponse<AtualizarStatusPedidoResponse>.Ok(
            response.Mensagem,
            response));
    }

    [HttpDelete("{id:guid}")]
    [SwaggerOperation(
        Summary = "Cancela um pedido",
        Description = "Realiza a exclusão lógica do pedido, alterando seu status para Cancelado.",
        OperationId = "CancelarPedido",
        Tags = ["Pedidos"])]
    [SwaggerResponse(
        StatusCodes.Status200OK,
        "Pedido cancelado com sucesso.",
        typeof(ApiResponse<object>))]
    [SwaggerResponse(
        StatusCodes.Status404NotFound,
        "Pedido não encontrado.",
        typeof(ApiResponse<object>))]
    public async Task<IActionResult> ExcluirAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        await excluirPedidoUseCase.ExecutarAsync(id, cancellationToken);

        return Ok(ApiResponse<object>.Ok("Pedido cancelado com sucesso."));
    }

}
