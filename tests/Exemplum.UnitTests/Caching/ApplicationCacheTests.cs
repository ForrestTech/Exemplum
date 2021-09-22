namespace Application.UnitTests.Caching
{
    using AutoFixture;
    using AutoFixture.AutoNSubstitute;
    using Exemplum.Infrastructure.Caching;
    using FluentAssertions;
    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.Extensions.Options;
    using NSubstitute;
    using NSubstitute.ExceptionExtensions;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;

    public class ApplicationCacheTests
    {
        [Fact]
        public async Task GetOrAddAsync_returns_value_from_the_cache()
        {
            var fixture = CreateFixture(CreateCacheOptions());

            var cache = fixture.Freeze<IDistributedCache>();
            var cacheData = fixture.Create<byte[]>();
            cache.GetAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult<byte[]>(cacheData));

            var serializer = fixture.Freeze<ICacheSerializer>();
            var cacheItem = fixture.Create<CacheItem>();
            serializer.Deserialize<CacheItem>(Arg.Any<byte[]>()).Returns(cacheItem);

            var sut = fixture.Create<ApplicationCache<CacheItem>>();

            var actual = await sut.GetOrAddAsync("key", () => Task.FromResult(new CacheItem()));

            actual.Name.Should().Be(cacheItem.Name);
        }

        [Fact]
        public async Task GetOrAddAsync_will_update_cache_when_cache_is_empty()
        {
            var fixture = CreateFixture(CreateCacheOptions());

            var cache = fixture.Freeze<IDistributedCache>();

            cache.GetAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult<byte[]>(null));

            var sut = fixture.Create<ApplicationCache<CacheItem>>();

            const string key = "key";
            var actual = await sut.GetOrAddAsync(key, () => Task.FromResult(new CacheItem()));

            await cache.Received(2)
                .GetAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());
            await cache.Received()
                .SetAsync(Arg.Any<string>(), Arg.Any<byte[]>(), Arg.Any<DistributedCacheEntryOptions>(),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task GetOrAddAsync_will_return_factory_item_when_cache_is_empty()
        {
            var fixture = CreateFixture(CreateCacheOptions());

            var cache = fixture.Freeze<IDistributedCache>();

            cache.GetAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult<byte[]>(null));

            var sut = fixture.Create<ApplicationCache<CacheItem>>();

            const string key = "key";
            var factoryItem = fixture.Create<CacheItem>();
            var actual = await sut.GetOrAddAsync(key, () => Task.FromResult(factoryItem));

            actual.Should().BeEquivalentTo(factoryItem);
        }

        [Fact]
        public async Task GetAsync_calls_get_on_cache_client()
        {
            var fixture = CreateFixture(CreateCacheOptions());

            var cache = fixture.Freeze<IDistributedCache>();

            var sut = fixture.Create<ApplicationCache<CacheItem>>();

            const string key = "key";
            await sut.GetAsync(key);

            await cache.Received()
                .GetAsync(Arg.Is<string>(x => x.Contains(key)),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task GetAsync_returns_null_when_distributed_cache_returns_null()
        {
            var fixture = CreateFixture(CreateCacheOptions());

            var cache = fixture.Freeze<IDistributedCache>();
            cache.GetAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult<byte[]>(null));

            var sut = fixture.Create<ApplicationCache<CacheItem>>();

            var actual = await sut.GetAsync("key");

            actual.Should().BeNull();
        }

        [Fact]
        public async Task GetAsync_calls_serializer_when_cache_returns_data()
        {
            var fixture = CreateFixture(CreateCacheOptions());

            var cache = fixture.Freeze<IDistributedCache>();
            var cacheData = fixture.Create<byte[]>();
            cache.GetAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(cacheData));

            var serializer = fixture.Freeze<ICacheSerializer>();

            var sut = fixture.Create<ApplicationCache<CacheItem>>();

            await sut.GetAsync("key");

            serializer.Received()
                .Deserialize<CacheItem>(Arg.Is(cacheData));
        }

        [Fact]
        public void GetAsync_throw_exception_when_cache_does()
        {
            var fixture = CreateFixture(CreateCacheOptions());

            var cache = fixture.Freeze<IDistributedCache>();
            cache.GetAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Throws(new Exception());

            var sut = fixture.Create<ApplicationCache<CacheItem>>();

            Func<Task> act = async () => await sut.GetAsync("key");

            act.Should().ThrowAsync<Exception>();
        }

        [Fact]
        public async Task GetAsync_returns_null_when_cache_exception_are_hidden()
        {
            var fixture = CreateFixture(CreateCacheOptions());

            var cache = fixture.Freeze<IDistributedCache>();
            cache.GetAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Throws(new Exception());

            var sut = fixture.Create<ApplicationCache<CacheItem>>();

            var actual = await sut.GetAsync("key", hideErrors: true);

            actual.Should().Be(null);
        }

        [Fact]
        public async Task SetAsync_swallows_exceptions_when_cache_exception_are_hidden()
        {
            var fixture = CreateFixture(CreateCacheOptions());

            var cache = fixture.Freeze<IDistributedCache>();
            cache.SetAsync(Arg.Any<string>(), Arg.Any<byte[]>(), Arg.Any<DistributedCacheEntryOptions>(), Arg.Any<CancellationToken>())
                .Throws(new Exception());

            var sut = fixture.Create<ApplicationCache<CacheItem>>();

            await sut.SetAsync("key", new CacheItem(), hideErrors: true);
        }
        
        [Fact]
        public async Task SetAsync_throws_cache_exceptions()
        {
            var fixture = CreateFixture(CreateCacheOptions());

            var cache = fixture.Freeze<IDistributedCache>();
            cache.SetAsync(Arg.Any<string>(), Arg.Any<byte[]>(), Arg.Any<DistributedCacheEntryOptions>(), Arg.Any<CancellationToken>())
                .Throws(new Exception());

            var sut = fixture.Create<ApplicationCache<CacheItem>>();

            Func<Task> act = async () => await sut.SetAsync("key", new CacheItem());
            await act.Should().ThrowAsync<Exception>();
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
            var options = new CacheOptions
            {
                KeyPrefix = "Exemplum",
                HideErrors = false,
                GlobalCacheEntryOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                }
            };
            
            var cacheItemConfigurators = new Dictionary<Type, DistributedCacheEntryOptions>
            {
                {
                    typeof(CacheItem),
                    new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) }
                }
            };
            
            options.CacheConfigurators.Add(cacheItemType =>
                cacheItemConfigurators.TryGetValue(cacheItemType, out DistributedCacheEntryOptions cacheItemEntryOptions)
                    ? cacheItemEntryOptions
                    : null);

            return options;
        }

        private static IFixture CreateFixture(IOptions<CacheOptions> cacheOptions)
        {
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            fixture.Inject(cacheOptions);
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