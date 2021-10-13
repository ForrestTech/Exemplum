namespace Exemplum.IntegrationTests.Domain.Todo
{
    using Exemplum.Domain.Common;
    using Exemplum.Domain.Todo;
    using FluentAssertions;
    using System;
    using System.Collections.Generic;
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
            var isEqual = one.Equals(two);
            var equalOperator = one == two;
            var notEqualOperator = one != two;
            var staticEqual = ValueObject.EqualOperator(one, two);
            var staticNotEqual = ValueObject.NotEqualOperator(one, two);
            
            isEqual.Should().Be(areEqual);
            equalOperator.Should().Be(areEqual);
            staticEqual.Should().Be(areEqual);
            staticNotEqual.Should().Be(!areEqual);
            notEqualOperator.Should().Be(!areEqual);
        }
        
        public static IEnumerable<object[]> ColourTestDat => new List<object[]>
            {
                new object[] { Colour.Blue, Colour.Blue, true },
                new object[] { Colour.From("#6666FF"), Colour.From("#6666FF"), true },
                new object[] { Colour.Blue, Colour.From("#FFFFFF"), false },
                new object[] { Colour.From("#FFFFFF"), Colour.From("#6666FF"), false }
            };
    }
}