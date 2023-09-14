namespace Exemplum.UnitTests.Application.Exceptions;

using Infrastructure.Persistence.ExceptionHandling.Handlers;
using Sprache;

public class SqlLiteUniqueIndexExceptionMessageGrammarTests
{
    [Fact]
    public void Parse_should_return_unique_index_properties_for_valid_error_message()
    {
        var actual = SqlLiteUniqueIndexExceptionGrammar.ErrorMessage
            .Parse(SqlLiteUniqueIndexExceptionGrammar.ExampleErrorMessage);

        actual.Table.Should().Be("TodoLists");
        actual.Field.Should().Be("Title");
    }

    [Theory]
    [InlineData("Wrong")]
    [InlineData(
        "SQLite Error 19: 'UNIQUE constraint failed wrong: TodoLists.Title'.")]
    [InlineData(
        "SQLite Error 19: 'UNIQUE constraint failed: TodoLis/ts.Title'.")]
    [InlineData(
        "SQLite Error 19: 'UNIQUE constraint failed: TodoLists.Ti/tle'.")]
    [InlineData(
        "SQLite Error 19: 'UNIQUE constraint failed: TodoLists.Title.")]
    public void Invalid_message_should_throw_parse_errors(string toTest)
    {
        Action actual = () => SqlLiteUniqueIndexExceptionGrammar.ErrorMessage.Parse(toTest);

        actual.Should().Throw<ParseException>();
    }
}