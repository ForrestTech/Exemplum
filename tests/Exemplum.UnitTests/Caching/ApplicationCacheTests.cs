namespace Application.UnitTests.Caching
{
    using AutoFixture;
    using AutoFixture.AutoNSubstitute;
    using Exemplum.Infrastructure.Caching;
    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.Extensions.Options;
    using NSubstitute;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;

    public class ApplicationCacheTests
    {
        [Fact]
        public async Task GetAsync_calls_get_on_distributed_cache_client()
        {
            var fixture = CreateFixture();

            fixture.Inject(CreateCacheOptions());

            var cache = fixture.Freeze<IDistributedCache>();

            var sut = fixture.Create<ApplicationCache<CacheItem>>();

            const string key = "key";
            await sut.GetAsync(key);

            await cache.Received()
                .GetAsync(Arg.Is<string>(x => x.Contains(key)),
                    Arg.Any<CancellationToken>());
        }

        private static IOptions<CacheOptions> CreateCacheOptions(Action<CacheOptions> optionBuilder = null)
        {
            var options = CreateDefaultOptions();
            if (optionBuilder != null)
            {
                optionBuilder(options);
            }

            var optionsWrapper = new OptionsWrapper<CacheOptions>(CreateDefaultOptions());
            return optionsWrapper;
        }

        private static CacheOptions CreateDefaultOptions()
        {
            return new CacheOptions
            {
                KeyPrefix = "Exemplum",
                HideErrors = false,
                GlobalCacheEntryOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                }
            };
        }

        private static IFixture CreateFixture()
        {
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            return fixture;
        }

        public class CacheItem
        {
            public string Name { get; set; }

            public int Number { get; set; } = 10;

            public string Foo { get; set; }
        }
    }
}