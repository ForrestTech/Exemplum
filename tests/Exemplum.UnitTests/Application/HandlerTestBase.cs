namespace Application.UnitTests.Application
{
    using AutoFixture;
    using AutoFixture.AutoNSubstitute;
    using AutoMapper;
    using Exemplum.Application.Common.Mapping;
    using Exemplum.Application.Persistence;
    using Exemplum.Infrastructure.Persistence;
    using Microsoft.EntityFrameworkCore;
    using System.Reflection;

    public class HandlerTestBase
    {
        protected static IFixture SetupFixture()
        {
            var fixture = new Fixture()
                .Customize(new AutoNSubstituteCustomization());

            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("Exemplum").Options;
            var context = new ApplicationDbContext(dbContextOptions);

            fixture.Inject<IApplicationDbContext>(context);

            var mappingConfiguration =
                new MapperConfiguration(cfg => cfg.AddMaps(Assembly.GetAssembly(typeof(MappingProfile))));
            fixture.Inject<IMapper>(mappingConfiguration.CreateMapper());
            return fixture;
        }
    }
}