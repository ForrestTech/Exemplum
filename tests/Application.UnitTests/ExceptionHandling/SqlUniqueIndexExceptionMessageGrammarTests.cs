namespace Application.UnitTests.ExceptionHandling
{
    using FluentAssertions;
    using Infrastructure.Persistence.ExceptionHandling.Handlers;
    using Sprache;
    using System;
    using Xunit;

    public class SqlUniqueIndexExceptionMessageGrammarTests
    {
        [Fact]
        public void Parse_should_return_unique_index_properties_for_valid_error_message()
        {
            var actual = SqlUniqueIndexExceptionGrammar.ErrorMessage
                .Parse(SqlUniqueIndexExceptionGrammar.ExampleErrorMessage);

            actual.Table.Should().Be("TodoLists");
            actual.Field.Should().Be("Title");
            actual.Value.Should().Be("Shopping");
        }
        
        [Theory]
        [InlineData("Wrong")]
        [InlineData("Cannot insert duplicate key row in object 'noperiod' with unique index 'nounderscores'. The duplicate key value is (Shopping).")]
        [InlineData("Cannot insert duplicate key row in wrong 'dbo.TodoLists' with unique index 'IX_TodoLists_Title'. The duplicate key value is (Shopping).")]
        [InlineData("Cannot insert duplicate key row in object 'dbo.TodoLists' with wrong index 'IX_TodoLists_Title'. The duplicate key value is (Shopping).")]
        [InlineData("Cannot insert duplicate key row in object 'dbo.TodoLists' with wrong index 'IX_TodoLists_Title'. The duplicate key value wrong (Shopping).")]
        public void Invalid_message_should_throw_parse_errors(string toTest)
        {
            Action actual = () => SqlUniqueIndexExceptionGrammar.ErrorMessage.Parse(toTest);

            actual.Should().Throw<ParseException>();
        }
        
        
    }
}