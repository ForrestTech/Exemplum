namespace Domain.UnitTests.Todo
{
    using Exemplum.Domain.Todo;
    using FluentAssertions;
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Drawing;
    using Xunit;

    public class ColourTests
    {
        [Fact]
        public void From_for_valid_colour_resolves_colour()
        {
            const string whiteCode = "#FFFFFF";
            
            var sut = Colour.From(whiteCode);
            
            sut.Code.Should().Be(whiteCode);
        }
        
        [Fact]
        public void From_for_invalid_colour_should_throw()
        {
            Action act = () => Colour.From("Foo");
            act.Should().Throw<UnsupportedColourException>();
        }
        
        [Theory, MemberData(nameof(ColourTestDat))]
        public void Colours_with_the_same_code_should_be_equal(Colour one, Colour two, bool areEqual)
        {
            //works with both .Equals and equality as its a value object
            var isEqual = one == two;
            isEqual.Should().Be(areEqual);
            one.Equals(two).Should().Be(areEqual);
        }
        
        public static IEnumerable<object[]> ColourTestDat => new List<object[]>
            {
                new object[] { Colour.White, Colour.White, true },
                new object[] { Colour.From("#FFFFFF"), Colour.From("#FFFFFF"), true },
                new object[] { Colour.White, Colour.Blue, false },
                new object[] { Colour.From("#FFFFFF"), Colour.From("#6666FF"), false }
            };
    }
}