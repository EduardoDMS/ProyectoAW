using System.Globalization;

namespace ProyectoAW.Infrastructure.Data;

public static class DateTimeText
{
    public const string Format = "yyyy-MM-dd HH:mm";

    public static string From(DateTime value) =>
        value.ToString(Format, CultureInfo.InvariantCulture);

    public static bool TryNormalize(string? value, out string normalized)
    {
        if (DateTime.TryParse(value, CultureInfo.InvariantCulture,
                DateTimeStyles.RoundtripKind, out var parsed))
        {
            normalized = From(parsed);
            return true;
        }

        normalized = string.Empty;
        return false;
    }
}
