namespace Exemplum.UnitTests;

public abstract class TestBase
{
    protected virtual IFixture CreateFixture()
    {
        var fixture = new Fixture()
            .Customize(new AutoNSubstituteCustomization());

        return fixture;
    }
}