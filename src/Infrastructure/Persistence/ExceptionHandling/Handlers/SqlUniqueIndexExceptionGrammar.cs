namespace Exemplum.Infrastructure.Persistence.ExceptionHandling.Handlers
{
    using Sprache;

    /// <summary>
    /// While this is very verbose I much prefer it to regex.  While verbose a set of parsers are very explicit
    /// </summary>
    public static class SqlUniqueIndexExceptionGrammar
    {
        private const string ErrorStartsWith = "Cannot insert duplicate key row in object ";

        public const string ExampleErrorMessage = "Cannot insert duplicate key row in object 'dbo.TodoLists' with unique index 'IX_TodoLists_Title'. The duplicate key value is (Shopping).";

        public static readonly Parser<UniqueIndexErrorProperties> ErrorMessage = (
            from startsWith in Parse.String(ErrorStartsWith)
            from tableDetails in DataBaseTable
            from withUniqueIndex in Parse.String(" with unique index ")
            from indexDetails in IndexDetails
            from duplicateKey in Parse.String(". The duplicate key value is ")
            from value in BracketedString
            select new UniqueIndexErrorProperties(tableDetails.Table, indexDetails.Field, value)).Token();

        private static readonly Parser<DatabaseTableDetails> DataBaseTable =
            from open in Parse.Char('\'')
            from schema in Parse.Letter.Many().Text()
            from delimiter in Parse.Char('.')
            from table in Parse.Letter.Many().Text()
            from close in Parse.Char('\'')
            select new DatabaseTableDetails(table, schema);

        private static readonly Parser<DatabaseIndexDetails> IndexDetails =
            from open in Parse.Char('\'')
            from indexIdentifier in Parse.Letter.Many().Text()
            from delimiter in Parse.Char('_')
            from tableName in Parse.Letter.Many().Text()
            from delimiter2 in Parse.Char('_')
            from fieldName in Parse.Letter.Many().Text()
            from close in Parse.Char('\'')
            select new DatabaseIndexDetails(tableName, fieldName);

        private static readonly Parser<string> BracketedString =
            from open in Parse.Char('(')
            from value in Parse.CharExcept(')').Many().Text()
            from close in Parse.Char(')')
            select value;

        public record UniqueIndexErrorProperties(string Table, string Field, string Value)
        {
        }

        private record DatabaseTableDetails(string Table, string Schema)
        {
        }

        private record DatabaseIndexDetails(string Table, string Field)
        {
        }
    }
}