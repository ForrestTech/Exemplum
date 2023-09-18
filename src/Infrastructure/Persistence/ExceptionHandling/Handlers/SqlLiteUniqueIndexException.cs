namespace Exemplum.Infrastructure.Persistence.ExceptionHandling.Handlers;

using Domain.Exceptions;
using Microsoft.Data.Sqlite;
using Sprache;
using System;

public class SqlLiteUniqueIndexException : IHandlerSpecificDBException
{
    private const int ErrorCode = 19;

    public bool CanHandle(Exception exception)
    {
        return exception?.InnerException is SqliteException { SqliteErrorCode: ErrorCode };
    }

    public void HandleException(Exception exception)
    {
        if (exception?.InnerException is not SqliteException sqliteException)
        {
            return;
        }

        try
        {
            var (_, field) = SqlLiteUniqueIndexExceptionGrammar.ErrorMessage.Parse(sqliteException.Message);

            throw new BusinessException(field,
                $"Duplicate entry. An item already exists that has the same '{field}'.");
        }
        catch (ParseException)
        {
            //its find to just swallow this here as the normal DB exception will bubble up and be handled
        }
    }
}