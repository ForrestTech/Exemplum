namespace Exemplum.IntegrationTests.Application
{
    using AutoFixture;
    using AutoFixture.AutoNSubstitute;
    using AutoMapper;
    using Exemplum.Application.Common.DomainEvents;
    using Exemplum.Application.Common.Identity;
    using Exemplum.Application.Common.Mapping;
    using Exemplum.Application.Persistence;
    using Infrastructure.DateAndTime;
    using Infrastructure.Persistence;
    using Infrastructure.Persistence.ExceptionHandling;
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

            var context = new ApplicationDbContext(dbContextOptions,
                fixture.Create<IHandleDbExceptions>(),
                fixture.Create<IPublishDomainEvents>(),
                new Clock(),
                fixture.Create<ICurrentUser>());

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            fixture.Inject<IApplicationDbContext>(context);

            var mappingConfiguration = new MapperConfiguration(config =>
                config.AddMaps(Assembly.GetAssembly(typeof(MappingProfile))));

            fixture.Inject(mappingConfiguration.CreateMapper());
            
            return fixture;
        }
    }
}