namespace Exemplum.Domain.Extensions
{
    using System.Collections.Generic;
    using System.Linq;

    public static class CollectionExtensions
    {
        public static bool None<T>(this IEnumerable<T> collection)
        {
            return  !collection.Any();
        }
    }
}