namespace Exemplum.Infrastructure.Caching
{
    using System.Text;
    using System.Text.Json;

    public class Utf8JsonCacheSerializer : ICacheSerializer
    {
        public byte[] Serialize<T>(T obj)
        {
            return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(obj));
        }

        public T? Deserialize<T>(byte[] bytes)
        {
            return JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(bytes));
        }
    }
}