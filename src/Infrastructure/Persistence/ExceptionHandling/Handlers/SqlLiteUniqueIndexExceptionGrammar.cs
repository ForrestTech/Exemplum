namespace Exemplum.Infrastructure.Persistence.ExceptionHandling.Handlers;

using Sprache;

public static class SqlLiteUniqueIndexExceptionGrammar
{
    public const string ExampleErrorMessage =
        "SQLite Error 19: 'UNIQUE constraint failed: TodoLists.Title'.";
    
    private const string ErrorStartsWith = "SQLite Error 19: 'UNIQUE constraint failed: ";
    
    public static readonly Parser<UniqueIndexErrorProperties> ErrorMessage = (
        from startsWith in Parse.String(ErrorStartsWith)
        from table in Parse.Letter.Many().Text()
        from delimiter in Parse.Char('.')
        from field in Parse.Letter.Many().Text()
        from end in  Parse.Char('\'')
        select new UniqueIndexErrorProperties(table, field)).Token();
    
    public record UniqueIndexErrorProperties(string Table, string Field)
    {
    }
}