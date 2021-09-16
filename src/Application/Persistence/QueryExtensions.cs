namespace Exemplum.Application.Persistence
{
    using System.Linq;

    public static class QueryExtensions
    {
        public static IQueryable<T> Query<T>(this IQueryable<T> query, IQueryObject<T> queryObject) where T : class
        {
            query = queryObject.ApplyQuery(query);

            return query;
        }
    }
    
    public interface IQueryObject<T>
    {
        public IQueryable<T> ApplyQuery(IQueryable<T> query);
    }
}