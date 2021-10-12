namespace Exemplum.Domain.Extensions
{
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
    }
}