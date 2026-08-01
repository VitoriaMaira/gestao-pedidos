using System.Net;

namespace LojaPedidos.Web.Contracts.Common;

public sealed record ApiResult<T>(
    bool Sucesso,
    T? Dados,
    string? Mensagem,
    HttpStatusCode? StatusCode = null,
    IReadOnlyDictionary<string, string[]>? Erros = null)
{
    public static ApiResult<T> Ok(T dados) => new(true, dados, null);

    public static ApiResult<T> Falha(
        string mensagem,
        HttpStatusCode? statusCode = null,
        IReadOnlyDictionary<string, string[]>? erros = null) =>
        new(false, default, mensagem, statusCode, erros);
}
