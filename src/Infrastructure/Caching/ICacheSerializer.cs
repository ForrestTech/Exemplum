namespace Exemplum.Infrastructure.Caching
{
    /// <summary>
    /// A common cache serialization abstraction for caching complex types into any given cache provider tha supports byte[]
    /// </summary>
    public interface ICacheSerializer
    {
        byte[] Serialize<T>(T obj);

        T? Deserialize<T>(byte[] bytes);
    }
}