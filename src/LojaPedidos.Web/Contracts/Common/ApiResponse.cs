namespace LojaPedidos.Web.Contracts.Common;

public sealed record ApiResponse<T>(
    string Mensagem,
    T? Dados,
    bool Sucesso);
