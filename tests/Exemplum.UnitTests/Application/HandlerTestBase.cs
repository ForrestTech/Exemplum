namespace Application.UnitTests.Application
{
    using AutoFixture;
    using AutoFixture.AutoNSubstitute;
    using AutoMapper;
    using Exemplum.Application.Common.Mapping;
    using Exemplum.Application.Persistence;
    using Exemplum.Infrastructure.Persistence;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Reflection;

    public class HandlerTestBase 
    {
        protected virtual IFixture CreateFixture()
        {
            var fixture = new Fixture()
                .Customize(new AutoNSubstituteCustomization());

            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase($"ExemplumTestDb_{Guid.NewGuid()}").Options;
            
            var context = new ApplicationDbContext(dbContextOptions);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            fixture.Inject<IApplicationDbContext>(context);

            var mappingConfiguration = new MapperConfiguration(config => 
                config.AddMaps(Assembly.GetAssembly(typeof(MappingProfile))));
            
            fixture.Inject<IMapper>(mappingConfiguration.CreateMapper());
            return fixture;
        }
    }
}