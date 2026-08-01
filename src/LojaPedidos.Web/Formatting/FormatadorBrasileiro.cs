using System.Globalization;

namespace LojaPedidos.Web.Formatting;

public static class FormatadorBrasileiro
{
    public static string FormatarMoeda(decimal valor)
    {
        var numero = valor.ToString("N2", CultureInfo.InvariantCulture)
            .Replace(",", "#")
            .Replace(".", ",")
            .Replace("#", ".");

        return $"R$ {numero}";
    }
}
