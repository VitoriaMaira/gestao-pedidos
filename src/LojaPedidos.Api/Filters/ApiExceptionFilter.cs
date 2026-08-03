using FluentValidation;
using LojaPedidos.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LojaPedidos.Api.Filters;

public sealed class ApiExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is ValidationException validationException)
        {
            HandleValidationException(context, validationException);
        }
        else if (context.Exception is LojaPedidosException)
        {
            HandleProjectException(context);
        }
        else
        {
            ThrowUnkowError(context);
        }

        context.ExceptionHandled = true;
    }

    private static void HandleValidationException(
        ExceptionContext context,
        ValidationException exception)
    {
        var errors = exception.Errors
            .GroupBy(error => error.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group.Select(error => error.ErrorMessage).ToArray());

        context.Result = new BadRequestObjectResult(
            new ValidationProblemDetails(errors)
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Existem dados inválidos na requisição."
            });
    }

    private static void HandleProjectException(ExceptionContext context)
    {
        var lojaPedidosException = (LojaPedidosException)context.Exception;
        var errorResponse = new ResponseErrorJson(lojaPedidosException.GetErrors());

        context.HttpContext.Response.StatusCode = lojaPedidosException.StatusCode;
        context.Result = new ObjectResult(errorResponse);
    }

    private static void ThrowUnkowError(ExceptionContext context)
    {
        var errorResponse = new ResponseErrorJson("Erro desconhecido.");

        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = new ObjectResult(errorResponse);
    }
}
