namespace LojaPedidos.Domain.ValueObjects;

public static class Cpf
{
    public static string Normalizar(string? valor)
    {
        return string.IsNullOrWhiteSpace(valor)
            ? string.Empty
            : new string(valor.Where(char.IsDigit).ToArray());
    }

    public static bool EhValido(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor)
            || valor.Any(caractere =>
                !char.IsDigit(caractere)
                && caractere is not ('.' or '-' or ' ')))
        {
            return false;
        }

        var cpf = Normalizar(valor);

        if (cpf.Length != 11 || cpf.Distinct().Count() == 1)
        {
            return false;
        }

        return CalcularDigito(cpf, 9) == cpf[9] - '0'
            && CalcularDigito(cpf, 10) == cpf[10] - '0';
    }

    private static int CalcularDigito(string cpf, int quantidadeDigitos)
    {
        var soma = 0;

        for (var indice = 0; indice < quantidadeDigitos; indice++)
        {
            soma += (cpf[indice] - '0') * (quantidadeDigitos + 1 - indice);
        }

        var resto = soma % 11;
        return resto < 2 ? 0 : 11 - resto;
    }
}
