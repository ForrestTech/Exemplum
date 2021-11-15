namespace Exemplum.UnitTests;

using Exemplum.Application.Persistence;

public static class TestExtensions
{
    public static void SeedData(this ISpecimenBuilder fixture, Action<IApplicationDbContext> seeder)
    {
        var context = fixture.Create<IApplicationDbContext>();

        seeder(context);

        context.SaveChanges();
    }
}