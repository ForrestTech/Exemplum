namespace Domain.UnitTests.Todo
{
    using Domain.Todo;
    using Shouldly;
    using System.Data.Common;
    using Xunit;

    public class ColourTests
    {
        [Fact]
        public void From_for_valid_colour_resolves_colour()
        {
            const string whiteCode = "#FFFFFF";
            
            var sut = Colour.From(whiteCode);
            
            sut.Code.ShouldBe(whiteCode);
        }
        
        [Fact]
        public void From_for_invalid_colour_should_throw()
        {
            Should.Throw<UnsupportedColourException>(() => Colour.From("Foo"));
        }
    }
}