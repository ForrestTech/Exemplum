namespace Exemplum.Domain.Extensions;

public static class StringExtensions
{
    public static bool HasValue(this string value)
    {
        return !string.IsNullOrWhiteSpace(value);
    }

    public static bool HasNoValue(this string value)
    {
        return !value.HasValue();
    }

    /// <summary>
    /// Check if string match ignoring culture and case
    /// </summary>
    public static bool IsTheSameAs(this string value, string toCompare)
    {
        return string.Equals(value, toCompare, StringComparison.InvariantCultureIgnoreCase);
    }
}