namespace LojaPedidos.Web.Contracts.Common;

public sealed record ApiProblem(
    string? Title,
    string? Detail,
    int? Status,
    IReadOnlyDictionary<string, string[]>? Errors);
