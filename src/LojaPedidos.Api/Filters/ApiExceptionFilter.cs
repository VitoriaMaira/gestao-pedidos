using FluentValidation;
using LojaPedidos.Application.Common.Exceptions;
using LojaPedidos.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LojaPedidos.Api.Filters;

public sealed class ApiExceptionFilter(ILogger<ApiExceptionFilter> logger) : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        context.Result = context.Exception switch
        {
            ValidationException exception => CriarResultadoValidacao(exception),
            DomainException exception => CriarResultado(
                StatusCodes.Status400BadRequest,
                "Não foi possível concluir a operação.",
                exception.Message),
            NotFoundException exception => CriarResultado(
                StatusCodes.Status404NotFound,
                "Pedido não encontrado.",
                exception.Message),
            _ => CriarResultadoErroInterno(context.Exception)
        };

        context.ExceptionHandled = true;
    }

    private static BadRequestObjectResult CriarResultadoValidacao(ValidationException exception)
    {
        var errors = exception.Errors
            .GroupBy(error => error.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group.Select(error => error.ErrorMessage).ToArray());

        var problem = new ValidationProblemDetails(errors)
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Existem dados inválidos na requisição."
        };

        return new BadRequestObjectResult(problem);
    }

    private static ObjectResult CriarResultado(int status, string title, string detail)
    {
        var problem = new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = detail
        };

        return new ObjectResult(problem) { StatusCode = status };
    }

    private ObjectResult CriarResultadoErroInterno(Exception exception)
    {
        logger.LogError(exception, "Ocorreu um erro inesperado durante a requisição.");

        return CriarResultado(
            StatusCodes.Status500InternalServerError,
            "Ocorreu um erro inesperado.",
            "Não foi possível concluir a operação. Tente novamente mais tarde.");
    }
}
