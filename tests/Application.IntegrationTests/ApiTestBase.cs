namespace Application.IntegrationTests
{
    using WebApi;
    using Xunit;
    using Xunit.Abstractions;

    public class ApiTestBase : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        protected readonly CustomWebApplicationFactory<Startup> Factory;
        protected readonly ITestOutputHelper TestOutputHelper;

        protected ApiTestBase(CustomWebApplicationFactory<Startup> factory, ITestOutputHelper testOutputHelper)
        {
            Factory = factory;
            TestOutputHelper = testOutputHelper;
        }
    }
}