using FluentValidation;
using LojaPedidos.Application.Common.Exceptions;
using LojaPedidos.Application.Common.Responses;
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
        context.Result = new BadRequestObjectResult(
            ApiResponse<object?>.Erro(string.Join(
                " ",
                exception.Errors.Select(error => error.ErrorMessage).Distinct())));
    }

    private static void HandleProjectException(ExceptionContext context)
    {
        var lojaPedidosException = (LojaPedidosException)context.Exception;
        var errorResponse = ApiResponse<object?>.Erro(
            string.Join(" ", lojaPedidosException.GetErrors()));

        context.HttpContext.Response.StatusCode = lojaPedidosException.StatusCode;
        context.Result = new ObjectResult(errorResponse);
    }

    private static void ThrowUnkowError(ExceptionContext context)
    {
        var errorResponse = ApiResponse<object?>.Erro("Erro desconhecido.");

        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = new ObjectResult(errorResponse);
    }
}
