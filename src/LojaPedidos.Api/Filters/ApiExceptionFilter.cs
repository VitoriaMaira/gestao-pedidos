using FluentValidation;
using LojaPedidos.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LojaPedidos.Api.Filters;

public sealed class ApiExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        context.Result = context.Exception switch
        {
            ValidationException exception => CriarResultadoValidacao(exception),
            DomainException exception => CriarResultadoDominio(exception),
            _ => null
        };

        context.ExceptionHandled = context.Result is not null;
    }

    private static BadRequestObjectResult CriarResultadoValidacao(ValidationException exception)
    {
        var errors = exception.Errors
            .GroupBy(error => error.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group.Select(error => error.ErrorMessage).ToArray());

        return new BadRequestObjectResult(new ValidationProblemDetails(errors));
    }

    private static BadRequestObjectResult CriarResultadoDominio(DomainException exception)
    {
        var problem = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Não foi possível concluir a operação.",
            Detail = exception.Message
        };

        return new BadRequestObjectResult(problem);
    }
}
