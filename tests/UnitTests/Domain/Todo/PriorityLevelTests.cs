namespace Exemplum.UnitTests.Domain.Todo;

using Exemplum.Domain.Todo;

public class PriorityLevelTests
{
    [Theory]
    [InlineData("High", true)]
    [InlineData("Medium", true)]
    [InlineData("Low", true)]
    [InlineData("None", true)]
    [InlineData("Failed", false)]
    [InlineData("", false)]
    public void TryParse(string name, bool pass)
    {
        var (success, _) = PriorityLevel.TryParse(name);

        success.Should().Be(pass);
    }
}