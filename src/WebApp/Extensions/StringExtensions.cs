namespace Exemplum.WebApp.Extensions
{
    public static class StringExtensions
    {
        public static bool HasNoValue(this string value)
        {
            return !value.HasValue();
        }
        
        public static bool HasValue(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }
    }
}