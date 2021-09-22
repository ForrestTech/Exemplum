namespace Exemplum.Infrastructure.Caching
{
    using Application.Common.Caching;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// A common abstraction that can be used to wrap almost any caching provider.  Make it easier to code application caching logic
    /// </summary>
    public interface ICacheProvider
    {
        Task<byte[]?> GetAsync(string key,
            CancellationToken cancellationToken = default(CancellationToken));

        Task SetAsync(string key,
            byte[] value,
            CacheEntryOptions options,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}