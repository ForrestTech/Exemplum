namespace Exemplum.UnitTests
{
    using AutoFixture;
    using AutoFixture.Kernel;
    using Exemplum.Application.Persistence;
    using System;

    public static class TestExtensions
    {
        public static void SeedData(this ISpecimenBuilder fixture, Action<IApplicationDbContext> seeder)
        {
            var context = fixture.Create<IApplicationDbContext>();

            seeder(context);

            context.SaveChanges();
        }
    }
}