namespace Exemplum.Domain.Extensions;

public static class CollectionExtensions
{
    public static bool None<T>(this IEnumerable<T> collection)
    {
        return !collection.Any();
    }
}