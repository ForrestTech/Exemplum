namespace Exemplum.UnitTests;

using Exemplum.Application.Persistence;
using FluentValidation;
using FluentValidation.Results;

public static class TestExtensions
{
    public static void SeedData(this ISpecimenBuilder fixture, Action<IApplicationDbContext> seeder)
    {
        var context = fixture.Create<IApplicationDbContext>();

        seeder(context);

        context.SaveChanges();
    }

    /// <summary>
    /// Inject a default validator that returns success for the given T 
    /// </summary>
    public static void InjectDefaultValidator<T>(this IFixture fixture)
    {
        var validator = Substitute.For<IValidator<T>>();
        validator.ValidateAsync(Arg.Any<T>(), Arg.Any<CancellationToken>()).Returns(new ValidationResult());
        fixture.Inject(validator);
    }

    public static void InjectFailingValidator<T>(this IFixture fixture, string propertyName = "sample",
        string errorMessage = "sample error")
    {
        var validator = Substitute.For<IValidator<T>>();
        validator.ValidateAsync(Arg.Any<T>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(new[] { new ValidationFailure(propertyName, errorMessage) }));
        fixture.Inject(validator);
    }
}