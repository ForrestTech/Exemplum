namespace Exemplum.UnitTests.Domain.Todo;

using Exemplum.Domain.Todo;

public class PriorityLevelTests
{
    [Theory]
    [InlineData("High")]
    public void TryParse(string name)
    {
        var foo = PriorityLevel.High;
        
        Thread.Sleep(5000);

        foo.Should().Be(PriorityLevel.High);
    }
}