namespace Exemplum.Infrastructure.Persistence.ExceptionHandling.Handlers;

using Application.Common.Validation;
using Npgsql;
using Sprache;

public class PostgreSQLUniqueIndexException : IHandlerSpecificDBException
{
    private const string ErrorCode = "23505";

    public bool CanHandle(Exception exception)
    {
        return exception?.InnerException is PostgresException { SqlState: ErrorCode };
    }

    public void HandleException(Exception exception)
    {
        if (exception?.InnerException is not PostgresException postgresException)
        {
            return;
        }

        try
        {
            var field  = FieldParser.Parse(postgresException.ConstraintName);

            throw new ValidationException(field,
                $"Duplicate entry. An item already exists that has the same '{field}'.");
        }
        catch (ParseException)
        {
            //its find to just swallow this here as the normal DB exception will bubble up and be handled
        }
    }
    
    private static readonly Parser<string> FieldParser = (
        from startsWith in Parse.String("IX_")
        from table in Parse.Letter.Many().Text()
        from delimiter in Parse.Char('_')
        from field in Parse.Letter.Many().Text()
        select field).Token();
}